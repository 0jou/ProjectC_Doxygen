using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeItemObjectNameText : MonoBehaviour
{

    // 制作者 (田内)

    [Header("アイテムID")]
    [SerializeField]
    private AssignItemID m_itemID = null;


    [Header("変更するTextMeshPro")]
    [SerializeField]
    private TextMeshProUGUI m_textMeshPro = null;


    // テキストを変更する
    private void Start()
    {

        if (m_textMeshPro == null)
        {
            Debug.LogError("名前を表示するテキストが登録されていません");
            return;
        }

        if (m_itemID == null)
        {
            Debug.LogError(transform.parent.name + "のAssignItemIDコンポーネントがアタッチされていません");
            return;
        }


        var data = ItemDataBaseManager.instance.GetItemData(m_itemID.ItemTypeID, m_itemID.ItemID);
        if (data == null)
        {
            Debug.LogError("このアイテムは登録されていません");
            return;
        }


        // IDを基にテキストを変更
        m_textMeshPro.text = data.ItemName;

    }



}
