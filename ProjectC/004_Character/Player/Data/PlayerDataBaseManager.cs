using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using SelectUseItemInfo;

[DefaultExecutionOrder(-100)]
public class PlayerDataBaseManager : BaseManager<PlayerDataBaseManager>
{

    [Header("プレイヤーのデータベース")]
    [SerializeField]
    private PlayerDataBase m_dataBase = null;


    public PlayerDataBase DataBase { get { return m_dataBase; } }
}