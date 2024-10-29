using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;
using KinematicCharacterController;
using NaughtyAttributes;

// キャラクター同士の押し出し処理コンポーネント（伊波）

public class PushOutCollider : MonoBehaviour
{
    [Header("滑らせる速さ")]
    [SerializeField, Range(0f, 1000f)] private float m_angleSpeed = 180f;
    private CapsuleCollider myCapsuleCollider;

    void Start()
    {
        if (!transform.root.TryGetComponent(out myCapsuleCollider)) return;

        this.OnTriggerEnterAsObservable()
         .Where(_ => enabled)
         .Subscribe(collider =>
         {
             PushOut(collider);
         }
         ).AddTo(this);

        this.OnTriggerStayAsObservable()
         .Where(_ => enabled)
         .Subscribe(collider =>
         {
             PushOut(collider);
         }
         ).AddTo(this);
    }

    void PushOut(Collider collider)
    {
        // 自分自身なら処理しない
        if (collider.transform.root.name == transform.root.name) return;

        KinematicCharacterMotor hitMotor;
        if (!collider.TryGetComponent(out hitMotor)) return;

        // 相手との角度計算
        Vector3 targetVec = collider.transform.root.position - transform.root.position;
        targetVec.y = 0;
        float angle = Vector3.SignedAngle(transform.root.forward, targetVec.normalized, Vector3.up);
        if (Mathf.Abs(angle) >= 90) return;
        if (angle > 0) angle += m_angleSpeed * Time.deltaTime; else angle -= m_angleSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        Vector3 vec = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad));

        // 相手の位置移動
        float dist = myCapsuleCollider.radius + hitMotor.Capsule.radius;
        Vector3 pos = transform.position;
        pos.y = hitMotor.transform.position.y;
        Vector3 nextPos = pos + (Quaternion.Euler(0, angle, 0) * transform.root.forward) * dist;
        hitMotor.MoveCharacter(nextPos);
    }
}
