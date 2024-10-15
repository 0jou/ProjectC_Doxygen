using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[RequireComponent(typeof(PlayableDirector))]
[System.Serializable]
public class AnimatorEventTimeLine : AnimatorEvents.EventNodeBase
{
    //実行するタイムラインをセット
    [SerializeField] private TimelineAsset timeline;
    // 時間が来たときに実行
    public override void OnEvent(Animator animator)
    {
        if(timeline==null)
        {
            return;
        }

        //if (animator.transform.root.TryGetComponent(out ShareNodes nodes))
        //{
        //    if (nodes.Nodes.TryGetValue(m_parent, out Transform parent))
        //    {
        //        m_createdInstance = GameObject.Instantiate(m_object, parent);
        //    }
        //    else
        //    {
        //        Debug.LogError("攻撃当たり判定の親オブジェクトが見つかりませんでした。" +
        //            "登録名が間違っているか、ShareNodeに追加されていません。");
        //    }
        //}
        //else Debug.LogError("ShareNodesコンポーネントが見つかりませんでした");


        if (animator.transform.TryGetComponent(out PlayableDirector director))
        {
            //指定したタイムラインを再生
            director.Play(timeline);

        }
        else
        {
            Debug.LogError("PlayableDirectorコンポーネントが見つかりませんでした");
            return;
        }

    }
}
