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

        PlayerStatus status = m_characterCore.PlayerParameters.PlayerStatus;
        // HPの初期設定
        BarSetting(status);

        // 変わったら実行する処理を登録
        status.m_bp.Subscribe(x => BarUpdate(status));
    }

    private void BarUpdate(PlayerStatus status)
    {
        barController.SetHealthValue(status.m_bp.Value);

        // デバッグ用
        Debug.Log("HealthUpdate");
    }

    [ContextMenu("BarSetting")]
    private void BarSetting(PlayerStatus status)
    {
        // BPの初期値を設定
        barController.SetHealth(
            status.m_bp.Value,
            status.MaxBP);
    }

}
