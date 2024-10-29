using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEmptyDishEvent : BaseManagementEvent
{

    [Header("空皿のプレハブ")]
    [SerializeField] private GameObject m_emptyDishPrefab;

    [Header("自動終了時間")]
    [SerializeField]
    private float m_endTime = 20.0f;
    private float m_currentEndCount = 0.0f;


    private GameObject m_emptyDishInstance = null;

    public override void OnStart()
    {
        if (m_emptyDishPrefab == null)
        {
            return;
        }
        m_emptyDishInstance = Instantiate(m_emptyDishPrefab);
        m_emptyDishInstance.transform.position= transform.position;
    }

    public override void OnUpdate()
    {
        m_currentEndCount += Time.deltaTime;

        if (m_currentEndCount >= m_endTime)
        {
            SetEventEnd(ManagementGameInfo.EventSolutionType.UnSolution);
        }
        if (!m_emptyDishInstance
            )
        {
            SetEventEnd(ManagementGameInfo.EventSolutionType.Solution);
        }
    }

    public override void OnExit()
    {
    }

}
