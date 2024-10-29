using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Localization.Settings;
using Cysharp.Threading.Tasks;

public class LocalizeTipsController : MonoBehaviour
{
    private int m_currentIndex = 0; // 選ばれているインデックス
    private int m_maxIndex = 0;

    [SerializeField] LocalizedStringTable m_localizedSpriteTable; // 参照するテーブル

    [SerializeField] TextMeshProUGUI m_textUI; // テキストUI


    private void Start()
    {
        m_currentIndex = 0;
        m_maxIndex = m_localizedSpriteTable.GetTable().Count;
    }

    [ContextMenu("Increment")]
    public void Increment()
    {
        AddIndex(1);
        ChangeText();
    }
    [ContextMenu("Decrement")]
    public void Decrement()
    {
        AddIndex(-1);
        ChangeText();
    }




    string GetTableString(int index)
    {
        var table=m_localizedSpriteTable.GetTable();

        var entry = table.GetEntry(index.ToString("000"));

        return entry.LocalizedValue;
    }

    private void ChangeText()
    {
        if (m_textUI == null)
        {
            return;
        }

        m_textUI.text = GetTableString(m_currentIndex);

        Debug.Log(m_textUI.text);
    }

    private void AddIndex(int index)
    {
        m_maxIndex = m_localizedSpriteTable.GetTable().Count;

        m_currentIndex += index;


        if (m_currentIndex >= m_maxIndex)
        {
            m_currentIndex = 0;
        }
        else if (m_currentIndex < 0)
        {
            m_currentIndex = m_maxIndex - 1;
        }
    }
}
