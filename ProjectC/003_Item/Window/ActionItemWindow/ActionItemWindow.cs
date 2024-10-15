using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 制作者(田内)

public class ActionItemWindow : BaseWindow
{

    [Header("スロット作成")]
    [SerializeField]
    private CreatePocketItemSlotList m_createSlotList = null;


    [Header("UI選択ウィンドウコントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("スクロール")]
    [SerializeField]
    private ChangeScrollViewPosition m_changeScrollViewPosition = null;


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

        }
        catch (System.OperationCanceledException ex)
        {

        }
    }


    public override async UniTask OnUpdate()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // ボタンを押すまで
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

                // UI選択の更新
                m_selectUIController.OnUpdate();

                // スクロールビューの更新
                m_changeScrollViewPosition.OnUpdate();

                // キャンセル
                if (PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.UI, "Cancel"))
                {
                    //SE追加（山本）
                    SoundManager.Instance.StartPlayback("Cancel");
                    return;
                }

                // 選択ウィンドウの作成
                if ( CreateSelectUseCraftItemWindow()) return;

                // UI選択の後処理
                m_selectUIController.OnLateUpdate();


                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();

            }

        }
        catch (System.OperationCanceledException ex)
        {

        }
    }




    private bool CreateSelectUseCraftItemWindow()
    {
      
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIコントローラーが登録されていません");
            return false;
        }

        // 選択したか
        if (!m_selectUIController.IsPress) return false;

        // 選択しているUIがなければ
        if (!m_selectUIController.CurrentSelectUI) return false;

        // 選択しているUIがスロットであれば
        var slotData = m_selectUIController.CurrentSelectUI.GetComponent<ItemSlotData>();
        if (slotData == null)
        {
            return false;
        }

        // アイテム所持状態へ
        // プレイヤーへアイテム情報を送る(吉田)
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.GetComponent<CharacterCore>()?.PlayerParameters.
                SetPutItemInfo(slotData.ItemTypeID, slotData.ItemID);

            var animator = player.GetComponent<CharacterCore>().m_animator;

            animator.SetBool("IsHoldItem", true);

            //武器所持状態なら武器を消失
            player.GetComponent<CharacterCore>().PlayerParameters.HideWeapon(animator);

        }

        return true;

    }

}
