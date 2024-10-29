using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CustomMaskableGraphic : MonoBehaviour
{
    // 使用するカスタムマテリアル
    [SerializeField]
    private Material customMaterial;

    private void Start()
    {
        customMaterial.EnableKeyword("UNITY_UI_CLIP_RECT");
    }
}
