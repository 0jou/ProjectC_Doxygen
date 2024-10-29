using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTriggerInAnimatorList : MonoBehaviour
{
    //リスト登録したAnimatorに一斉にTriggerをセットするコンポーネント（山本）

    [Header("Animatorリスト")]
    [SerializeField]
    private Animator[] m_animators;

    public void SetTriggerAnimatorInList(string _trigerName)
    {
        foreach (var animator in m_animators)
        {
            animator.SetTrigger(_trigerName);
        }
    }

}
