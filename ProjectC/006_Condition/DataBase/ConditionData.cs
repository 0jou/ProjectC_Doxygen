using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConditionInfo;

[CreateAssetMenu]
[System.Serializable]
public class ConditionData : ScriptableObject
{

    // 制作者(田内)

    //============================
    // 状態異常の処理

    [Header("状態異常の処理コンポーネントを含むプレハブ")]
    [SerializeField]
    private GameObject m_conditionPrefab;

    [Header("投げた時の状態異常の処理コンポーネントを含むプレハブ")]
    [SerializeField]
    private GameObject m_throwConditionPrefab;


    public GameObject ConditionPrefab { get { return m_conditionPrefab; } }
    public GameObject ThrowConditionPrefab { get { return m_throwConditionPrefab; } }



    //============================
    // この料理のID


    [Header("この状態のID")]
    [SerializeField]
    private ConditionID m_conditionID = ConditionID.Normal;


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// この状態のIDを確認する、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 状態のID
    /// </returns>
    /// --------------------------------------
    #endregion
    public ConditionID ConditionID { get { return m_conditionID; } }


    //============================
    // この料理の名前


    [Header("この状態の名前")]
    [SerializeField]
    private string m_conditionName = "None";


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// この状態の名前確認する、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 状態の名前
    /// </returns>
    /// --------------------------------------
    #endregion
    public string ConditionName { get { return m_conditionName; } }


    //============================
    // この料理の紹介文


    [Header("この状態の紹介文")]
    [SerializeField]
    private string m_conditionDescriptionText = "None";


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// この状態の紹介文を確認する、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 状態の紹介文
    /// </returns>
    /// --------------------------------------
    #endregion
    public string ConditionDescriptionText { get { return m_conditionDescriptionText; } }


    //============================
    // この料理の見た目


    [Header("この状態の見た目")]
    [SerializeField]
    private Sprite m_conditionSprite = null;


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// この料理の見た目確認する、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 料理の見た目
    /// </returns>
    /// --------------------------------------
    #endregion
    public Sprite ConditionSprite { get { return m_conditionSprite; } }


    //============================
    // この状態を表す色　追加（吉田）


    [Header("この状態の色")]
    [SerializeField]
    private Color m_conditionColor = new(1f, 1f, 1f, 1f);


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// この状態を表す色を確認する、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 状態を表す色
    /// </returns>
    /// --------------------------------------
    #endregion
    public Color ConditionColor { get { return m_conditionColor; } }

}
