/**
* @file ThrowRock.cs
* @brief 狼の岩投げ処理
*/
using Arbor;
using UnityEngine;
using System;
using UniRx;
using ItemInfo;

/**
* @brief 狼の岩投げ処理　伊波
* @details ArborのTargetを参照して投げる
*/
public class ThrowRock : MonoBehaviour
{
    [Tooltip("岩の投げるまでの時間　秒")]
    [SerializeField, Range(0.0f, 10.0f)] private float m_throwSeconds;

    [Tooltip("着弾までの時間　秒")]
    [SerializeField, Range(0.0f, 5.0f)] private float m_arrivalTime;

    [Tooltip("投げられる最大角度")]
    [SerializeField, Range(0.0f, 180.0f)] private float m_throwAngle;

    [Tooltip("削除判定用のオブジェクト")]
    [SerializeField] private GameObject m_checkHitPrefab;

    private Transform m_target;
    private Transform m_lunchObj;

    void Start()
    {
        if (transform.root.TryGetComponent(out EnemyParameters parameters))
        {
            m_target = parameters.Target;
        }
        m_lunchObj = transform.parent;

        Observable.Timer(TimeSpan.FromSeconds(m_throwSeconds)).Subscribe(_ =>
        {
            transform.parent = null;

            // プレイヤーのアイテム投げ処理を参考に作成
            var collider = transform.GetComponentInChildren<Collider>();
            if (collider == null)
            {
                Debug.LogError("m_colliderが存在しません");
                return;
            }
            // Rigidbodyの設定を変更 投げる
            if (gameObject.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.constraints = RigidbodyConstraints.None;
                rigidbody.useGravity = true;
            }
            else
            {
                rigidbody = gameObject.AddComponent<Rigidbody>();
            }

            if (m_target) rigidbody.AddForce(CalcThrowPower(m_lunchObj.position, m_target.position) * rigidbody.mass, ForceMode.Impulse);
            else rigidbody.AddForce(CalcThrowPower(m_lunchObj.position, m_lunchObj.position + m_lunchObj.forward * 5f) * rigidbody.mass, ForceMode.Impulse);


            Instantiate(m_checkHitPrefab, transform);

            // 効果音
            SoundManager.Instance.Start3DPlayback("WolfThrowRock", rigidbody.gameObject);

        }).AddTo(this);


        Vector3 CalcThrowPower(Vector3 _startPos, Vector3 _endPos)
        {
            //　開始位置と到着位置の長さを計算
            var adjacent = Vector3.Distance(
                    _startPos,
                    _endPos);

            float angle = Vector3.SignedAngle(m_lunchObj.root.forward, _endPos - _startPos, Vector3.up);
            if (Mathf.Abs(angle) > m_throwAngle)
            {
                Vector3 toEndVec = _endPos - _startPos;
                toEndVec.Normalize();
                if (angle > 0)
                    _endPos = Quaternion.Euler(0, 0, m_throwAngle * Mathf.Rad2Deg) * toEndVec + _startPos;
                else
                    _endPos = Quaternion.Euler(0, 0, -m_throwAngle * Mathf.Rad2Deg) * toEndVec + _startPos;
            }


            //　落下距離を計算
            //　0.5 * 重力 * 到達時間の2乗
            var opposite = Mathf.Abs(
                0.5f * Physics.gravity.y * m_arrivalTime * m_arrivalTime);

            //　到達点＋上向きに落下距離 放物線の頂点
            var upPos = _endPos + Vector3.up * opposite;

            //　斜辺の長さを計算
            var hypotenuse = Vector3.Distance(
                _startPos, upPos);

            //　角度を計算
            //　余弦定理
            float theta = -Mathf.Acos(
                (Mathf.Pow(hypotenuse, 2) + Mathf.Pow(adjacent, 2) - Mathf.Pow(opposite, 2))
                / (2 * hypotenuse * adjacent));

            //　横軸のspeedから斜め方向の速さを計算
            float hypotenuseSpeed = hypotenuse / m_arrivalTime;

            // 一旦攻撃対象の方を見る
            m_lunchObj.LookAt(_endPos);
            //　砲台の高さ向きを変える
            m_lunchObj.Rotate(Vector3.right, theta * Mathf.Rad2Deg, Space.Self);

            //Vector3 result = Quaternion.Euler(0, 0, -90) * toEndVec;
            //result = Quaternion.AngleAxis(theta * Mathf.Rad2Deg, Vector3.right) * result;

            ////　砲台の高さ向きを変える
            //transform.Rotate(Vector3.right, theta * Mathf.Rad2Deg, Space.Self);

            return m_lunchObj.forward * hypotenuseSpeed;
        }
    }
}
