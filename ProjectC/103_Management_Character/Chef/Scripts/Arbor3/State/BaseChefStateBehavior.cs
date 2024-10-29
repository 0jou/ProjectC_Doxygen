using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class BaseChefStateBehavior : StateBehaviour 
{
    // 制作者　田内

    [Header("シェフ情報")]
    [SerializeField]
    protected FlexibleChefDataVariable m_flexibleChefDataVariable = null;


    #region メソッド説明
    /// <summary>
    /// シェフ情報が存在するかのチェック
    /// </summary>
    #endregion
    protected ChefData GetChefData()
    {

        // シェフ情報が存在するか確認

        if (m_flexibleChefDataVariable == null)
        {
            Debug.LogError(this.name + "シェフ情報がシリアライズされていません");
            return null;
        }

        if (m_flexibleChefDataVariable.value.ChefData == null)
        {
            Debug.LogError(this.name + "シェフ情報が存在しません");
            return null;
        }

        // 存在する
        return m_flexibleChefDataVariable.value.ChefData;

    }


    protected void SetTransition(StateLink _link)
    {
        if (_link == null)
        {
            Debug.LogError("次のシーンがセットされていません");
            return;
        }

        Transition(_link);

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
