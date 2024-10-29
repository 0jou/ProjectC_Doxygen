using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{

    //トータル制限時間
    private float m_totalTime;

    [Header("制限時間（分）")]
    [SerializeField]
    private int m_minute;

    [Header("制限時間（秒）")]
    [SerializeField]
    private float m_seconds;

    [SerializeField]
    private TextMeshProUGUI m_timerText;

    [SerializeField]
    private SceneTransitionManager m_sceneTransitionManager = null;

    [SerializeField]
    private CreateNonProXiWindow m_windowController = null;

    //前回Update時の秒数
    private float m_oldSecounds;


    void Start()
    {
        m_totalTime = m_minute * 60 + m_seconds;
        m_oldSecounds = 0.0f;

    }


    void Update()
    {
        if (m_totalTime <= 0.0f)
        {
            return;
        }

        m_totalTime = m_minute * 60 + m_seconds;
        m_totalTime -= Time.deltaTime;

        m_minute = (int)m_totalTime / 60;
        m_seconds = m_totalTime - m_minute * 60;

        //タイマー表示用UIテキストに時間を表示する

        if ((int)m_seconds != (int)m_oldSecounds)
        {
            m_timerText.text = m_minute.ToString("00") + ":" + ((int)m_seconds).ToString("00");
        }

        m_oldSecounds = m_seconds;

        if (m_totalTime <= 0.0f)
        {
            Debug.Log("制限時間終了");

            if(m_windowController)
            {
                _=m_sceneTransitionManager.SceneChange();
                return;
            
            }

        }

    }
}
