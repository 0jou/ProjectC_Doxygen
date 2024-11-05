using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using SelectUseItemInfo;

[DefaultExecutionOrder(-100)]
public class EnemyDataBaseManager : BaseManager<EnemyDataBaseManager>
{

    [Header("敵のデータベース")]
    [SerializeField]
    private EnemyDataBase m_dataBase = null;


    public EnemyDataBase DataBase { get { return m_dataBase; } }


    public EnemyData GetEnemyData(EnemyID _enemyID)
    {

        foreach (var list in m_dataBase.EnemyDataBaseList)
        {
            // アイテムの種類が一致
            if (list.EnemyID == _enemyID)
            {
                return list;
            }
        }

        Debug.LogError(_enemyID.ToString() + "この敵のデータベースは登録されていません");
        return null;

    }
}
