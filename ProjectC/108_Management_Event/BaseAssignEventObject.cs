/*
 * @file BaseAssignEventObject
 * @brief ImetaAIを用いてイベントオブジェクト群として管理するための基底クラス
 * Colliderに接触、接触終了した際のイベントと外部アクセス時のイベントを定義
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @brief ImetaAIを用いてイベントオブジェクト群として管理するための外部アクセス前提イベントオブジェクトの基底クラス
/// 接触時イベント(自動),接触終了時イベント(自動)、アクセス時イベント(外部呼び出し)
/// </summary>
public class BaseAssignEventObject : MonoBehaviour
{
    //! アクセス時に実行させたいアニメーショントリガー名
    [Header("アニメーショントリガー名")]
    [SerializeField] string m_animatorTriggerName;

    //! 接触イベントを発生させるためのタグリスト
    [Header("接触イベントを発生させるためのタグリスト")]
    [Tag]
    [SerializeField] List<string> m_tagList = new();

    //! アニメーショントリガー名のプロパティ
    public string AnimatorTriggerName { get { return m_animatorTriggerName; } }

    //! 接触中かを管理するプロパティ
    private bool m_isCollided = false;

    //! 接触中かを返すプロパティ
    public bool Collided { get { return m_isCollided; } }

    //====================================================================================================
    // virtual関数 要定義
    //====================================================================================================

    /// <summary>
    /// @brief 接触時のイベント
    /// m_tagListに登録されたタグオブジェクトとの接触時に自動で呼ばれる
    /// </summary>
    protected virtual void OnCollisionTriggerEvent()
    {
        Debug.LogError("BaseAssignEventObjectのOnCollisionTriggerEventが呼ばれました");
    }

    /// <summary>
    /// @brief 接触終了時のイベント
    /// </summary>
    protected virtual void OnCollisionTriggerExitEvent()
    {
        Debug.LogError("BaseAssignEventObjectのOnCollisionExitEventが呼ばれました");
    }

    /// <summary>
    /// @brief 何らかのアクセスがあった際のイベント
    /// アクセスの定義は継承先で行う
    /// </summary>
    public virtual void OnCollisionAccesesEvent()
    {
        Debug.LogError("BaseAssignEventObjectのOnCollisionAccesesEventが呼ばれました");
    }

    /// <summary>
    /// @brief アクセスされたかどうかを返す
    /// 主にキーアクセスを想定
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public virtual bool IsAccessed(ref IInputProvider input)
    {
        Debug.LogError("BaseAssignEventObjectのIsAccessedが呼ばれました");
        return false;
    }


    //====================================================================================================

    private void OnTriggerEnter(Collider other)
    {
        bool tagCheck = false;
        foreach (var tag in m_tagList)
        {
            if (other.CompareTag(tag))
            {
                tagCheck = true;
            }
        }

        if (tagCheck)
        {
            m_isCollided = true;
            OnCollisionTriggerEvent();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        bool tagCheck = false;
        foreach (var tag in m_tagList)
        {
            if (other.CompareTag(tag))
            {
                tagCheck = true;
            }
        }

        if (tagCheck)
        {
            m_isCollided = false;
            OnCollisionTriggerExitEvent();
        }
    }

    protected void Start()
    {
        if(IMetaAI<BaseAssignEventObject>.Instance==null)
        {
            Debug.LogError("BaseAssignEventObjectのMetaAIが存在しません");
            return;
        }


        // MetaAIに登録
        IMetaAI<BaseAssignEventObject>.Instance.RegisterObject(this);
    }
}
