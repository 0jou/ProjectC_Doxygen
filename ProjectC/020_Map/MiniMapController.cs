using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEditor.SearchService;
using UnityEngine;

public class MiniMapController : MonoBehaviour
{
    [Header("北固定にするか")]
    [SerializeField]
    private BoolReactiveProperty m_isNorthFixed = new(false);

    [Space(10)]
    [Header("以下は基本触らない")]
    [Header("Start() 又は コンテキストメニュー で設定する")]

    [SerializeField]
    private MiniMapTargetDirection m_miniMapTargetDirection = null;

    [SerializeField]
    private MapShowObjectTargetDirection m_cameraMapShowDirection = null;

    [SerializeField]
    private List<MapShowObjectTargetDirection> m_mapShowDirectionList = new();

    async void Start()
    {
        if (m_miniMapTargetDirection == null)
        {
           await SearchMiniMapTargetDirection();
        }
        if (m_mapShowDirectionList.Count == 0)
        {
            await SearchMapShowObjectTargetDirection();
        }

        m_isNorthFixed.Subscribe(_value =>
        {
            if (_value)
            {
                NorthFixed();
            }
            else
            {
                NorthUnFixed();
            }
        });

    }

    // 北固定の場合
    void NorthFixed()
    {
        if (m_miniMapTargetDirection == null) return;

        m_miniMapTargetDirection.enabled = false;

        foreach (var mapShowDirection in m_mapShowDirectionList)
        {
            mapShowDirection.enabled = false;
        }

        // カメラ方向は回転あり
        if (m_cameraMapShowDirection != null)
        {
            m_cameraMapShowDirection.enabled = true;
        }
    }

    // 北固定解除の場合
    void NorthUnFixed()
    {
        if (m_miniMapTargetDirection == null) return;

        m_miniMapTargetDirection.enabled = true;

        foreach (var mapShowDirection in m_mapShowDirectionList)
        {
            mapShowDirection.enabled = true;
        }

        // カメラ方向は回転なし
        if (m_cameraMapShowDirection != null)
        {
            m_cameraMapShowDirection.enabled = true;
        }
    }


    [ContextMenu("SearchMiniMapTargetDirection")]
    async UniTask SearchMiniMapTargetDirection()
    {
        // メインスレッドで Unity API を実行
        await UniTask.SwitchToMainThread();

        m_miniMapTargetDirection = FindObjectOfType<MiniMapTargetDirection>();
    }

    [ContextMenu("SearchMapShowObjectTargetDirection")]
    async UniTask SearchMapShowObjectTargetDirection()
    {
        // メインスレッドで Unity API を実行
        await UniTask.SwitchToMainThread();

        m_mapShowDirectionList = 
            new List<MapShowObjectTargetDirection>(FindObjectsOfType<MapShowObjectTargetDirection>());

        // カメラに引っ付いてるやつを取得
        foreach (var mapShowDirection in m_mapShowDirectionList)
        {
            if (mapShowDirection.gameObject.name.Contains("Camera"))
            {
                m_cameraMapShowDirection = mapShowDirection;
                break;
            }
        }

        m_mapShowDirectionList.Remove(m_cameraMapShowDirection);
    }


}
