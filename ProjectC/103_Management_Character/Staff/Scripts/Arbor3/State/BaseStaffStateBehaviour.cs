using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class BaseStaffStateBehaviour : StateBehaviour
{
    // スタッフのステート処理
    // Nullチェックなどをの関数をまとめておく
    // 制作者　田内


    //===========================

    [Header("スタッフ情報")]
    [SerializeField]
    protected FlexibleStaffDataVariable m_flexibleStaffDataVariable = null;


    //==============================================================

    #region メソッド説明
    /// <summary>
    /// スタッフ情報が存在するかのチェック
    /// </summary>
    #endregion
    protected StaffData GetStaffData()
    {

        // スタッフ情報が存在するか確認

        if (m_flexibleStaffDataVariable == null)
        {
            Debug.LogError("スタッフ情報がシリアライズされていません");
            return null;
        }

        if (m_flexibleStaffDataVariable.value.StaffData == null)
        {
            Debug.LogError("スタッフ情報が存在しません");
            return null;
        }

        // 存在する
        return m_flexibleStaffDataVariable.value.StaffData;

    }

    protected bool SetTransition(StateLink _link)
    {
        if (_link == null)
        {
            Debug.LogError("次のシーンがセットされていません");
            return false;
        }

        Transition(_link);
        return true;

    }


    // 一番親の情報を取得する
    protected GameObject GetRootGameObject()
    {
        Transform currentTransform = transform;

        // ルートオブジェクトに到達するまで親を辿る
        while (currentTransform.parent != null)
        {
            currentTransform = currentTransform.parent;
        }

        return currentTransform.gameObject;
    }


}
