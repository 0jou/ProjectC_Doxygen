using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[CreateAssetMenu(fileName = "InputActionButtonData", menuName = "ScriptableObjects/作成 InputActionButtonData")]
public class InputActionButtonData :ScriptableObject
{
    // 制作者 田内
    // InputActionのボタン情報

    [Header("ButtonName")]
    [SerializeField]
    private string m_buttonName = "";

    public string ButtonName
    {
        get { return m_buttonName; }
    }


    [Header("ボタン画像")]
    [SerializeField]
    private Sprite m_buttonSprite = null;

    public Sprite ButtonSprite
    {
        get { return m_buttonSprite; }
    }
}
