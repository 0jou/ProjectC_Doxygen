using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static ActionUIController;


// 制作者(吉田)

/// <summary>
/// アイテムを拾ったり、インタラクトする際のUI表示を制御するクラス
/// </summary>

public class AnyActionUIController : MonoBehaviour
{

    [SerializeField]
    private GameObject m_anyActionUI = null;
    [SerializeField]
    [ReadOnly]
    private TextMeshProUGUI m_anyActionUIText = null;
    [SerializeField]
    [ReadOnly]
    private InputActionButton_ChangeInputActionReference m_inputAction = null;

    //[SerializeField]
    //private Transform m_actionUIPlace = null;

    /// <summary>
    /// アクション情報
    /// データベースの方がいいかも。
    /// </summary>
    [Serializable]
    struct AnyActionInfo
    {
        /// <summary>
        /// 表示するアクション名
        /// </summary>
        public string ActionText;
        public InputActionReference InputActionReference;
        public float Distance;
    }

    [SerializeField]
    private SerializableDictionary<ActionUIState, AnyActionInfo> m_actionInfoList = new();

    [Header("現在のアクション状態")]
    [SerializeField]
    private ActionUIState m_nowActionState;

    /// <summary>
    /// m_nowActionState に応じたUIを表示する
    /// </summary>
    [Button("ChangeNowStateUI")]
    private void ChangeNowStateUI()
    {
        ChangeStateUI(m_nowActionState);
    }

    /// <summary>
    /// アクション状態を変更する
    /// </summary>
    /// <param name="_actionState"></param>
    public void ChangeStateUI(ActionUIState _actionState)
    {
        if(_actionState == ActionUIState.None)
        {
            m_anyActionUI.SetActive(false);
            return;
        }
        else
        {
            m_anyActionUI.SetActive(true);
        }

        if(m_anyActionUI == null)
        {
            Debug.LogError("m_anyActionUIが設定されていません");
            return;
        }

        if(m_inputAction == null)
        {
            m_inputAction = m_anyActionUI.GetComponentInChildren<InputActionButton_ChangeInputActionReference>();
            if (m_inputAction == null)
            {
                Debug.LogError("m_inputActionが設定されていません");
                return;
            }
        }

        if(m_anyActionUIText == null)
        {
            m_anyActionUIText = m_anyActionUI.GetComponentInChildren<TextMeshProUGUI>();
            if(m_anyActionUIText == null)
            {
                Debug.LogError("m_anyActionUITextが設定されていません");
                return;
            }
        }

        if(m_actionInfoList.ContainsKey(_actionState))
        {
            m_anyActionUIText.text = m_actionInfoList[_actionState].ActionText;
        }
        else
        {
            Debug.LogError("m_actionInfoListに" + _actionState + "が設定されていません");
        }

        if(m_inputAction.InputActionReference != m_actionInfoList[_actionState].InputActionReference)
        {
            m_inputAction.ChangeInputActionReference(m_actionInfoList[_actionState].InputActionReference);
        }

        m_nowActionState = _actionState;

    }



    /*
     * スロットでのアクションを選択する場合、以下のコードを使用する
     * 
    [Header("スロット管理")]
    [SerializeField]
    private ActionSlotList m_slotList = null;

    [Header("スロットコントローラー")]
    [SerializeField]
    private SelectUIController m_selectUIController = null;

       public void Start()
    {
        if (m_slotList == null)
        {
            Debug.LogError("m_slotListがシリアライズされていません");
            return;
        }

        if (m_selectUIController == null)
        {
            Debug.LogError("m_selectUIControllerがシリアライズされていません");
            return;
        }

        // UI選択の更新 UIリスト追加
        m_selectUIController.AddUIList(m_slotList.SlotList.Value);
        m_slotList.AddSlotAction = (slot) => m_selectUIController.AddUI(slot);
    }

    public void Update()
    {
        // UI選択の更新
        m_selectUIController.OnUpdate();

        m_selectUIController.OnLateUpdate();
    }

    */

}
