using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 制作者 吉田
/// 
/// シリアライズしたTMPテキストをコピーして、
/// このスクリプトをアタッチしたオブジェクトのTMPに貼り付ける
/// [RequireComponent(typeof(TMPro.TextMeshProUGUI))]あります
/// 
/// 更新順位は10に設定しています（後に更新されてほしいため）
/// コピー元がない場合は、
/// コピー先のTMPは空白になる
/// 
/// 変更・修正などご自由にどうぞ
/// 
/// </summary>

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
[DefaultExecutionOrder(10)]
public class CopyText : MonoBehaviour
{
    [Header("コピー元のTMP")]
    [SerializeField]
    private TMPro.TextMeshProUGUI m_originText = null;
    public TMPro.TextMeshProUGUI OriginText { get { return m_originText; } set { m_originText = value; } }

    [Header("コピー先のTMP")]
    [SerializeField]
    [ReadOnly]// 同じオブジェクト内のTMPを使う
    private TMPro.TextMeshProUGUI m_copyToText = null;

    [Header("毎フレーム更新するかどうか")]
    [SerializeField]
    bool m_isUpdate = false;
    public bool IsUpdate { get { return m_isUpdate; } set { m_isUpdate = value; } }




    // Start is called before the first frame update
    void Start()
    {
        if (m_copyToText == null)
        {
            m_copyToText = GetComponent<TMPro.TextMeshProUGUI>();
        }

        Copy();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isUpdate) return;

        //途中でコピー先がなくなってしまった場合
        if (m_copyToText == null)
        {
            Destroy(this);
            return;
        }

        // ------------------------
        // 処理
        // ------------------------
        Copy();
    }

    public void Copy()
    {
        if (m_copyToText == null) return;

        if(m_originText == null)
        {
            m_copyToText.text = "";
            return;
        }

        m_copyToText.text = m_originText.text;

    }

    [Button("コピー先のTMP 自身から検索")]
    private void FindCopyToText()
    {
        m_copyToText = GetComponent<TMPro.TextMeshProUGUI>();
    }

}
