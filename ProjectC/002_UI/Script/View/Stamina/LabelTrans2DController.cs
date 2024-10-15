using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using Unity.VisualScripting;
using Arbor.Calculators;

public class LabelTrans2DController : MonoBehaviour
{
    // 追尾される3Dオブジェクト
    [SerializeField] private Transform m_target = null;

    [Header("画面座標でのずらす位置")]
    [SerializeField] private Vector2 m_offsetNear = new Vector2(0, 0);
    [SerializeField] private Vector2 m_offsetFar = new Vector2(0, 0);

    [Header("ターゲットとカメラの距離基準（一番遠い）")]
    [SerializeField] private float m_distMax = 9.0f;
    [Header("")]
    [SerializeField] private float m_speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (m_target == null)
        {
            Debug.LogError("m_targetがnullです");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (m_target == null)
        {
            return;
        }

        float dist = Vector3.Distance(Camera.main.transform.position, m_target.position);
        float distRate = (dist / m_distMax);
        distRate = Mathf.Clamp01(distRate);

        Vector2 offset = Vector2.Lerp(m_offsetNear, m_offsetFar, distRate);
        Vector2 screenPos = Change2DPos(m_target.position) + offset;

        //移動
        Vector3 movePos =
            Vector3.Lerp(gameObject.transform.position, screenPos, m_speed * Time.deltaTime);
        movePos.z = 0.0f;
        gameObject.transform.position = movePos;
    }

    private Vector2 Change2DPos(Vector3 _3DPos)
    {
        Vector2 pos2D =
            RectTransformUtility.WorldToScreenPoint(Camera.main, _3DPos);
        return pos2D;
    }

    [ContextMenu("PosUpdate")]
    private void PosUpdate()
    {
        if (m_target == null)
        {
            return;
        }

        Vector2 newPos = Change2DPos(m_target.position) + m_offsetFar;
        gameObject.transform.position = newPos;
    }
}
