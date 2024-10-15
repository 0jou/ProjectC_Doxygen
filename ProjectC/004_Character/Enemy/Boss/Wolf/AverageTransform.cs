/**
* @file AverageTransform.cs
* @brief TransformListの平均座標を自分の座標に更新
*/
using System.Collections.Generic;
using UnityEngine;

/**
* @brief TransformListの平均座標を計算　伊波
* @details 狼が岩掴む処理に使用　計算結果は自身のTransformに反映
*/
public class AverageTransform : MonoBehaviour
{
    [SerializeField] private List<Transform> m_transforms = new();

    void FixedUpdate()
    {
        Vector3 sumPos = Vector3.zero;
        Vector3 sumRot = Vector3.zero;
        foreach (Transform t in m_transforms)
        {
            sumPos += t.position;
            sumRot += t.eulerAngles;
        }
        transform.position = sumPos / m_transforms.Count;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(sumRot / m_transforms.Count), 0.5f * Time.fixedDeltaTime);
    }
}
