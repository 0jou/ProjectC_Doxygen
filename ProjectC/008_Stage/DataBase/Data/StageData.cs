using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StageInfo;

[CreateAssetMenu]
[System.Serializable]
public class StageData : ScriptableObject
{

    // 制作者　田内

    //=============================================================

    [Header("ステージID")]
    [SerializeField]
    private StageID m_stageID = StageID.Stage01;

    public StageID StageID { get { return m_stageID; } }

    //=============================================================

    [Header("移動したいシーン名")]
    [SerializeField]
    private string m_sceneName = "GameScene";

    public string SceneName { get { return m_sceneName; } }

    //=============================================================

    [Header("ステージ名")]
    [SerializeField]
    private string m_stageNameText = "None";

    public string StageNameText { get { return m_stageNameText; } }


    //=============================================================

    [Header("ステージ説明文")]
    [SerializeField]
    private string m_stageDescriptionText = "None";

    public string StageDescriptionText { get { return m_stageDescriptionText; } }

    //=============================================================

    [Header("ステージイメージ画像")]
    [SerializeField]
    private Sprite m_stageSprite = null;

    public Sprite StageSprite { get { return m_stageSprite; } }
}
