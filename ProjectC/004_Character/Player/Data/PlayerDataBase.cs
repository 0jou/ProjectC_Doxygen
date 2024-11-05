using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerDataBase : ScriptableObject
{
    [SerializeField, Header("プレイヤーの各攻撃情報")]
    private List<AttackDamageData> m_attackData = new ();

    public List<AttackDamageData> AttackData { get { return m_attackData; } }
}
