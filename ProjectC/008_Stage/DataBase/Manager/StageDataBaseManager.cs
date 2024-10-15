using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using StageInfo;

public class StageDataBaseManager : BaseManager<StageDataBaseManager>
{
    // ステージ情報を保持するマネージャー
    // 制作者　田内


    [Header("ステージのデータベース")]
    [SerializeField]
    private StageDataBase m_dataBase = null;


    public StageDataBase DataBase { get { return m_dataBase; } }


    //===============================================================================
    //===============================================================================
    //===============================================================================


    #region メソッド説明
    ///--------------------------------------
    /// <summary>
    /// 引数IDのStageDataを返す
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 引数IDと一致するStageData(一致しなければnullを返す)
    /// </returns>
    /// --------------------------------------
    #endregion
    // 引数IDのIngredientDataを返す
    public StageData GetStageData(StageID _id)
    {

        // 返すデータ
        StageData data = null;


        foreach (var list in m_dataBase.StageDataBaseList)
        {


#if DEBUG
            if (!list)
            {

                // データベースがnullでないか確認する
                if (data != null)
                {
                    Debug.LogError("ステージデータが登録されていません");
                }

                continue;

            }


            if (list.StageID == _id)
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
            if (list.StageID == _id)
            {
                data = list;
                break;
            }

#endif

        }


        if (data == null)
        {
            Debug.LogError(_id + " このIDのステージは存在しません。登録できているか確認してください");
        }

        return data;

    }





}
