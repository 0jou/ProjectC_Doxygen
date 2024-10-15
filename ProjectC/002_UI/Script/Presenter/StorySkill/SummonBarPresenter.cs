using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem.XR;

/// <summary>
/// 召喚時間の値が変更されたらバーを更新する
/// </summary>
public class SummonBarPresenter : MonoBehaviour
{
    [SerializeField] private CharacterCore m_characterCore;

    [SerializeField] private HPBarController barController;
    [SerializeField] private ShowAndHideForGameObject m_showAndHide;

    public void Start()
    {
        // 初期化
        InitSetting();

        // 変わったら実行する処理を登録
        m_characterCore.PlayerParameters.CastTimeProgress.Subscribe(x => BarUpdate(x));
    }

    private void BarUpdate(float _value)
    {
        barController.SetHealthValue(_value);

        if(_value <= 0.0f || _value >= 1.0f )
        {
            m_showAndHide.OnHide();
        }
        else
        {
            m_showAndHide.OnShow();
        }
    }

    [Button("Init Setting")]
    private void InitSetting()
    {
        if (barController == null)
        {
            barController = GetComponent<HPBarController>();
        }

        // Controller側では計算しない
        barController.IsCalc = false;
        // 初期値を設定
        barController.SetHealthValue(
            m_characterCore.PlayerParameters.CastTimeProgress.Value);
    }

}
