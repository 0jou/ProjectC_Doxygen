/**
* @file EnemyDataBase.cs
* @brief 敵のステータス管理
*/
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/**
* @brief 敵のステータス管理 いは
*/
[CreateAssetMenu]
public class EnemyDataBase : ScriptableObject
{
    [Header("敵のデータリスト")]
    [SerializeField, EnumIndex(typeof(EnemyID))]
    private List<EnemyData> m_enemyDataBaseList = new List<EnemyData>();

    public List<EnemyData> EnemyDataBaseList { get { return m_enemyDataBaseList; } }
}
