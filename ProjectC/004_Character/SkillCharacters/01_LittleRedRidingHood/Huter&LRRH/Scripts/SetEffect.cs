using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEffect : MonoBehaviour
{
    [Header("出現するエフェクト")]
    [SerializeField] GameObject m_effectObjectPrefab;

    private GameObject m_effectObj;

    //出現と同時にエフェクトを表示するスクリプト（仮）（山本）
    void Start()
    {
        //エフェクト出現
        m_effectObj = Object.Instantiate(m_effectObjectPrefab, transform.position, Quaternion.identity);
        //スケール値０に
        m_effectObj.transform.localScale = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {
        m_effectObj.transform.position = transform.position;
        //スケール値を調整
        m_effectObj.transform.localScale = transform.localScale;
    }

    private void OnDestroy()
    {
        Destroy(m_effectObj);
    }


}
