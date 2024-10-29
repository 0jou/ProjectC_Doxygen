using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnHomeWindow : JudgeWindow
{
    //経営シーンへと変えるか選択するウィンドウ（山本）

    [Header("シーン変更")]
    [SerializeField]
    private SceneTransitionManager m_sceneTransitionManager = null;


  
    // シーン遷移を行う
    // 遷移できればtrueを返す
    override protected async UniTask UpdateJudge()
    {
        if (m_sceneTransitionManager == null)
        {
            Debug.LogError("シーン変更クラスがシリアライズされていません");
            return;
        }

        if (m_judgeFlg)
        {

            // Todo:ここでシーン遷移
            //経営シーンへ移行する

            _ = m_sceneTransitionManager.SceneChange();

        }
        else
        {
            // 無し
        }

        await UniTask.CompletedTask;

    }


}
