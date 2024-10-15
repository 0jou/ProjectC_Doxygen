using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using DG.Tweening;

public class ChangeActionItemListPosition : MonoBehaviour
{

    //================================================


    [Header("スロットコントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;


    //=================================================

    [Header("DOスピード")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float m_doSpead = 0.1f;


    //================================================


    [Header("DOEase")]
    [SerializeField]
    private Ease m_ease = Ease.OutCirc;


    //=================================================
    // 初期座標


    float m_initializePosX = 0.0f;


    //=================================================



    public void OnInitialize()
    {

        m_initializePosX = transform.position.x;
    
    }


    public void OnUpdate()
    {

        CheckChangePosition();

    }


    private void CheckChangePosition()
    {

        if (m_selectUIController == null)
        {
            Debug.LogError("スロットコントローラーが登録されていません");
            return;
        }

        if (!m_selectUIController.IsSelectChangeFlg)
        {
            return;
        }

        ChangePosition(true);

    }



    private void ChangePosition(bool _isDOMove)
    {

        if (m_selectUIController == null)
        {
            Debug.LogError("スロットコントローラーが登録されていません");
            return;
        }


        var currentSelectSlot = m_selectUIController.CurrentSelectUI;
        if (currentSelectSlot == null) return;


        float targetPosX = m_selectUIController.CurrentSelectUI.transform.position.x;
        float listPosX = transform.position.x;

        float goalPosX = listPosX - (targetPosX - m_initializePosX);


        if (_isDOMove)
        {
            // 座標を更新
            DOMoveXPosition(goalPosX);
        }
        else
        {
            Vector3 pos = transform.position;
            pos.x = goalPosX;
            gameObject.transform.position = pos;
        }

    }

    private void DOMoveXPosition(float _pos)
    {

        transform.DOMoveX(endValue: _pos, duration: m_doSpead).SetEase(m_ease).SetLink(gameObject);

    }

}
