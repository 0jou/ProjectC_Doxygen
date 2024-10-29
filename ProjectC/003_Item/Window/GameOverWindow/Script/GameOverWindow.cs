using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverWindow : JudgeWindow
{
    // シーン遷移を行う
    // 遷移できればtrueを返す
    override protected async UniTask UpdateJudge()
    {
      
        if (m_judgeFlg)
        {

            // Todo:ここでシーン遷移
            //経営シーンへ移行する

            int a = 0;


            // TODO:敵リセット処理　伊波
            // 展示会終了後削除

            CharacterMeta meta = IMetaAI<CharacterCore>.Instance as CharacterMeta;
            if (!meta) return;
            foreach(CharacterCore chara in meta.ObjectList)
            {
                chara.EnemyResetPos();
            }

        }
        else
        {
           
        }

        await UniTask.CompletedTask;

    }
}
