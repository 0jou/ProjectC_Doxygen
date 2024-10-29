using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfChangeCurrentSelectUICursorPosition : MonoBehaviour
{
    //ある程度自分自身で管理できるように改造したカーソル処理（山本）
    //=================================================================
    // UIコントローラー

    [Header("UIを管理するコントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

    [Header("カーソルポジション")]
    [SerializeField]
    private GameObject m_cursorPosition = null;

    //[Header("スロット登録クラス")]
    //[SerializeField]
    //private CreateActionItemSlotList m_itemSlotList = null;

    //=================================================================
    //=================================================================
    //=================================================================

    private void Update()
    {
        ChangePosition();
    }


    // カーソルを変更
    private void ChangePosition()
    {

        if (m_selectUIController == null)
        {
            Debug.LogError("controllerが登録されていません");
            return;
        }


        var currentSlot = m_selectUIController.CurrentSelectUI;

        // スロットが無ければ
        if (currentSlot == null)
        {
            // 非表示
            m_cursorPosition.SetActive(false);

        }
        // 選択しているスロットがあれば
        else
        {
            // 表示
            m_cursorPosition.SetActive(true);
            // 現在選択されているスロットの座標に移動する
            m_cursorPosition.transform.position = currentSlot.transform.position;

        }


    }


}
