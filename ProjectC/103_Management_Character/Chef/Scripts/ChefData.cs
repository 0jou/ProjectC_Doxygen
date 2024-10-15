using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ChefStateInfo;

namespace ChefStateInfo
{
    public enum ChefState
    {

        Default = 0,    // 通常
        Cooking = 1,    // 調理中


    }
}

public class ChefData : MonoBehaviour
{
    // シェフデータ
    // 制作者 田内


    private ChefState m_currentChefState = ChefState.Default;

    public ChefState CurrentChefState
    {
        get { return m_currentChefState; }
        set { m_currentChefState = value; }
    }

}
