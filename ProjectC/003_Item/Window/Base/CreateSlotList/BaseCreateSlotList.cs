using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SelectUIInfo;
using PocketItemDataInfo;

[RequireComponent(typeof(GridLayoutGroup))]
public class BaseCreateSlotList : MonoBehaviour
{

    // 制作者(田内)

    //===================================================================================

    protected enum CreateSlotType
    {
        AddSelectUIController,      // コントローラーに追加する
        NotAddSelectUIController,   // コントローラーに追加しない
    }

    [Header("UIコントローラー種類")]
    [SerializeField]
    protected CreateSlotType m_createSlotType = CreateSlotType.AddSelectUIController;

    [Header("UI選択コントローラー")]
    [SerializeField]
    protected SelectUIController m_selectUIController = null;

    //==========================
    // 改行値
    private int m_lineBreak = 1;

    //====================================================================================


    [Header("作成するスロットプレハブ")]
    [SerializeField]
    protected GameObject m_slot = null;


    //===================================================================================

    // 作成したスロット
    protected List<ItemSlotData> m_slotList = new();

    public List<ItemSlotData> SlotList { get { return m_slotList; } }


    //===================================================================================
    //===================================================================================
    //===================================================================================

    virtual public void OnInitialize()
    {
        // 改行値をセット
        SetLineBreak();

        // スロットを作成
        CreateSlot();

    }


    virtual public void CreateSlot()
    {

        // ここでスロットを作成
        CreateSlotInstance();

        // Nullのオブジェクトを取り除く
        RemoveNullSlotList();

    }


    virtual public void DestroyItemSlotData(ItemSlotData _data)
    {
        if (_data == null) return;

        // 当てはまるデータを取り除く
        m_slotList.Remove(_data);

        // スロットを削除する
        Destroy(_data.gameObject);

    }


    virtual public void RemoveItemSlotData(ItemSlotData _data)
    {
        if (_data == null) return;

        // 当てはまるデータを取り除く
        m_slotList.Remove(_data);
    }


    // Nullのスロットを削除する
    virtual protected void RemoveNullSlotList()
    {
        // nullのスロットをすべて取り除く
        m_slotList.RemoveAll(slot => slot == null);
    }


    // 改行値をセット
    virtual protected void SetLineBreak()
    {
        if (m_createSlotType != CreateSlotType.AddSelectUIController) return;
        if (m_selectUIController == null)
        {
            Debug.LogError("追加するSelectUIControllerがシリアライズされていません");
            return;
        }

        if (gameObject.TryGetComponent<GridLayoutGroup>(out var grid))
        {
            m_lineBreak = grid.constraintCount;
        }
    }


    // 作成したスロットをコントローラーに追加する
    virtual protected void AddSelectUIControler(GameObject _addObject)
    {
        if (_addObject == null) return;

        if (m_createSlotType != CreateSlotType.AddSelectUIController) return;
        if (m_selectUIController == null)
        {
            Debug.LogError("追加するSelectUIControllerがシリアライズされていません");
            return;
        }

        // スロットコントローラーにセット
        m_selectUIController.AddUI(_addObject, SelectUIType.Press, m_lineBreak);

    }


    protected void DestroySlotList()
    {// スロットが削除されないところで終わってるよ

        foreach (var list in m_slotList)
        {
            if (list == null) continue;

            // 削除する
            Destroy(list.gameObject);
        }

        // 初期化
        m_slotList.Clear();

    }

    // スロットのinstanceを作成する
    // 派生クラスはこのメソッドをoverrideしてください
    virtual protected void CreateSlotInstance()
    {
        Debug.LogError("オーバーライドしてください");
    }

}

