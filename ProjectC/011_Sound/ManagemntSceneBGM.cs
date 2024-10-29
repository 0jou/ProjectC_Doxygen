using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagemntSceneBGM : MonoBehaviour
{
    //経営パートのBGMを流す処理（山本）
    
    void Start()
    {
        SoundManager.Instance.StartBGM("ManagementBGM");
    }

   
}
