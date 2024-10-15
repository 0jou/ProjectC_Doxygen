using Cysharp.Threading.Tasks;
using DG.Tweening;
using MackySoft.Navigathena.SceneManagement;
using MackySoft.Navigathena.SceneManagement.Utilities;
using MackySoft.Navigathena.Transitions;
using NaughtyAttributes;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class NextPageTransitionBehaviour : MonoBehaviour
{
    [Foldout("ページ関係オブジェクト登録"), Header("スクショの描画先画像オブジェクト")]
    [SerializeField] private Image m_image;
    [Foldout("ページ関係オブジェクト登録"), Header("スクショ画像に対してのサイズ調整用")]
    [SerializeField] private RectTransform m_rect;
    [Foldout("ページ関係オブジェクト登録"), Header("ページめくり演出マテリアル")]
    [SerializeField] private Material m_material;

    // [SerializeField] private Camera m_camera { get; }

    [Foldout("ページ関係オブジェクト登録"), Header("ロード画面 遷移アニメーション時隠れるオブジェクト")]
    public GameObject m_endHideObject;  // ロード画面 遷移アニメーション時隠れる

    [Header("ページをめくるのにかかる時間")]
    public float m_slideMoveTime = 3.0f;


    private string m_propertyName = "_Flip";

    private CaptureScreenToImage m_capture;

    private readonly float m_startUpValue = 1.0f;
    private readonly float m_finalValue = -1.0f;

    [ContextMenu("NextPage")]
    private async void OnEvent()
    {
        await ScreenShot();
        await NextPage(m_startUpValue);
    }

    // ページめくり演出開始
    public async UniTask NextPage()
    {
        await NextPage(m_slideMoveTime);
    }
    public async UniTask NextPage(float slideTime)
    {
        int sliderID = Shader.PropertyToID(m_propertyName);

        m_material.SetFloat(sliderID, m_startUpValue);

        await LoadTexture();
        m_image.enabled = true;
        await m_material.DOFloat(m_finalValue, sliderID, slideTime)
            .SetUpdate(true);   // timeScaleを0にしても動くように ロード演出などで時間を止めるなどすることあがあるため
    }
    public UniTask ScreenShot()
    {
        if (!m_capture)
        {
            m_capture = gameObject.AddComponent<CaptureScreenToImage>();
        }
        return m_capture.Capture();
    }
    private UniTask LoadTexture()
    {
        m_image.sprite = SceneCaptureManager.m_capturedImage;
        m_rect.sizeDelta = SceneCaptureManager.m_sizeDelta;
        return UniTask.CompletedTask;
    }
}


public sealed class NextPageTransitionDirector : ITransitionDirector
{
    readonly ISceneIdentifier m_sceneInfo;

    public NextPageTransitionDirector(ISceneIdentifier sceneInfo)
    {
        m_sceneInfo = sceneInfo;
    }

    public ITransitionHandle CreateHandle()
    {
        return new NextPageTransitionDirectorHandle(m_sceneInfo);
    }


    class NextPageTransitionDirectorHandle : ITransitionHandle
    {
        readonly ISceneIdentifier m_SceneInfo;
        ISceneHandle m_SceneHandle;
        NextPageTransitionBehaviour m_Director;

        public NextPageTransitionDirectorHandle(ISceneIdentifier sceneInfo)
        {
            m_SceneInfo = sceneInfo;
        }

        public async UniTask Start(CancellationToken cancellation = default)
        {
            var handle = m_SceneInfo.CreateHandle();
            var scene = await handle.Load(cancellationToken: cancellation);

            if (!scene.TryGetComponentInScene<NextPageTransitionBehaviour>(out m_Director, true))
            {
                throw new InvalidOperationException($"Scene '{scene.name}' does not have a {nameof(NextPageTransitionBehaviour)} component.");
            }

            m_SceneHandle = handle;

            await m_Director.NextPage(m_Director.m_slideMoveTime);

            Time.timeScale = 0;
        }

        public async UniTask End(CancellationToken cancellation = default)
        {
            Time.timeScale = 1;
            await m_Director.ScreenShot();

            if (m_Director.m_endHideObject != null)
            {
                m_Director.m_endHideObject.SetActive(false);
            }

            //await m_Director.NextPage(m_Director.m_endSlideTime);

            m_Director = null;

            await m_SceneHandle.Unload(null, cancellation);
            m_SceneHandle = null;
        }
    }

}