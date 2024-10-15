using UniRx;
using UnityEngine;

/// <summary>
/// HPBarのPresenter(吉田)
/// 体力とHPBarを紐づける
/// </summary>
public class HPBarPresenter : MonoBehaviour
{
    [SerializeField] private CharacterCore characterCore;
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
        characterCore.Status.m_hp.Subscribe(x => BarUpdate());
    }

    private void BarUpdate()
    {
        barController.SetHealthValue(characterCore.Status.m_hp.Value);

        // デバッグ用
        //Debug.Log("HealthUpdate");
    }

    [ContextMenu("BarSetting")]
    private void BarSetting()
    {
        // HPの初期値を設定
        barController.SetHealth(
            characterCore.Status.m_hp.Value,
            characterCore.Status.MaxHP);
    }

}
