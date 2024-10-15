using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class SetRemainingTimeText : MonoBehaviour
{
    // 制作者 田内
    // 残り時間をテキストにセット

    [Header("変更するテキスト")]
    [SerializeField]
    private TextMeshProUGUI m_text = null;


    // Update is called once per frame
    void Update()
    {
        SetText();
    }

    void SetText()
    {
        if (m_text == null)
        {
            Debug.LogError("テキストがシリアライズされていません");
            return;
        }

        var timeLimit = ManagementGameManager.instance.TimeLimit;
        var elapsedTime = ManagementGameManager.instance.ElapsedTime;

        // 残りの時間を計算
        int time = (int)(timeLimit - elapsedTime);


        int minutes = time / 60;

        int seconds = time % 60;

        // テキストを更新
        m_text.text = minutes.ToString("00") + ":" + seconds.ToString("00");

    }
}
