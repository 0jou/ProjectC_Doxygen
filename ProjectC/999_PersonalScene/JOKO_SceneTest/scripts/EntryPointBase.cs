using CI.QuickSave;
using Cysharp.Threading.Tasks;
using MackySoft.Navigathena;
using MackySoft.Navigathena.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// どのシーンから起動しても共通の初期化、処理などを実行し特定のシーンからの遷移しないと動かないといった症状を防ぐための基底クラス
/// 基本的にOnEditorFirstPreInitializeに記述される前提だが、エディタ時のみ実行される処理のためビルド時には動かないといった事が起こりうるため注意
/// </summary>
public class EntryPointBase : SceneEntryPointBase
{
    // 初期化時に追加するプレファブ
    [Header("初期化時に追加するプレファブ群")]
    [SerializeField] private List<GameObject> initializeObjects = new List<GameObject>();

    private UniTask OnFirstPreInitializeFunc()
    {
        // セーブデータの保存先(共用版)
        QuickSaveGlobalSettings.StorageLocation = "Assets/ProjectC/998_DebugSaveData";



       foreach (var obj in initializeObjects)
        {
            Instantiate(obj);
        }



       return UniTask.CompletedTask;
    }



#if UNITY_EDITOR
    // エディタ内での実行時、一番最初にロードされたシーンで`OnInitialize`の前に呼び出される。 (エディタ専用)
    protected override async UniTask OnEditorFirstPreInitialize(ISceneDataWriter writer, CancellationToken cancellationToken)
    {
        await OnFirstPreInitializeFunc();
    }
#endif

    // 必要であればコメントアウト解除して使用してください。

    //// 遷移演出の開始後に呼び出されます。
    //protected override async UniTask OnInitialize(ISceneDataReader reader, IProgress<IProgressDataStore> transitionProgress, CancellationToken cancellationToken)
    //{
    //}

    //// 遷移演出の終了後に呼び出されます。
    //protected override async UniTask OnEnter(ISceneDataReader reader, CancellationToken cancellationToken)
    //{
    //}

    //// 遷移演出の開始前に呼び出されます。
    //protected override async UniTask OnExit(ISceneDataWriter writer, CancellationToken cancellationToken)
    //{
    //}

    //// 遷移演出の開始後に呼び出されます。
    //protected override async UniTask OnFinalize(ISceneDataWriter writer, IProgress<IProgressDataStore> transitionProgress, CancellationToken cancellationToken)
    //{
    //}

}
