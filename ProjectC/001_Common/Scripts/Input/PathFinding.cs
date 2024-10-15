using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine.SocialPlatforms.Impl;
using System;

// NavMeshAgentから移動関係の情報をやり取りする（伊波）

public class PathFinding : MonoBehaviour
{
    [SerializeField] private NavMeshAgent m_agent;
    private Transform m_myTransform;

    // 移動したい方向
    public Vector3 DesiredVelocity => m_agent.desiredVelocity;

    // 残り距離
    public float RemainingDistance => m_agent.remainingDistance;

    // 最終目的地
    public Vector3 Destination => m_agent.destination;


    // 到着した？
    public bool IsArrived
    {
        get
        {
            if (Vector3.Distance(m_agent.destination, m_myTransform.position) <= m_agent.stoppingDistance)
            {
                Stop();
                return true;
            }

            // 考え中は到達してない
            if (m_agent.pathPending) return false;
            //　ストップしてないときは到達してない
            if (m_agent.isStopped == false) return false;

            return true;
        }
    }

    // 停止
    public void Stop()
    {
        m_agent.isStopped = true;
    }


    // 目的地を設定
    public void SetDestination(Vector3 pos)
    {
        m_agent.SetDestination(pos);
        m_agent.isStopped = false;
    }

    private void Awake()
    {
        if (!m_agent)
        {
            Debug.LogError("NavMeshAgentがセットされていません。");
            return;
        }

        TryGetComponent(out m_myTransform);

        m_agent.updatePosition = false;
        m_agent.updateRotation = false;

        m_agent.isStopped = true;
    }

    private void Start()
    {
        UpdateAsync();
    }

    private void Update()
    {
        //if (isActiveAndEnabled == false)
        //{
        //    return;
        //}


        //// 経路探索がまだ終わっていないときは、ナニモしない
        //if (m_agent.pathPending)
        //{
        //    return;
        //}

        //if (m_agent.remainingDistance <= m_agent.stoppingDistance)
        //{
        //    m_agent.isStopped = true;
        //}


        //// Agent座標を、自分の座標まで移動
        //if ((m_agent.nextPosition - m_myTransform.position).magnitude > 1.0f)
        //{
        //    Vector3 savePos = m_agent.destination;

        //    m_agent.Warp(m_myTransform.position);

        //    SetDestination(savePos);
        //}
        //else
        //{
        //    m_agent.nextPosition = m_myTransform.position;
        //}
    }

    async private void UpdateAsync()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();
        while (cancelToken.IsCancellationRequested == false)
        {
            // GameObjectや自分が無効状態になっているときは、ナニモしない
            if (isActiveAndEnabled == false)
            {
                await UniTask.DelayFrame(1);
                continue;
            }


            // 経路探索がまだ終わっていないときは、ナニモしない
            if (m_agent.pathPending)
            {
                await UniTask.DelayFrame(1);
                continue;
            }

            if (m_agent.remainingDistance <= m_agent.stoppingDistance)
            {
                m_agent.isStopped = true;
            }

            
            // Agent座標を、自分の座標まで移動
            if ((m_agent.nextPosition - m_myTransform.position).magnitude > 1.0f)
            {
                Vector3 savePos = m_agent.destination;

                m_agent.Warp(m_myTransform.position);

                SetDestination(savePos);

                await UniTask.DelayFrame(3);
            }
            else
            {
                m_agent.nextPosition = m_myTransform.position;
            }

            await UniTask.DelayFrame(1);
        }
    }
}