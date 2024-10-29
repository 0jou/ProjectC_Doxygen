using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimatorStateMachine : MonoBehaviour
{
    private ActionStateBase m_nowState;

    public void ChangeState(ActionStateBase _state)
    {
        // 現在のStateのExitを実行
        m_nowState?.OnExit();

        // State入れ替え
        m_nowState = _state;

        // 新しいStateのEnterを実行
        m_nowState?.OnEnter();
    }


    private void Update()
    {
        m_nowState?.OnUpdate();
    }

    private void FixedUpdate()
    {
        m_nowState?.OnFixedUpdate();
    }

    [System.Serializable]
    public abstract class ActionStateBase
    {
        [SerializeField]private string m_stateName;


        public AnimatorStateMachine StateMachine { get; private set; }


        public virtual void Initialize(AnimatorStateMachine _stateMachine)
        {
            StateMachine = _stateMachine;
        }

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }

    }

}
