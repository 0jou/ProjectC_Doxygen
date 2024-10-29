using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : BasePocketItemDataController
{
    // 倉庫管理マネージャークラス(シングルトン)
    // 制作者　田内

    //============================================================================================
    // シングルトン

    #region シングルトン

    public static StorageManager instance;

    protected virtual void Awake()
    {
        // インスタンスがなければ作成
        if (instance == null)
        {
            instance = (StorageManager)FindObjectOfType(typeof(StorageManager));

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
