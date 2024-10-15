using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//アニメーションの任意のタイミングで武器のコライダーをアクティブにする（山本）
[System.Serializable]
public class AnimatorEventWeapon : AnimatorEvents.EventNodeBase
{
    private GameObject m_weapon = null;

  
    // 時間が来たときに実行
    public override void OnEvent(Animator animator)
    {

        m_weapon = GameObject.FindGameObjectWithTag("Weapon");
        if (m_weapon == null)
        {
            Debug.LogError("武器を装備していません");
            return;
        }



        //var charaCore = animator.transform.GetComponentInParent<CharacterCore>();


        //if (m_weapon.GetComponent<OwnerInfoTag>() == false)
        //{
        //    // 作成者情報を記憶
        //    var ownerInfo = m_weapon.AddComponent<OwnerInfoTag>();
        //    ownerInfo.GroupNo = charaCore.GroupNo;
        //    ownerInfo.Characore = charaCore;
        //}



        var boxCollider = m_weapon.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogError("BoxColliderついてないよ");
            return;
        }

        if (boxCollider.enabled == false)
        {
            boxCollider.enabled = true;
        }


    }

    public override void OnExit(Animator animator)
    {

        var boxCollider = m_weapon.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            Debug.LogError("BoxColliderついてないよ");
            return;
        }

        if (boxCollider.enabled == true)
        {
            boxCollider.enabled = false;
        }
    }


}
