using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;


[RequireComponent(typeof(BoxCollider))]
public class ProximityCreateWindow : MonoBehaviour
{

    // 近くでactionボタンを押すことでシリアライズしたウィンドウを作成する
    // BoxColliderが必要です
    // 制作者　田内


    [Header("作成するウィンドウ")]
    [SerializeField]
    private WindowController m_windowController = null;


    [Header("タグ")]
    [SerializeField]
    private string m_tag = "Player";


    // ウィンドウが作成できるかどうか
    private bool m_isCreate = false;

    public bool IsCreate { get { return m_isCreate; } }

    // 作成したウィンドウ保持
    private WindowController m_createWindow = null;


    private void Start()
    {
        // MetaAIに登録
        IMetaAI<ProximityCreateWindow>.Instance.RegisterObject(this);
    }


    // ウィンドウを作成
    public async UniTask CreateWindow()
    {
        if (m_isCreate == false) return;
        if (m_createWindow != null) return;

        if (m_windowController == null)
        {
            Debug.LogError("windowContorollerがシリアライズされていません");
            return;
        }


        //SE音追加（山本）
        SoundManager.Instance.StartPlayback("SceneChange");

        // 作成
        m_createWindow = Instantiate(m_windowController);
        await m_createWindow.CreateWindow<BaseWindow>();
        Destroy(m_createWindow.gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(m_tag))
        {
            m_isCreate = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag(m_tag))
        {
            m_isCreate = false;
        }
    }

}
