using Cysharp.Threading.Tasks;
using MackySoft.Navigathena;
using MackySoft.Navigathena.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class TitleEntryPoint : EntryPointBase
{
    [Header("シーン遷移途中でカメラが重複しないよう制御する 登録しなくても可")]
    [SerializeField] private Camera m_camera = null;

    [Header("シーン遷移途中で削除するオブジェクト 遷移演出の直前に消される")]
    [SerializeField, NonReorderable] private List<UnityEngine.Object> m_deleteObjects = new List<UnityEngine.Object>();

    public string m_musicName;

    public TitleScreenManager titleScreenManager;

    // 遷移演出の終了後に呼び出されます。
    protected override async UniTask OnEnter(ISceneDataReader reader, CancellationToken cancellationToken)
    {
        if (titleScreenManager != null)
        {
            titleScreenManager.StartUp();
        }

        if (m_musicName.Any())
        {
            SoundManager.Instance.StartBGM(m_musicName);
        }
    }

    // 遷移演出の開始前に呼び出されます。
    protected override async UniTask OnExit(ISceneDataWriter writer, CancellationToken cancellationToken)
    {
        CaptureScreenToImage capture = gameObject.AddComponent<CaptureScreenToImage>();
        await capture.Capture();
        foreach (var obj in m_deleteObjects)
        {
            Destroy(obj);
        }
        if (m_camera)
        {
            m_camera.enabled = false;
        }
    }

    // 遷移演出の開始後に呼び出されます。
    protected override async UniTask OnFinalize(ISceneDataWriter writer, IProgress<IProgressDataStore> transitionProgress, CancellationToken cancellationToken)
    {
        SoundManager.Instance.StopBGM();
    }
}
