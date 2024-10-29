using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shader_Global_TimeSpeed : MonoBehaviour
{
    /// <summary>
    /// シェーダー全体で使用可能な時間の速さ
    /// </summary>
    [SerializeField, Range(0.0f, 10.0f)]
    float globalTimeSpeed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        // シェーダー側のグローバル変数にセットする
        Shader.SetGlobalFloat("_GlobalTimeSpeed", globalTimeSpeed);
    }
}
