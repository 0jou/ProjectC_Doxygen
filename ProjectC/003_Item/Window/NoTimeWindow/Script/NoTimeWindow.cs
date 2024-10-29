using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoTimeWindow : JudgeWindow
{
    [SerializeField]
    SceneTransitionManager m_sceneTransitionManager;

    override protected async UniTask UpdateJudge()
    {
        
        if (m_judgeFlg)
        {

            //元の世界に帰る
           _= m_sceneTransitionManager.SceneChange();

        }
        else
        {
            // TODO:敵リセット処理　伊波
            // 展示会終了後削除

            CharacterMeta meta = IMetaAI<CharacterCore>.Instance as CharacterMeta;
            if (!meta) return;
            foreach (CharacterCore chara in meta.ObjectList)
            {
                chara.EnemyResetPos();
            }


        }

        await UniTask.CompletedTask;

    }
}
