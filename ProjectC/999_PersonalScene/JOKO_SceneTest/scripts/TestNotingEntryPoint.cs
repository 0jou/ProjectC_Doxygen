using Cysharp.Threading.Tasks;
using MackySoft.Navigathena;
using MackySoft.Navigathena.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TestNotingEntryPoint : EntryPointBase
{
    [SerializeField] private CaptureScreenToImage m_capture;

    [Header("設定すれば遷移時カメラを無効にする")]
    [SerializeField] private Camera m_camera = null;

    [Header("遷移時削除するオブジェクト")]
    [SerializeField, NonReorderable] private List<UnityEngine.Object> m_deleteObjects = new List<UnityEngine.Object>();


    [Header("アクティブをtrueにするリスト")]
    [Header("※シリアライズするオブジェクトはアクティブをfalseにすること")]
    [SerializeField] private List<GameObject> m_objList = new();

    [SerializeField] private GameObject pagePrefab=null;

    // 遷移演出の終了後に呼び出されます。
    protected override async UniTask OnEnter(ISceneDataReader reader, CancellationToken cancellationToken)
    {
        // リストで更新(田内)
        foreach(var obj in m_objList)
        {
            obj.SetActive(true);
        }


        if (CutSceneManager.instance != null)
        {
            //入場イベント再生
            CutSceneManager.instance.PlayCutScene(CutSceneNumber.StartMovie);
        }



        if (pagePrefab)
        {
            GameObject instance = Instantiate(pagePrefab);

            NextPageTransitionBehaviour nextPageTransitionBehaviour = instance.GetComponentInChildren<NextPageTransitionBehaviour>();

            if (nextPageTransitionBehaviour)
            {
                await nextPageTransitionBehaviour.NextPage();

                

            }
            Destroy(instance);
        }




    }

    // タイトル画面は一番最初に実行されるものとして、ビルド時にも同様の必須初期化処理を実行させるための処理

#if UNITY_EDITOR
#else
    //protected override async UniTask OnInitialize(ISceneDataReader reader, IProgress<IProgressDataStore> transitionProgress, CancellationToken cancellationToken)
    //{
    //await OnFirstPreInitializeFunc();
    //}

#endif


    // 遷移演出の開始前に呼び出されます。
    protected override async UniTask OnExit(ISceneDataWriter writer, CancellationToken cancellationToken)
    {

       

        await m_capture.Capture();
        if (m_camera)
        {
            m_camera.enabled = false;
        }
        foreach (var obj in m_deleteObjects)
        {
            Destroy(obj);
        }
    }
}
