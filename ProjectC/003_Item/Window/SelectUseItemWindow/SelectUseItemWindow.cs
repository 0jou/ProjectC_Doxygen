using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

using SelectUIInfo;
using ItemInfo;
using SelectUseItemInfo;


namespace SelectUseItemInfo
{
    public enum SelectUseItemWindowType
    {
        Inventory = 0,
        Action = 1,
    }

    public enum SelectUseItemID
    {
        Put,
        Pitch,//投げる（山本）
        Eat,
        Exit,
    }
}

public partial class SelectUseItemWindow : BaseWindow
{


    // 選択した作成アイテムの使用用途を決めるウィンドウ
    // 制作者(田内)

    [Header("UIコントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;


    [Header("親とするUI")]
    [SerializeField]
    private GameObject m_parentUI = null;


    [Header("作成するボタンプレハブ")]
    [SerializeField]
    private GameObject m_button = null;


    // 返す使用用途ID
    private SelectUseItemID m_currentSelectUseItemID = new();


    //=================================================================================


    public void SetData(SelectUseItemWindowType _windowType, ItemTypeID _typeID, uint _id)
    {
        switch (_windowType)
        {

            case SelectUseItemWindowType.Inventory:
                {
                    EditInventoryWindow(_typeID, _id);
                    break;
                }

            case SelectUseItemWindowType.Action:
                {
                    EditActionWindow(_typeID, _id);
                    break;
                }


        }
    }




    public new async UniTask<SelectUseItemID> OnUpdate()
    {

        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerが登録されていません");
            return SelectUseItemID.Exit;
        }


        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // ボタンを押すまで
            while (cancelToken.IsCancellationRequested == false)
            {

                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();


                m_selectUIController.OnUpdate();

                // ボタンが選択されたら
                if (CheckPressSelectUseItemButton())
                {
                    return m_currentSelectUseItemID;
                }

                // キャンセル
                if (PlayerInputManager.instance.IsInputActionWasPressed(InputActionMapTypes.UI, "Cancel"))
                {
                    return SelectUseItemID.Exit;
                }

                m_selectUIController.OnLateUpdate();

                await UniTask.DelayFrame(1);

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

        return default;

    }


    // ボタンが選択されたかを確認するメソッド 
    private bool CheckPressSelectUseItemButton()
    {
        if (m_selectUIController == null)
        {
            Debug.LogError("SelectUIControllerが登録されていません");
            return false;
        }

        // 選択されれば
        if (!m_selectUIController.IsPress) return false;

        var ui = m_selectUIController.CurrentSelectUI;
        if (ui == null)
        {
            return false;
        }

        //==================

        var button = ui.GetComponent<SelectUseItemButton>();
        if (button?.IsCanPress == false)
        {
            return false;
        }

        // 返すIDをセット
        m_currentSelectUseItemID = button.ID;


        return true;

    }



    // 使用用途を決めるボタンを作成する
    private void AddButton(SelectUseItemID _id, bool _active, string _text)
    {
        if (m_selectUIController == null)
        {
            Debug.LogError("UI選択コントローラーが存在しません");
            return;
        }


        if (m_parentUI == null)
        {
            Debug.LogError("親にするUIが存在しません");
            return;
        }

        if (m_button == null)
        {
            Debug.LogError("作成するボタンが存在しません");
            return;
        }

        // ボタンを作成(親指定)
        var obj = Instantiate(m_button, m_parentUI.transform);


        var button = obj.GetComponentInChildren<SelectUseItemButton>();
        if (button == null)
        {
            Debug.LogError("SelectUseItemButtonコンポーネントが存在しません");
            return;
        }

        button.SetData(_id, _active, _text);


        // UIコントローラーに追加
        m_selectUIController.AddUI(obj,SelectUIType.Press, 1);

    }

}
