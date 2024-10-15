using Cysharp.Threading.Tasks;
using MackySoft.Navigathena.SceneManagement;
using MackySoft.Navigathena.SceneManagement.Utilities;
using MackySoft.Navigathena.Transitions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BlindTransitionDirectorBehaviour : MonoBehaviour
{
    [SerializeField] BlindAnimator m_blindAnimator;

    public UniTask CutIn()
    {
        return m_blindAnimator.CutIn();
    }
    public UniTask In()
    {
        return m_blindAnimator.In();
    }

    public UniTask Out()
    {
        return m_blindAnimator.Out();
    }
}

public sealed class BlindTransitionDirector : ITransitionDirector
{
    readonly ISceneIdentifier m_sceneInfo;

    public BlindTransitionDirector(ISceneIdentifier sceneInfo)
    {
        m_sceneInfo = sceneInfo;
    }

    public ITransitionHandle CreateHandle()
    {
        return new BlindTransitionDirectorHandle(m_sceneInfo);
    }


    class BlindTransitionDirectorHandle : ITransitionHandle
    {
        readonly ISceneIdentifier m_SceneInfo;
        ISceneHandle m_SceneHandle;
        BlindTransitionDirectorBehaviour m_Director;

        public BlindTransitionDirectorHandle(ISceneIdentifier sceneInfo)
        {
            m_SceneInfo = sceneInfo;
        }

        public async UniTask Start(CancellationToken cancellationToken = default)
        {
            var handle = m_SceneInfo.CreateHandle();
            var scene = await handle.Load(cancellationToken: cancellationToken);

            if (!scene.TryGetComponentInScene<BlindTransitionDirectorBehaviour>(out m_Director, true))
            {
                throw new InvalidOperationException($"Scene '{scene.name}' does not have a {nameof(BlindTransitionDirectorBehaviour)} component.");
            }

            m_SceneHandle = handle;

            await m_Director.In();
        }

        public async UniTask End(CancellationToken cancellationToken = default)
        {
            await m_Director.Out();

            m_Director = null;

            await m_SceneHandle.Unload(cancellationToken: cancellationToken);
            m_SceneHandle = null;
        }
    }

}