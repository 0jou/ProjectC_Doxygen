using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConditionInfo;

public class ConditionDataBaseManager : BaseManager<ConditionDataBaseManager>
{
    // 制作者(田内)

    [Header("状態のデータベース")]
    [SerializeField]
    private ConditionDataBase m_dataBase = null;


    public ConditionDataBase DataBase { get { return m_dataBase; } }


    //===============================================================================
    //===============================================================================
    //===============================================================================


    #region メソッド説明
    ///--------------------------------------
    /// <summary>
    /// 引数IDのConditionDataを返す
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 引数IDと一致するConditionData(一致しなければnullを返す)
    /// </returns>
    /// --------------------------------------
    #endregion
    // 引数IDのIngredientDataを返す
    public ConditionData GetConditionData(ConditionID _id)
    {

        // 返すデータ
        ConditionData data = null;


        foreach (var list in m_dataBase.ConditionDataBaseList)
        {


#if DEBUG
            if (!list)
            {

                // データベースがnullでないか確認する
                if (data != null)
                {
                    Debug.LogError("状態データが登録されていません");
                }

                continue;

            }


            if (list.ConditionID == _id)
            {

                // デバッグ時はIDが被っていないか確認する
                if (data != null)
                {
                    Debug.LogError("IDが2つ存在します。確認してください");
                }

                data = list;

            }


#else


            if (!list)
            {

                continue;

            }

            // 一致すれば
            if (list.ConditionID == _id)
            {
                data = list;
                break;
            }

#endif

        }


        if (data == null)
        {
            Debug.LogError(_id + " このIDの状態は存在しません。登録できているか確認してください");
        }

        return data;

    }


}
