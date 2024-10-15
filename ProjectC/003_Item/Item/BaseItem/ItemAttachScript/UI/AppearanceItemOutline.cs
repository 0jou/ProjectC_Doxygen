using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Unity.VisualScripting;

public class AppearanceItemOutline : MonoBehaviour
{

    // 追加するマテリアル
    private Material m_addMat = null;

    private void Start()
    {
        //Addressableから取得
        Addressables.LoadAssetAsync<Material>("ItemOutlineMatrial")
            .Completed += OnMaterialLoaded;
    }

    // メッシュレンダラーに引数のマテリアルをコピーして追加する
    private void AddMaterial(Material _originalMat)
    {
        // オブジェクトが存在するか確認
        if (this == null || gameObject == null) return;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRendererが存在しません");
            return;
        }

        List<Material> materials = new List<Material>(meshRenderer.materials);
        m_addMat = new Material(_originalMat);// コピーを作成

        // 透明にしておく
        m_addMat.SetFloat("_Alpha", 0.0f);

        materials.Add(m_addMat);
        meshRenderer.SetMaterials(materials);
    }

    void OnMaterialLoaded(AsyncOperationHandle<Material> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            AddMaterial(handle.Result);
        }
        else
        {
            // Addressableでの取得に失敗した場合
            Debug.LogError("マテリアル取得失敗。");
        }
    }

    public void OnShow(float _duration)
    {
        if (m_addMat == null) return;
        m_addMat.DOFloat(endValue: 1.0f, property: "_Alpha", duration: _duration);
    }

    public void OnHide(float _duration)
    {
        if (m_addMat == null) return;
        m_addMat.DOFloat(endValue: 0.0f, property: "_Alpha", duration: _duration);
    }

    public void OnDestroy()
    {
        if (m_addMat == null) return;
        Destroy(m_addMat);
        m_addMat = null;
    }

}
