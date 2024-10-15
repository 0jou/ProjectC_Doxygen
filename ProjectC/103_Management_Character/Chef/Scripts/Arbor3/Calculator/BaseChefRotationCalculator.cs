using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class BaseChefRotationCalculator :BaseChefCalclator
{
    // 特定の方向を見る基底計算クラス
    // これを使う場合、AgentMoveToPositionのStoppingDistanceを100にすること
    // 制作者　田内


    [Header("移動させる距離")]
    [SerializeField]
    private float m_moveDistance = 0.1f;


    protected virtual void GetTargetPosition(ref Vector3 _targetPos)
    {
        Debug.LogError("オーバーライドして書き換えてください");
    }


    public override void OnCalculate()
    {
        var obj = GetRootGameObject();
        if (obj == null) return;


        var pos = obj.transform.position;
        var targetPos = obj.transform.position;

        GetTargetPosition(ref targetPos);

        // 角度
        Vector3 dir = targetPos - pos;
        dir.y = 0.0f;
        dir.Normalize();

        pos = pos + (dir * m_moveDistance);

        m_outputPos.SetValue(pos);

    }
}
