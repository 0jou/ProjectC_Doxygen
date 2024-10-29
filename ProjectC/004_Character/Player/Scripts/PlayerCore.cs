using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤーの行動全般(倉田)
public class PlayerCore : MonoBehaviour
{
    // グループナンバー
    [SerializeField] int m_groupNo;

    

 



    // プレイヤーのアクションステートのベース(倉田)
    public abstract class PlayerActionState_Base : AnimatorStateMachine.ActionStateBase
    {
        protected PlayerCore Core { get; private set; }

        public override void Initialize(AnimatorStateMachine stateMachine)
        {
            base.Initialize(stateMachine);

            Core = stateMachine.GetComponent<PlayerCore>();
        }
    }

  

}
