using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SelectUseItemInfo;

using UnityEngine.UI;
using TMPro;

public class SelectUseItemButton : MonoBehaviour
{

    // 制作者(田内)

    [Header("ボタン画像")]
    [SerializeField]
    private Image m_buttonImage = null;


    [Header("ボタンテキスト")]
    [SerializeField]
    private TextMeshProUGUI m_buttonText = null;



    //==========================================================================
    // 選択ID


    private SelectUseItemID m_id = new();

    public SelectUseItemID ID { get { return m_id; } }


    //===========================================================================
    // 押せるかどうか


    private bool m_isCanPress = true;

    public bool IsCanPress { get { return m_isCanPress; } }


    //===========================================================================

    public void SetData(SelectUseItemID _id, bool _active,string _text)
    {
        if(m_buttonText==null)
        {
            Debug.LogError("TextMeshProが存在しません");
            return;
        }

        // IDを更新
        m_id = _id;

        // 押せるかどうかを更新
        m_isCanPress = _active;

        // ボタンのテキストを更新
        m_buttonText.text = _text;

        // 押すことができなければ
        if (!m_isCanPress)
        {
            // 画像を少し黒く薄く
            if (m_buttonImage)
            {
                Color color = m_buttonImage.color;
                color.r = 0.2f;
                color.g = 0.2f;
                color.b = 0.2f;
                color.a = 0.5f;
                m_buttonImage.color = color;
            }
        }
    }



}
