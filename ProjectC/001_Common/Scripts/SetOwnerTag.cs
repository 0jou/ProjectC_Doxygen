using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetOwnerTag : MonoBehaviour
{
    private void Awake()
    {
        var charaCore = GetComponentInParent<CharacterCore>();


        // 作成者情報を記憶
        var ownerInfo = gameObject.AddComponent<OwnerInfoTag>();
        ownerInfo.GroupNo = charaCore.GroupNo;
        ownerInfo.Characore = charaCore;


    }
}
