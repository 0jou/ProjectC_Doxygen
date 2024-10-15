using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindAnimator : MonoBehaviour
{
    [ContextMenu("DoAnimation")]
    private UniTask Method()
    {
        return CutIn();
    }
    [ContextMenu("In")]
    private UniTask INN()
    {
        return In();
    }
    [ContextMenu("Out")]
    private UniTask OUTT()
    {
        return Out();
    }

    // カットイン関連のUI
    [SerializeField] CanvasGroup m_cutInCanvas;

    // カットインの位置
    [SerializeField] RectTransform m_rectTrans;

    // シェーダーグラフが適用されたマテリアル
    [SerializeField] Material m_material;

    // 操作するプロパティのID
    private int m_blindID;

    // 定数の定義
    [SerializeField] float m_cutInFadeDuration = 0.4f;
    [SerializeField] float m_cutInDisplayDuration = 1.5f;
    [SerializeField] float m_blindFadeDuration = 0.5f;
    [SerializeField] float m_cutInFadeOutDelay = 0.3f;
    [SerializeField] float m_cutInFadeOutDuration = 0.3f;
    [SerializeField] float m_initialPositionX = 500f;
    [SerializeField] float m_finalPositionX = 0.0f;

    [SerializeField] string m_propertyName = "_BlindValue";

    private float m_startUpValue = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        float outlineLength = m_material.GetFloat(Shader.PropertyToID("_OutLineLength"));

        m_startUpValue = 0.0f - outlineLength;

        m_blindID = Shader.PropertyToID(m_propertyName);
        m_material.SetFloat(m_blindID, m_startUpValue);

    }

    private UniTask InSequence(Sequence seq)
    {
        // カットインを表示(画面中央まで移動)
        seq.Append(m_cutInCanvas.DOFade(1.0f, m_cutInFadeDuration))
            .Join(m_rectTrans.DOAnchorPosX(m_finalPositionX, m_cutInFadeDuration))
            .AppendInterval(m_cutInDisplayDuration);
        return seq.Play().ToUniTask();
    }
    private UniTask OutSequence(Sequence seq)
    {
        // カットインを表示(画面中央まで移動)
        seq.Append(m_material.DOFloat(1.0f, m_blindID, m_blindFadeDuration))
            .Join(m_cutInCanvas.DOFade(0.0f, m_cutInFadeOutDuration)).SetDelay(m_cutInFadeOutDelay)
            .Append(m_rectTrans.DOAnchorPosX(m_initialPositionX, m_cutInFadeOutDuration));
        return seq.Play().ToUniTask();
    }

    public UniTask In()
    {
        Debug.Log("In");
        // シェーダーグラフのパラメーターを初期化
        m_material.SetFloat(m_blindID, m_startUpValue);

        var sequence = DOTween.Sequence();

        return InSequence(sequence);
    }


    public UniTask Out()
    {
        Debug.Log("Out");
        var sequence = DOTween.Sequence();
        return OutSequence(sequence);
    }

    public UniTask CutIn()
    {
        Debug.Log("CutIn");
        // シェーダーグラフのパラメーターを初期化
        m_material.SetFloat(m_blindID, m_startUpValue);
        var sequence = DOTween.Sequence();
        InSequence(sequence);
        OutSequence(sequence);

        return sequence.ToUniTask();
    }
}
