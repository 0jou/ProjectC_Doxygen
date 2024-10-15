using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using DG.Tweening;
using Cysharp.Threading.Tasks;

public partial class ProximityCreateUI : MonoBehaviour
{
    // m_tagのオブジェクトと当たり判定を行い、UIを作成する
    // 作成用BoxCollider等の当たり判定が必要
    // 制作者　田内

    public enum ProximityCreateUIDestroyType
    {
        Collider,     // 共通当たり判定で削除
        EndCollider,  // 削除用当たり判定で削除
        Time,         // 時間経過で削除
    }



    //=============================================================

    [Header("当たり判定を行うTag")]
    [SerializeField]
    [Tag]
    private string m_tag = "Player";

    //=============================================================

    [Header("再度作成する")]
    [SerializeField]
    private bool m_showAgainFlg = true;

    //==============================================================

    [Header("作成するチュートリアルUIプレハブ")]
    [SerializeField]
    private GameObject m_uiPrefab = null;

    // 作成したウィンドウ保持
    private GameObject m_createUI = null;


    [Header("Fade間隔")]
    [SerializeField]
    [Range(0.01f, 10.0f)]
    private float m_doDuration = 0.5f;

    //============================================================

    [Header("削除判定の種類")]
    [SerializeField]
    private ProximityCreateUIDestroyType m_type = ProximityCreateUIDestroyType.Collider;


    //==============================================================
    // 作成可能状態か

    private bool m_isCreate = false;

    //====================================================================-

    // 当たり判定でウィンドウを作成可能状態にする
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(m_tag))
        {
            // 作成可能状態
            m_isCreate = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitCollider(other);
    }


    private void Update()
    {
        CreateUI();

        UpdateCollider();
        UpdateEndCollider();
        UpdateTime();
    }


    private void CreateUI()
    {
        // 作成できないor作成済み
        if (!m_isCreate) return;
        if (m_createUI != null) return;

        if (m_uiPrefab == null)
        {
            Debug.LogError("作成するUIプレハブがシリアライズされていません");
            return;
        }

        // ウィンドウを作成
        m_createUI = Instantiate(m_uiPrefab);

        // 表示
        ShowUI();

    }


    private void DestroyUI()
    {
        // 作成したウィンドウを削除する
        if (m_createUI != null)
        {
            Destroy(m_createUI);
        }

        // 再度作成可能でなければ
        if (m_showAgainFlg == false)
        {
            Destroy(gameObject);
        }

    }


    // 作成したUIからキャンバスグループを取得する
    private CanvasGroup GetCanvasGroup()
    {
        if (m_createUI == null) return null;
        return m_createUI.GetComponent<CanvasGroup>();
    }


    private void ShowUI()
    {
        var canvasGroup = GetCanvasGroup();
        if (canvasGroup == null) return;

        // 一度透明度を0にする
        canvasGroup.alpha = 0.0f;

        // フェードイン
        canvasGroup.DOFade(endValue: 1.0f, duration: m_doDuration).
             SetLink(gameObject);
    }


    private void HideUI()
    {
        var canvasGroup = GetCanvasGroup();
        if (canvasGroup == null)
        {
            DestroyUI();
            return;
        }

        // フェードアウト
        canvasGroup.DOFade(endValue: 0.0f, duration: m_doDuration).
             SetLink(gameObject).OnComplete(() =>
             {
                 DestroyUI();
             });
    }



    // キャンバスグループリストを取得する
    private List<CanvasGroup> GetCanvasGroupList()
    {
        List<CanvasGroup> canvasGroups = new();

        foreach (GameObject obj in FindObjectsOfType(typeof(GameObject)))
        {
            // シーン上に存在するオブジェクトならば処理.
            if (!obj.activeInHierarchy) continue;

            if (obj.layer != LayerMask.NameToLayer("UI")) continue;

            if (obj.TryGetComponent<CanvasGroup>(out CanvasGroup canvasGroup))
            {
                canvasGroups.Add(canvasGroup);
            }
        }

        return canvasGroups;
    }







}
