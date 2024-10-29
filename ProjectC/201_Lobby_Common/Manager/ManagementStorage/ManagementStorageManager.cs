using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagementStorageManager :BasePocketItemDataController
{
    // 制作者 田内
    // 経営用ストレージ、このストレージから提供する料理を決める
    

    #region シングルトン
    public static ManagementStorageManager instance;

    protected virtual void Awake()
    {
        // インスタンスがなければ作成
        if (instance == null)
        {
            instance = (ManagementStorageManager)FindObjectOfType(typeof(ManagementStorageManager));

            DontDestroyOnLoad(gameObject);
        }
        // あれば作成しない
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

}
