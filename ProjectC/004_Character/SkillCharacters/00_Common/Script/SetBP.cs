using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBP : MonoBehaviour
{
    //スキル使用時のBPを決めるクラス
    [SerializeField]
    private float m_skillPayBP = 0.0f;

    public float SkillPayBP { get { return m_skillPayBP; } }


}
