using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NaughtyAttributes;

public class StorehouseManager : BasePocketItemDataController
{
    // 倉庫管理マネージャークラス(シングルトン)
    // 制作者　田内

    //============================================================================================
    // シングルトン

    public static StorehouseManager instance;

    protected virtual void Awake()
    {
        // インスタンスがなければ作成
        if (instance == null)
        {
            instance = (StorehouseManager)FindObjectOfType(typeof(StorehouseManager));

            DontDestroyOnLoad(gameObject);
        }
        // あれば作成しない
        else
        {
            Destroy(gameObject);
        }
    }

}
