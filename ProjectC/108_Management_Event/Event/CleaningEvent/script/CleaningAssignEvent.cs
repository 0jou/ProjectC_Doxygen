/* @file CleaningAssignEvent.cs
 * @brief 任意のTagのコライダーが接触した際に行われる汚れ自体の挙動を制御するスクリプト
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// @brief 汚れ自体の接触時の挙動を制御するスクリプト
/// </summary>
public class CleaningAssignEvent : BaseAssignEventObject
{
    private DecalProjector m_projector = null;

    [SerializeField] private Shader m_copyShader;


    //! @brief 接触時のイベント 汚れを強調表示する
    protected override void OnCollisionTriggerEvent()
    {
        m_projector.material.SetFloat("_EmphasisValue", 1.0f);
    }

    //! @brief 接触終了時のイベント 汚れの強調表示を解除する
    protected override void OnCollisionTriggerExitEvent()
    {
        m_projector.material.SetFloat("_EmphasisValue", 0.0f);
    }

    //! @brief アクセスされたかどうかの定義 Gathering(Eキー)を押すとアクセスされる
    public override bool IsAccessed(ref IInputProvider input)
    {
        return input.Gathering;
    }

    //! @brief アクセス時のイベント 汚れを消す
    public override void OnCollisionAccesesEvent()
    {
        Destroy(gameObject);
    }



    private void Start()
    {
        base.Start();

        m_projector = GetComponent<DecalProjector>();

        if (m_projector != null)
        {
            // マテリアルをコピーしておかないと末尾の情報で固定されてしまうためコピー
            m_projector.material = new(m_copyShader);
        }
    }

    private void OnDestroy()
    {
        // マテリアルを新しく作成したので明示的に削除しておく
        // https://www.create-forever.games/unity-material-memory-leak/
        Destroy(m_projector.material);
    }
}
