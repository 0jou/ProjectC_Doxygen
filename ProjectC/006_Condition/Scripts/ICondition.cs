using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 状態異常を処理するクラスのインターフェース（伊波）

public interface ICondition
{
    public ConditionInfo.ConditionID ConditionID { get; }

    public GameObject Owner { get; set; }

    public bool IsEffective();
    public float DamageMulti(ConditionInfo.ResistanceID[] resistances);

    public void ReplaceCondition(ICondition newCondition, ConditionInfo.ResistanceID[] resistances);
}

public class NullCondition : ICondition
{
    public static NullCondition NullInstance = new();

    public ConditionInfo.ConditionID ConditionID { get; } = new ConditionInfo.ConditionID();
    public GameObject Owner { get; set; } = null;
    public bool IsEffective() { return true; }
    public float DamageMulti(ConditionInfo.ResistanceID[] resistances) { return 1f; }
    public void ReplaceCondition(ICondition newCondition, ConditionInfo.ResistanceID[] resistances) {}
}
