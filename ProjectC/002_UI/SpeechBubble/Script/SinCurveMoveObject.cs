using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinCurveMoveObject : MonoBehaviour
{
    // signcurveで動かす
    // 制作者　田内

    private enum SinCurveMoveType
    {
        Height, // 縦移動
        Width,  // 横移動
    }

    [Header("移動種類")]
    [SerializeField]
    private SinCurveMoveType m_sinCurveMoveType = SinCurveMoveType.Height;


    [Header("移動の振幅 ")]
    [SerializeField]
    private float m_amplitude = 0.1f;


    [Header("移動速度")]
    [SerializeField]
    private float m_spead = 1.0f;


    // 初期座標
    private Vector3 m_initPos = new();


    void Start()
    {
        m_initPos = transform.position;
    }

    void Update()
    {
        // Sine波に基づいてUIを上下に動かす
        float move = m_amplitude * Mathf.Sin(Time.time * m_spead);
        var pos = m_initPos;


        switch (m_sinCurveMoveType)
        {
            case SinCurveMoveType.Height:
                {
                    pos.y += move;
                    break;
                }
            case SinCurveMoveType.Width:
                {
                    pos.x += move;
                    break;
                }
        }

        transform.position = pos;
    }




}
