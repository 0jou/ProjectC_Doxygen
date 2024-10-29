/*!
 * @file GenerateCleaningEvent.cs
 * @brief 汚れイベントオブジェクト出現させるイベント
 * @date 24/09/18 10:00 汚れオブジェクトを出現させる処理を追加
 * @date 24/10/2? 追加(田内) 汚れが無くなれば終了する処理を追加
 */

using Arbor.Calculators;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @brief 汚れイベント
/// </summary>
public class GenerateCleaningEvent : BaseManagementEvent
{

    //! @brief 汚れのプレハブ
    //! @todo 上甲 汚れの種類追加? 必要かは不明
    [Header("汚れのプレハブ")]
    [SerializeField] private GameObject m_dirtPrefab;

    public Vector3 m_position { get; set; } = new Vector3(0, 0, 0);

    //! @brief 座標からのずれる範囲
    [SerializeField]
    private float ｍ_randomRange = 0.3f;

    // 追加(田内)
    [Header("自動終了時間")]
    [SerializeField]
    private float m_endTime = 20.0f;
    private float m_currentEndCount = 0.0f;


    private GameObject m_createDirt = null;


    public void SetRandomRange(float range)
    {
        ｍ_randomRange = range;
    }

    private bool m_isPositionSet = false;



    public override void OnStart()
    {
        Debug.Log("CleaningEvent");
        CreateDirt();
    }

    public override void OnUpdate()
    {
        CheckDirt();
        CountEndTime();
    }

    public override void OnExit()
    {
        if (m_createDirt != null) Destroy(m_createDirt);
    }

    public void SetPosition(Vector3 position)
    {
        m_position = position;
        m_isPositionSet = true;
    }

    /// <summary>
    /// @brief 汚れオブジェクトを生成する
    /// @todo 汚れの生成位置調整機能追加
    /// @todo 汚れの位置を地形にある程度沿わす処理追加
    /// </summary>
    private void CreateDirt()
    {
        if (m_dirtPrefab == null)
        {
            Debug.LogError("汚れプレハブがシリアライズされていません");
            return;
        }


        m_createDirt = Instantiate(m_dirtPrefab);

        //位置を設定
        if (m_isPositionSet)
        {
            m_createDirt.transform.position = m_position;
        }

        //ランダムでずらす
        var randomRange = new Vector3(Random.Range(-ｍ_randomRange, ｍ_randomRange), 0, Random.Range(-ｍ_randomRange, ｍ_randomRange));
        m_createDirt.transform.position += randomRange;
    }


    // 追加 田内
    // 時間経過で自動的に終了
    private void CountEndTime()
    {
        m_currentEndCount += Time.deltaTime;
        if (m_endTime < m_currentEndCount)
        {
            SetEventEnd(ManagementGameInfo.EventSolutionType.UnSolution);
        }
    }

    // 追加 田内
    // 汚れが無くなれば終了
    private void CheckDirt()
    {
        if (m_createDirt == null)
        {
            SetEventEnd(ManagementGameInfo.EventSolutionType.Solution);
        }
    }

}