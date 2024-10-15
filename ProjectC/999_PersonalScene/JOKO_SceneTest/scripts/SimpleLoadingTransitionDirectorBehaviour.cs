using Cysharp.Threading.Tasks;
using DG.Tweening;
using MackySoft.Navigathena.SceneManagement;
using MackySoft.Navigathena.SceneManagement.Utilities;
using MackySoft.Navigathena.Transitions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public sealed class SimpleLoadingTransitionDirectorBehaviour : MonoBehaviour
{

    [SerializeField]
    float m_fadeDuration = 0.5f;

    [SerializeField]
    Ease m_fadeEase = Ease.OutCubic;

    [SerializeField]
    CanvasGroup m_canvasGroup;

    public UniTask StartTransition(CancellationToken cancellationToken = default)
    {
        return m_canvasGroup.DOFade(1f, m_fadeDuration)
            .SetEase(m_fadeEase)
            .ToUniTask(cancellationToken: cancellationToken);
    }

    public UniTask EndTransition(CancellationToken cancellationToken = default)
    {
        return m_canvasGroup.DOFade(0f, m_fadeDuration)
            .SetEase(m_fadeEase)
            .ToUniTask(cancellationToken: cancellationToken);
    }
}

public sealed class SimpleLoadingTransitionDirector : ITransitionDirector
{

    readonly ISceneIdentifier m_sceneInfo;

    public SimpleLoadingTransitionDirector(ISceneIdentifier sceneInfo)
    {
        m_sceneInfo = sceneInfo;
    }

    public ITransitionHandle CreateHandle()
    {
        return new SimpleLoadingTransitionDirectorHandle(m_sceneInfo);
    }

    class SimpleLoadingTransitionDirectorHandle : ITransitionHandle
    {

        readonly ISceneIdentifier m_SceneInfo;
        ISceneHandle m_SceneHandle;
        SimpleLoadingTransitionDirectorBehaviour m_Director;

        public SimpleLoadingTransitionDirectorHandle(ISceneIdentifier sceneInfo)
        {
            m_SceneInfo = sceneInfo;
        }

        public async UniTask Start(CancellationToken cancellationToken = default)
        {
            var handle = m_SceneInfo.CreateHandle();
            var scene = await handle.Load(cancellationToken: cancellationToken);

            if (!scene.TryGetComponentInScene<SimpleLoadingTransitionDirectorBehaviour>(out m_Director, true))
            {
                throw new InvalidOperationException($"Scene '{scene.name}' does not have a {nameof(SimpleLoadingTransitionDirectorBehaviour)} component.");
            }

            m_SceneHandle = handle;

            await m_Director.StartTransition(cancellationToken);
        }

        public async UniTask End(CancellationToken cancellationToken = default)
        {
            await m_Director.EndTransition(cancellationToken);
            m_Director = null;

            await m_SceneHandle.Unload(cancellationToken: cancellationToken);
            m_SceneHandle = null;
        }
    }
}