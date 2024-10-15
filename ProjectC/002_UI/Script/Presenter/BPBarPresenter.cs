using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class BPBarPresenter : MonoBehaviour
{
    [SerializeField] private CharacterCore m_characterCore;
    [SerializeField] private HPBarController barController;

    public void Start()
    {
        if (barController == null)
        {
            barController = GetComponent<HPBarController>();
        }

        // HPの初期設定
        BarSetting();

        // 変わったら実行する処理を登録
        m_characterCore.Status.m_bp.Subscribe(x => BarUpdate());
    }

    private void BarUpdate()
    {
        barController.SetHealthValue(m_characterCore.Status.m_bp.Value);

        // デバッグ用
        Debug.Log("HealthUpdate");
    }

    [ContextMenu("BarSetting")]
    private void BarSetting()
    {
        // BPの初期値を設定
        barController.SetHealth(
            m_characterCore.Status.m_bp.Value,
            m_characterCore.Status.MaxBP);
    }

}
