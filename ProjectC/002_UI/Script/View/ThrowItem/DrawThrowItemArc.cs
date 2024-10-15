using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// アイテム投の放物線を描画するクラス（吉田）
/// 参考：https://qiita.com/_udonba/items/860041daa27e0b7ffa6c
/// </summary>
public class DrawThrowItemArc : MonoBehaviour
{
    /// <summary>
    /// 放物線の描画ON/OFF
    /// </summary>
    private bool m_showArc = true;
    public bool ShowArc { set { m_showArc = value; } }

    /// <summary>
    /// 放物線を構成する線分の数
    /// </summary>
    [SerializeField]
    private int segmentCount = 100;

    /// <summary>
    /// 放物線を長さ計算するか
    /// </summary>
    private float predictionTime = 6.0F;

    /// <summary>
    /// 放物線のMaterial
    /// </summary>
    [SerializeField, Tooltip("放物線のマテリアル")]
    private Material arcMaterial;

    /// <summary>
    /// 放物線の幅
    /// </summary>
    [SerializeField, Tooltip("放物線の幅")]
    private float arcWidth = 0.02F;

    /// <summary>
    /// 放物線描画の親オブジェクト
    /// </summary>
    private GameObject arcObjectsParent = null;

    /// <summary>
    /// 放物線を構成するLineRenderer
    /// </summary>
    private LineRenderer[] lineRenderers;

    /// <summary>
    /// 生成開始座標
    /// </summary>
    private Vector3 startPos;
    public Vector3 StartPos { set { startPos = value; } }

    /// <summary>
    /// 初速度
    /// </summary>
    private Vector3 throwPower = Vector3.zero;
    public Vector3 ThrowPower { set { throwPower = value; } }

    ///// <summary>
    ///// 着弾マーカーオブジェクトのPrefab
    ///// </summary>
    //[SerializeField, Tooltip("着弾地点に表示するマーカーのPrefab")]
    //private GameObject pointerPrefab;

    ///// <summary>
    ///// 着弾点のマーカーのオブジェクト
    ///// </summary>
    //private GameObject pointerObject;


    void Start()
    {
        //Addressableから取得
        Addressables.LoadAssetAsync<Material>("ThrowItemArcMatrial")
            .Completed += OnMaterialLoaded;
    }

    void Update()
    {
        if (m_showArc && arcObjectsParent != null)
        {
            // 放物線を表示
            float timeStep = predictionTime / segmentCount;
            bool draw = false;
            float hitTime = float.MaxValue;
            for (int i = 0; i < segmentCount; i++)
            {
                // 線の座標を更新
                float startTime = timeStep * i;
                float endTime = startTime + timeStep;
                SetLineRendererPosition(i, startTime, endTime, !draw);

                // 衝突判定
                if (!draw)
                {
                    hitTime = GetArcHitTime(startTime, endTime);
                    if (hitTime != float.MaxValue)
                    {
                        draw = true; // 衝突したらその先の放物線は表示しない
                    }
                }
            }

            // マーカーの表示
            if (hitTime != float.MaxValue)
            {
                Vector3 hitPosition = GetArcPositionAtTime(hitTime);
                ShowPointer(hitPosition);
            }
        }
        else
        {
            if (arcObjectsParent)
            {
                // 放物線とマーカーを表示しない
                for (int i = 0; i < lineRenderers.Length; i++)
                {
                    lineRenderers[i].enabled = false;
                }
                //pointerObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 指定時間に対するアーチの放物線上の座標を返す
    /// </summary>
    /// <param name="time">経過時間</param>
    /// <returns>座標</returns>
    private Vector3 GetArcPositionAtTime(float time)
    {
        return (startPos + ((throwPower * time) + (0.5f * time * time) * Physics.gravity));
    }

    /// <summary>
    /// LineRendererの座標を更新
    /// </summary>
    /// <param name="index"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    private void SetLineRendererPosition(int index, float startTime, float endTime, bool draw = true)
    {
        lineRenderers[index].SetPosition(0, GetArcPositionAtTime(startTime));
        lineRenderers[index].SetPosition(1, GetArcPositionAtTime(endTime));
        lineRenderers[index].enabled = draw;
    }

    /// <summary>
    /// LineRendererオブジェクトを作成
    /// </summary>
    private void CreateLineRendererObjects()
    {
        // 親オブジェクトを作り、LineRendererを持つ子オブジェクトを作る
        arcObjectsParent = new GameObject("ArcObject");

        lineRenderers = new LineRenderer[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            GameObject newObject = new GameObject("LineRenderer_" + i);
            newObject.transform.SetParent(arcObjectsParent.transform);
            lineRenderers[i] = newObject.AddComponent<LineRenderer>();

            // 光源関連を使用しない
            lineRenderers[i].receiveShadows = false;
            lineRenderers[i].reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            lineRenderers[i].lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            lineRenderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            // 線の幅とマテリアル
            List<Material> materials = new List<Material>();
            materials.Add(arcMaterial);
            lineRenderers[i].SetMaterials(materials);
            lineRenderers[i].startWidth = arcWidth;
            lineRenderers[i].endWidth = arcWidth;
            lineRenderers[i].numCapVertices = 5;
            lineRenderers[i].enabled = false;
        }
    }

    /// <summary>
    /// 指定座標にマーカーを表示
    /// </summary>
    /// <param name="position"></param>
    private void ShowPointer(Vector3 position)
    {
        //pointerObject.transform.position = position;
        //pointerObject.SetActive(true);
    }

    /// <summary>
    /// 2点間の線分で衝突判定し、衝突する時間を返す
    /// </summary>
    /// <returns>衝突した時間(してない場合はfloat.MaxValue)</returns>
    private float GetArcHitTime(float startTime, float endTime)
    {
        // Linecastする線分の始終点の座標
        Vector3 startPosition = GetArcPositionAtTime(startTime);
        Vector3 endPosition = GetArcPositionAtTime(endTime);

        // 衝突判定
        RaycastHit hitInfo;
        if (Physics.Linecast(startPosition, endPosition, out hitInfo))
        {
            // 衝突したColliderまでの距離から実際の衝突時間を算出
            float distance = Vector3.Distance(startPosition, endPosition);
            return startTime + (endTime - startTime) * (hitInfo.distance / distance);
        }
        return float.MaxValue;
    }

    private void OnMaterialLoaded(AsyncOperationHandle<Material> handle)
    {
        if (this == null || gameObject == null) return;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            arcMaterial = handle.Result;
            CreateLineRendererObjects();
        }
        else
        {
            // Addressableでの取得に失敗した場合
            Debug.LogError("マテリアル取得失敗。");
        }
    }

    private void OnDestroy()
    {
        Destroy(arcObjectsParent);
    }
}

