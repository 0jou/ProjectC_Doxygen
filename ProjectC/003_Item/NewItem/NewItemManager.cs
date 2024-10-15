using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewItemManager : BasePocketItemDataController
{
    // 新規獲得アイテムを保持しておくマネージャー(シングルトン)
    // 制作者　田内

    //============================================================================================
    // シングルトン

    public static NewItemManager instance;

    protected virtual void Awake()
    {
        // インスタンスがなければ作成
        if (instance == null)
        {
            instance = (NewItemManager)FindObjectOfType(typeof(NewItemManager));

            DontDestroyOnLoad(gameObject);
        }
        // あれば作成しない
        else
        {
            Destroy(gameObject);
        }
    }

    public void DeleteInstance()
    {
        if (instance != null)
        {
            Destroy(instance);
            instance = null;
        }
    }


}
