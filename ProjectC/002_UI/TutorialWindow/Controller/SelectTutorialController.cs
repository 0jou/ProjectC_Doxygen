using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using NaughtyAttributes;

[System.Serializable]
public class TutorialData
{
    [Header("画像")]
    [SerializeField]
    private Sprite m_sprite = null;

    public Sprite Sprite
    {
        get { return m_sprite; }
    }


    [Header("テキスト")]
    [TextArea(1, 10)]
    [SerializeField]
    private string m_text = null;

    public string Text
    {
        get { return m_text; }
    }
}

public class SelectTutorialController : MonoBehaviour
{
    // 制作者 田内

    [Header("チュートリアル情報")]
    [SerializeField]
    private List<TutorialData> m_tutorialDataList = new();

    public List<TutorialData> TutorialDataList
    {
        get { return m_tutorialDataList; }
    }

    //========================
    // 選択中のチュートリアル

    private int m_currentTutorial = 0;

    public int CurrentTutorial
    {
        get { return m_currentTutorial; }
    }

    //====================
    // 変更されたかどうか

    bool m_isSelectChangeFlg = false;

    public bool IsSelectChangeFlg { get { return m_isSelectChangeFlg; } }


    //==================
    // ボタン

    [BoxGroup("Button")]
    [Header("次に進むボタン")]
    [SerializeField]
    private SelectTutorialTutorialInputActiomButton m_nextInputActionButton = null;

    [BoxGroup("Button")]
    [Header("前に戻るボタン")]
    [SerializeField]
    private SelectTutorialTutorialInputActiomButton m_backInputActionButton = null;

    [BoxGroup("Button")]
    [Header("終了ボタン")]
    [SerializeField]
    private SelectTutorialTutorialInputActiomButton m_closeInputActionButton = null;

    //==============================================
    //              実行処理
    //==============================================

    /// <summary>
    /// 実行処理
    /// </summary>
    public void OnUpdate()
    {
        Select();
    }

    /// <summary>
    /// 後実行処理
    /// </summary>
    public void OnLateUpdate()
    {
        m_isSelectChangeFlg = false;
    }


    /// <summary>
    /// 現在のチュートリアル情報を取得
    /// </summary>
    public TutorialData GetCurrentTutorialData()
    {
        if (m_tutorialDataList.Count <= 0) return null;

        return m_tutorialDataList[m_currentTutorial];
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
                m_currentTutorial++;
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
                m_currentTutorial--;
                m_isSelectChangeFlg = true;
            }
        }
    }


    // 次に進めるかどうか
    public bool IsGoNext()
    {
        if (m_currentTutorial < m_tutorialDataList.Count - 1)
        {
            return true;
        }

        return false;

    }


    // 前に戻れるかどうか
    public bool IsGoBack()
    {
        if (0 < m_currentTutorial)
        {
            return true;
        }

        return false;
    }


    // 終了できるかどうか
    public bool IsGoClose()
    {
        if (m_tutorialDataList.Count - 1 <= m_currentTutorial)
        {
            return true;
        }

        return false;
    }

}
