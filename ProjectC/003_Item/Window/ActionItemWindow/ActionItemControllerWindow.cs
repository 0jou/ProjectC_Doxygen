using Cysharp.Threading.Tasks;
using ItemInfo;
using PocketItemDataInfo;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

//新規アクションアイテムウィンドウ（山本）

public class ActionItemControllerWindow : BaseWindow
{
    [Header("スロット作成")]
    [SerializeField]
    private CreateActionItemSlotList m_createSlotList = null;
    public CreateActionItemSlotList CreateSlotList { get { return m_createSlotList; } }


    [Header("UI選択ウィンドウコントローラー")]
    [SerializeField]
    private SelectUIActionWindowController m_selectUIController = null;

    [Header("スクロール")]
    [SerializeField]
    private ChangeScrollViewPosition m_changeScrollViewPosition = null;

    [Header("スプライトの配置場所")]
    [SerializeField]
    private GameObject m_content = null;
    public GameObject ContentTrans { get { return m_content; } }


    public override async UniTask OnInitialize()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            
            await base.OnInitialize();
            cancelToken.ThrowIfCancellationRequested();

            if (m_createSlotList == null)
            {
                Debug.LogError("CreateItemSlotListコンポーネントがアタッチされていません");
                return;
            }

            // スロット作成
            m_createSlotList.OnInitialize();

            await UniTask.CompletedTask;

        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }

    public override async UniTask OnUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            while (cancelToken.IsCancellationRequested == false)
            {

                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                if (m_selectUIController == null)
                {
                    Debug.LogError("SlotContorollerコンポーネントがアタッチされていません");
                    return;
                }

                if (m_input == null)
                {
                    Debug.LogError("PlayerInputコンポーネントがアタッチされていません");
                    return;
                }

                if (m_changeScrollViewPosition == null)
                {
                    Debug.LogError("ChangeScrollViewPosコンポーネントがアタッチされていません");
                    return;
                }


                //ActionItemWindowの情報更新
                SetItemInfomation();

                //スプライトとUIの削除の監視（山本）
                DeleteSprite();

                //スロットとUIリストの作成
                m_createSlotList.CreateSlot();

                if (CheckNullUIgameObjList())
                {
                    //TODo
                    //時間停止中は入らないようにする（山本）
                    if (Time.timeScale != 0.0f)
                    {
                        // UI選択の更新
                        m_selectUIController.OnUpdate();
                    }

                    // スクロールビューの更新
                    m_changeScrollViewPosition.OnUpdateEveryTime();


                    if (Time.timeScale != 0.0f)
                    {
                        //アイテムアクションへと移行
                        SelectItemAction();
                    }

                }

                // UI選択の後処理
                m_selectUIController.OnLateUpdate();

                await UniTask.DelayFrame(1);

                

                cancelToken.ThrowIfCancellationRequested();

               

            }


        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }


    //ActionItemWindowの情報登録
    private void SetItemInfomation()
    {
        foreach (var slot in m_createSlotList.SlotList)
        {
            if (slot == null) continue;

            // 注文アイテムスロットのデータをセット
            var itemData = ItemDataBaseManager.instance.GetItemData(ItemTypeID.Food, slot.ItemID);
            slot.SetItemSlotData(itemData, PocketType.Inventory);

        }
    }

    //スプライトとUIの削除の監視（山本）
    private void DeleteSprite()
    {
        //アイテムウィンドウ数の監視とスプライトの削除
        foreach (var obj in m_createSlotList.SlotList)
        {
            
            if (obj == null)
            {
                continue;
            }

            var _data = obj?.GetComponent<ItemSlotData>();

            if (_data == null)
            {
                break;
            }

            var inventryList = InventoryManager.instance.GetItemNum(_data.ItemTypeID, _data.ItemID);

            if (inventryList == 0)
            {
                //削除
                Destroy(obj.gameObject);
            }
        }

        var uiList = m_selectUIController.UIList;

        //NullのUIリストを削除する


        for (int i = 0; i < uiList.Count; i++)
        {
            if (uiList[i] == null) continue;

            //削除前のリストのカウント数
            var preDeleteListCount = uiList[i].List.Count;

            //リスト内のnullを削除
            uiList[i].List.RemoveAll(ui => ui == null);

            //削除前後でカウント数が変化していたら
            if (preDeleteListCount != uiList[i].List.Count)
            {
                m_selectUIController.CheckWithChange();
                //カーソル位置の更新
                m_selectUIController.SetUIActionWindowGameObject();
            }
        }



    }


    //UIリスト内にオブジェクトが残っているか確認する処理（山本）
    private bool CheckNullUIgameObjList()
    {
        var uiList = m_selectUIController.UIList;

        bool nullFlg = false;

        for (int i = 0; i < uiList.Count; i++)
        {
            //uiList内のuIGameObjectList内にオブジェクトが含まれていたならtrueを返す
            if (uiList[i].List.Count != 0)
            {
                nullFlg = true;
                break;
            }
        }

        return nullFlg;

    }

    private void SelectItemAction()
    {
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIコントローラーが登録されていません");
            return;
        }


        //UIリストとスロットが空ならリターン
        if ((m_selectUIController.UIList.Count == 0) || (m_createSlotList.SlotList.Count == 0))
        {
            return;
        }

        // 選択しているUIがなければリターン
        if (!m_selectUIController.CurrentSelectUI)
        {
            return;
        }

        // 選択しているUIがスロットになければリターン
        var slotData = m_selectUIController.CurrentSelectUI.GetComponent<ItemSlotData>();
        if (slotData == null)
        {
            return;
        }

        // プレイヤーへアイテム情報を送る処理(吉田)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            return;
        }

        if (player.TryGetComponent(out CharacterCore characterCore))
        {

            var animator = characterCore.m_animator;

            if (animator == null)
            {
                return;
            }


            if (characterCore.PlayerParameters == null)
            {
                return;
            }

            //選択したアイテム情報をプレイヤーに渡す
            characterCore.PlayerParameters.SetPutItemInfo(slotData.ItemTypeID, slotData.ItemID);



            //料理を置く
            if (m_selectUIController.IsPut)
            {
                if (!animator.GetBool("IsPutItem"))
                {
                    animator.SetBool("IsPutItem", true);
                    //武器所持状態なら武器を消失
                    player.GetComponent<CharacterCore>().PlayerParameters.HideWeapon(animator);
                    return;
                }
            }

            //料理を置く
            if (m_selectUIController.IsUse)
            {
                var data = ItemDataBaseManager.instance.GetItemData<FoodData>(slotData.ItemTypeID, slotData.ItemID);

                if (data == null)
                {
                    return;
                }

                //食べられるなら食べる状態へ移行
                if (data.IsEat && !animator.GetBool("IsEatItem"))
                {
                    animator.SetBool("IsEatItem", true);
                    //武器所持状態なら武器を消失
                    player.GetComponent<CharacterCore>().PlayerParameters.HideWeapon(animator);
                    return;
                }
                else
                {
                    return;
                }

            }

            //料理を投げる
            if (m_selectUIController.IsThrow)
            {

                if (!animator.GetBool("IsThrow"))
                {
                    animator.SetBool("IsThrow", true);
                    //武器所持状態なら武器を消失
                    player.GetComponent<CharacterCore>().PlayerParameters.HideWeapon(animator);

                    return;
                }
            }


        }

    }



}



