using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ConditionInfo
{
    // キャラクターの状態や料理の付与する状態などを示す列挙型
    public enum ConditionID
    {
        Normal      = 0,   // 通常の状態
        Paralysis   = 1,    // 麻痺状態
        Poison      = 2,    // 毒状態
        Sleep       = 3,    // 眠り状態
        Fire        = 4,    // 火炎放射
        Confusion   = 5,    // 混乱状態S
        Stun        = 6,    // スタン状態

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
