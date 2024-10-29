using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.InputSystem;

using NaughtyAttributes;
using UniRx;


[RequireComponent(typeof(CanvasGroup))]
public class InputActionButton : MonoBehaviour
{
    // 制作者　田内
    // InputActionに対応させたボタン

    //===============================================
    // メイン

    public enum InputActionButtonUpdateType
    {
        Auto,
        Self,
    }

    /// <summary>
    /// 実行の種類
    /// Auto - 勝手に実行:Self - 自身で実行
    /// </summary>
    [BoxGroup("実行")]
    [Header("実行タイプ")]
    [SerializeField]
    private InputActionButtonUpdateType m_updateType = InputActionButtonUpdateType.Auto;

    public InputActionButtonUpdateType UpdateType
    {
        get { return m_updateType; }
        set { m_updateType = value; }
    }


    [Header("対応キー")]
    [SerializeField]
    protected InputActionReference m_inputActionReference = null;

    public InputActionReference InputActionReference
    {
        get { return m_inputActionReference; }
    }


    [BoxGroup("Button")]
    [Header("ボタン画像")]
    [SerializeField]
    protected Image m_buttonImage = null;


    //===============================================
    // 長押し用

    private enum HoldType
    {
        Disable,
        Enable,
    }

    [BoxGroup("Gage")]
    [Header("長押しゲージを表示するか")]
    [SerializeField]
    private HoldType m_holdType = HoldType.Disable;

    [BoxGroup("Gage")]
    [EnableIf("m_holdType", HoldType.Enable)]
    [Header("ゲージ画像")]
    [SerializeField]
    private Image m_gaugeImage = null;

    //==================
    // SE

    [BoxGroup("SE")]
    [Header("Press")]
    [SerializeField]
    private string m_pressSE = "";

    //==================
    // Color

    [BoxGroup("Color")]
    [Header("キャンバスグループ")]
    [SerializeField]
    private CanvasGroup m_canvasGroup = null;

    [BoxGroup("Color")]
    [Header("使用不可色要素")]
    [SerializeField]
    private float m_imPossibleAlpha = 0.5f;

    //================================================
    //              実行処理
    //================================================

    private void Start()
    {
        // デバイスが変更時にボタン画像を更新
        PlayerInputManager.instance.CurrentDevice.Subscribe(device =>
        {
            UpdateButtonImage(device);
        }
        ).AddTo(this);

        // 自動で実行する場合のみ
        switch (m_updateType)
        {
            case InputActionButtonUpdateType.Auto:
                {
                    OnInitialize();
                    break;
                }
        }
    }


    private void Update()
    {
        // 自動で実行する場合のみ
        switch (m_updateType)
        {
            case InputActionButtonUpdateType.Auto:
                {
                    OnUpdate();
                    break;
                }
        }
    }


    /// <summary>
    /// 初期化処理
    /// </summary>
    public void OnInitialize()
    {
        SetColor();
    }

    /// <summary>
    /// 実行処理
    /// </summary>
    public void OnUpdate()
    {
        PlaySE();
        UpdateHoldGage();
        SetColor();
    }


    /// <summary>
    /// ボタンを選択したかどうか
    /// </summary>
    public bool IsInputActionTrriger()
    {
        #region nullチェック
        if (m_inputActionReference == null)
        {
            Debug.LogError("InputActionReferenceがシリアライズされていません");
            return false;
        }
        #endregion

        if (IsPress() == false) return false;

        m_inputActionReference.action.Enable();
        return m_inputActionReference.action.triggered;
    }


    // ボタンを押せるか同か
    virtual protected bool IsPress()
    {
        // ボタンが使用不可能の場合を作りたい場合、このメソッドをオーバーライドする
        return true;
    }


    // ボタン画像更新
    protected void UpdateButtonImage(DeviceTypes _deviceTypes)
    {
        #region nullチェック
        if (m_buttonImage == null)
        {
            Debug.LogError("ButtonImageがシリアライズされていません");
            return;
        }
        #endregion

        Sprite sprite = PlayerInputManager.instance.InputActionButtonDataBase.GetSprite(m_inputActionReference, _deviceTypes);

        if (sprite == null)
        {
            m_buttonImage.enabled = false;
        }
        else
        {
            m_buttonImage.enabled = true;
            m_buttonImage.sprite = sprite;
        }
    }


    // 長押しゲージ更新
    private void UpdateHoldGage()
    {
        if (m_holdType != HoldType.Enable) return;
        if (IsPress() == false)
        {
            m_gaugeImage.fillAmount = 0;
            return;
        }

        #region nullチェック
        if (m_inputActionReference == null)
        {
            Debug.LogError("InputActionReferenceがシリアライズされていません");
            return;
        }
        if (m_gaugeImage == null)
        {
            Debug.LogError("ゲージ用画像がシリアライズされていません");
            return;
        }
        #endregion

        m_inputActionReference.action.Enable();
        float progress = m_inputActionReference.action.GetTimeoutCompletionPercentage();

        // 進捗
        m_gaugeImage.fillAmount = progress;

    }


    // SEを再生
    private void PlaySE()
    {
        #region nullチェック
        if (m_inputActionReference == null)
        {
            Debug.LogError("InputActionReferenceがシリアライズされていません");
            return;
        }
        #endregion

        if (IsPress() == false) return;
        if (m_pressSE.Length <= 0) return;

        // 選択音
        if (m_inputActionReference.action.triggered)
        {
            try
            {
                SoundManager.Instance.StartPlayback(m_pressSE);
            }
            catch
            {

            }
        }
    }


    // 色をセット
    private void SetColor()
    {
        #region nullチェック
        if (m_canvasGroup == null)
        {
            Debug.LogError("CanvasGroupがシリアライズされていません");
            return;
        }
        #endregion

        // 可能であれば
        if (IsPress()) m_canvasGroup.alpha = 1.0f;
        else m_canvasGroup.alpha = m_imPossibleAlpha;
    }

}
