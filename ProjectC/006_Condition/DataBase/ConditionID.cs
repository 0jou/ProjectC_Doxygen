using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConditionInfo
{
    // キャラクターの状態や料理の付与する状態などを示す列挙型
    public enum ConditionID
    {
        Normal      = -1,   // 通常の状態
        Paralysis   = 0,    // 麻痺状態
        Poison      = 1,    // 毒状態
        Sleep       = 2,    // 眠り状態
        Fire        = 3,    // 火炎放射
        Confusion   = 4,    // 混乱状態S
        Stun        = 5,    // スタン状態

        ConditionTypeNum,
    }

    public enum ResistanceID
    {
        Disadvantage,
        Normal,
        Advantage,

        ResistanceTypeNum
    }
}
