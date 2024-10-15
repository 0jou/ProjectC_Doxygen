using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using NaughtyAttributes;

using StageInfo;

public class SelectStageController : MonoBehaviour
{

    // 制作者　田内
    // ステージを選択するコントローラー

    //=====================
    // 選択中のステージID

    private StageID m_currentSelectStageID = StageID.Stage01;

    public StageID CurrentSelectStageID { get { return m_currentSelectStageID; } }


    //==========================
    // 選択が変更されたか

    bool m_isSelectChangeFlg = false;

    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// 現在選択しているステージが変更されたかどうか確認する、読み取り専用プロパティ
    /// </summary>
    /// --------------------------------------
    /// <return>
    /// true - 変更された
    /// false - 変更されていない
    /// </return>
    /// --------------------------------------
    #endregion
    public bool IsSelectChangeFlg { get { return m_isSelectChangeFlg; } }

    //==================
    // ボタン

    [BoxGroup("Button")]
    [Header("次に進むボタン")]
    [SerializeField]
    private InputActionButton m_nextInputActionButton = null;

    [BoxGroup("Button")]
    [Header("前に戻るボタン")]
    [SerializeField]
    private InputActionButton m_backInputActionButton = null;

    [BoxGroup("Button")]
    [Header("スタートボタン")]
    [SerializeField]
    private InputActionButton m_startInputActionButton = null;


    //===============================================================================

    public void OnUpdate()
    {
        Select();
    }

    public void OnLateUpdate()
    {
        m_isSelectChangeFlg = false;
    }


    public StageData GetCurrentSelectStageData()
    {
        StageData stageData = StageDataBaseManager.instance.GetStageData(m_currentSelectStageID);
        return stageData;
    }

    public bool IsStart()
    {
        if (IsGoStart())
        {
            return m_startInputActionButton.IsInputActionTrriger();
        }

        return false;
    }


    private void Select()
    {
        // 次に進む
        Next();

        // 前に戻る
        Back();
    }



    private void Next()
    {
        if (m_nextInputActionButton == null) return;

        if (m_nextInputActionButton.IsInputActionTrriger())
        {
            if (IsGoNext())
            {
                m_currentSelectStageID++;
                m_isSelectChangeFlg = true;
            }
        }
    }



    private void Back()
    {
        if (m_backInputActionButton == null) return;

        if (m_backInputActionButton.IsInputActionTrriger())
        {
            if (IsGoBack())
            {
                m_currentSelectStageID--;
                m_isSelectChangeFlg = true;
            }
        }
    }



    // 次に進めるかどうか
    public bool IsGoNext()
    {
        if (m_currentSelectStageID < StageID.Max - 1)
        {
            return true;
        }

        return false;

    }


    // 前に戻れるかどうか
    public bool IsGoBack()
    {
        if (StageID.Min + 1 < m_currentSelectStageID)
        {
            return true;
        }

        return false;
    }


    // スタートできるかどうか
    public bool IsGoStart()
    {
        return true;
    }


}
