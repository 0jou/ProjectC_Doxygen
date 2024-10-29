using Arbor;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// 進捗率に応じて
/// imageのfillAmountを変更することでimageの長さを変更する　コンポーネント
/// 
/// </summary>
public class HPBarController : MonoBehaviour, ISerializationCallbackReceiver
{
    [Tooltip("値変えると長さ変わる")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float m_value = new();

    [SerializeField] private bool m_isCalc = true;
    public bool IsCalc
    {
        get { return m_isCalc; }
        set { m_isCalc = value; }
    }
    [ShowIf(nameof(m_isCalc))]
    [SerializeField] private float m_maxHealth = 50.0f;

    [Tooltip("色や長さ変更するImageを設定")]
    [SerializeField] private Image m_image;

    [Tooltip("HPの値を表示するテキスト(TMP)を設定する　表示しない場合はNoneで可")]
    [SerializeField] private TextMeshProUGUI m_text;

    // TODO:色の変化などは別のクラスに移動
    [Tooltip("色の変化の設定")]
    [SerializeField] private List<HPBarColor> m_barColorList = new List<HPBarColor>();


    // 接敵時の表示切替用　伊波
    private ArborFSM m_arborFSM;
    private bool m_isShow = false;

    public bool IsSetHealthValue
    {
        get { return (m_text != null); }
        private set { }
    }


    // 現在のHP、最大値、最小値もセット
    public void SetHealth(float _current, float _max)
    {
        m_maxHealth = _max;
        SetHealthValue(_current);
    }

    // 現在のHPをセット
    public void SetHealthValue(float _currentHealth)
    {
        if (m_isCalc)
        {
            m_value = _currentHealth / m_maxHealth;
        }
        else
        {
            m_value = _currentHealth;
        }

        // テキストへ反映
        SetHealthText();

        // HPバーの更新
        UpdateFillAmount();

    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_image == null)
        {
            Debug.LogError("HPBarController Imageが設定されていません");
        }

        if (!transform.root.TryGetComponent(out m_arborFSM))
        {
            Debug.Log("HPBarControllerがArborを見つけられませんでした", gameObject);
            return;
        }
        for (int child = 0; child < transform.childCount; ++child)
        {
            transform.GetChild(child).gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// インスペクターで値を変更した時に呼ばれる（保存前）
    /// </summary>
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        UpdateFillAmount();
        SetHealthText();
    }

    /// <summary>
    /// インスペクターでの値の変更後、保存された値を読み込む際に呼ばれる
    /// </summary>
    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
    }

    private void UpdateFillAmount()
    {
        if (m_image == null) return;

        // 値→長さ
        m_image.fillAmount = m_value;

        // 値→色
        for (int i = 0; i < m_barColorList.Count; i++)
        {
            if (m_value > m_barColorList[i].m_value) continue;

            m_image.color = m_barColorList[i].m_color;
            break;
        }
    }

    private void SetHealthText()
    {
        if (m_text == null) return;

        float currentHealth = m_maxHealth * m_value;
        int intCurrent = (int)currentHealth;
        int intMax = (int)m_maxHealth;

        // 切り捨てで0になってしまった時
        if (intCurrent == 0 && m_value > 0)
        {
            intCurrent = 1;
        }

        m_text.text = intCurrent + " / " + intMax;
    }

    public void Update()
    {
        // 敵のHPバー表示非表示切り替え
        if (m_arborFSM == null) return;
        if (m_isShow)
        {
            if (m_value < 1.0f) return;

            Transform target = m_arborFSM.parameterContainer.GetTransform("Target");
            if (target == null) ShowHP(false);
        }
        else
        {
            if (m_value < 1.0f)
            {
                ShowHP(true);
                return;
            }
            Transform target = m_arborFSM.parameterContainer.GetTransform("Target");
            if (target != null)
            {
                foreach (CharacterCore chara in IMetaAI<CharacterCore>.Instance.ObjectList)
                {
                    if (chara.InputType == CharacterCore.InputTypes.Player)
                    {
                        ShowHP(true);
                        return;
                    }
                }
            }
        }
    }

    private void ShowHP(bool _isShow)
    {
        for (int child = 0; child < transform.childCount; ++child)
        {
            transform.GetChild(child).gameObject.SetActive(_isShow);
        }
        m_isShow = _isShow;
    }
}

[System.Serializable]
public class HPBarColor
{
    public float m_value;
    public Color m_color;
}