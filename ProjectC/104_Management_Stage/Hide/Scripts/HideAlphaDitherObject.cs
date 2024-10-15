using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAlphaDitherObject : MonoBehaviour
{
    // 制作者 田内

    [Header("使用するシェーダー(HideAlphaShaderをセットする)")]
    [SerializeField]
    private Shader m_hideAlphaDitherShader = null;

    public Shader HideAlphaDitherShader { get { return m_hideAlphaDitherShader; } set { m_hideAlphaDitherShader = value; } }





}
