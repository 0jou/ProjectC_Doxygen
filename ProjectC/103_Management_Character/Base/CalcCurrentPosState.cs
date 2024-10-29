using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;


[AddBehaviourMenu("Common/CalcCurrentPosState")]
[AddComponentMenu("")]
public class CalcCurrentPosState : Calculator
{
    // 制作者 田内
    // 親オブジェクトの座標(現在の位置)を計算する

    //=========
    // 出力
    [Header("出力:座標")]
    [SerializeField]
    protected OutputSlotVector3 m_outputPos = new();

    //=========================================
    //				実行処理
    //=========================================

    public override void OnCalculate()
    {
        Transform currentTransform = transform;

        // ルートオブジェクトに到達するまで親を辿る
        while (currentTransform.parent != null)
        {
            currentTransform = currentTransform.parent;
        }

        m_outputPos.SetValue(currentTransform.position);

    }
}
