using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using DG.Tweening;

public class HideAlphaDitherAreaObject : MonoBehaviour
{

    // エリア内or外にTagがいる場合、子オブジェクトを全て隠す
    // BoxColliderなどを付けること
    // 制作者　田内

    private class HideAreaObjctData
    {
        public GameObject HideObject = null;

        public Renderer Renderer = null;

    }

    [Header("使用するシェーダー(HideAlphaShaderをセットする)")]
    [SerializeField]
    private Shader m_hideAlphaDitherShader = null;


    //[Header("判定を行うTag")]
    //[SerializeField]
    //private string m_tag = "Player";

    [Header("内側にいると隠す")]
    [SerializeField]
    private bool m_HideWhenInside = true;


    private List<HideAreaObjctData> m_hideObjectList = new();


    //==========================================================


    private void Start()
    {
        GetHideAreaObjctData(this.gameObject);
        SetHideShader();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 非表示
            Hide(m_HideWhenInside);
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 表示
            Hide(!m_HideWhenInside);
        }
    }


    // 表示・非表示
    private void Hide(bool _isInside)
    {
        foreach (var obj in m_hideObjectList)
        {
            if (obj == null) continue;

            // マテリアルのプロパティを変更してアルファ値を操作
            foreach (var material in obj.Renderer.materials)
            {
                if (_isInside)
                {
                    material.SetFloat("_DitherValue", 1f); // アルファ値を1にして表示
                }
                else
                {
                    material.SetFloat("_DitherValue", 0f); // アルファ値を0にして隠す
                }
            }
        }
    }


    void GetHideAreaObjctData(GameObject _obj)
    {
        var children = _obj.GetComponentsInChildren<Transform>();

        foreach (var child in children)
        {
            // 親自身をスキップ
            if (child == _obj.transform) continue;

            // 管理データを作成
            HideAreaObjctData data = new()
            {
                HideObject = child.gameObject,
                Renderer = child.GetComponent<Renderer>()
            };

            // レンダーが存在すれば
            if (data.Renderer != null)
            {
                // 子オブジェクトを取得
                m_hideObjectList.Add(data);
            }
        }
    }


    // シェーダーを変更する
    void SetHideShader()
    {

        if (m_hideAlphaDitherShader == null)
        {
            Debug.LogError("指定されたシェーダーが存在しません");
            return;
        }

        foreach (var obj in m_hideObjectList)
        {

            if (obj?.Renderer == null) continue;

            foreach (var material in obj.Renderer.materials)
            {
                // マテリアルのシェーダーを変更
                material.shader = m_hideAlphaDitherShader;
            }

        }

    }



}
