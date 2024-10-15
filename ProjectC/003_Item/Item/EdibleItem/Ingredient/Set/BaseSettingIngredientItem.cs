using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IngredientInfo;

public class BaseSettingIngredientItem : MonoBehaviour
{


    [Header("作成する材料ID")]
    [SerializeField]
    protected IngredientID m_ingredientID = IngredientID.BigMushroom;


    [Header("レイの長さ")]
    [SerializeField]
    protected float m_rayDistance = 10.0f;


    //==============================================================


    // 設置する座標
    protected bool RayHitPosition(Vector3 _startPos, out RaycastHit _hit)
    {
        // 方向
        Vector3 dir = Vector3.down;

        // レイ作成
        Ray ray = new Ray(_startPos, dir);
        if (Physics.Raycast(ray, out _hit, m_rayDistance))
        {
            //Debug.Log("当たり判定しました");
            return true;
        }
        else
        {
            //Debug.Log("当たり判定できませんでした");
            return false;
        }

    }
}
