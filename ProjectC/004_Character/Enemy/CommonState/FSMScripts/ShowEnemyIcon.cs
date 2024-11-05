using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using Cysharp.Threading.Tasks;
using static EnemyIconController;

// 敵のアイコン表示（伊波）

[AddComponentMenu("")]
[AddBehaviourMenu("Enemy/ShowEnemyIcon")]
public class ShowEnemyIcon : StateBehaviour
{
    public StateLink m_successLink;

    [SerializeField]
    private EnemyIconType m_showIconType;

    [SerializeField]
    private FlexibleFloat m_iconFillTime;
    
    private EnemyIconController m_iconController;

    public override void OnStateAwake()
    {
        base.OnStateAwake();
        m_iconController = transform.root.GetComponentInChildren<EnemyIconController>();
        if (!m_iconController)
        {
            Debug.LogError("EnemyIconControllerが見つかりませんでした。" + gameObject.name);
        }
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
        if (m_iconController)
        {
            m_iconController.ShowIcon(m_showIconType, m_iconFillTime.value);
        }
        //await UniTask.Delay(TimeSpan.FromSeconds(m_iconShowTime));
        //if (iconController)
        //{
        //    iconController.HideIcon(EnemyIconController.EnemyIconType.DiscoverIcon);
        //}

        Transition(m_successLink);
    }

    //public override void OnStateEnd()
    //{
    //}

    //public override void OnStateUpdate()
    //{
    //}

    //public override void OnStateLateUpdate()
    //{
    //}
}
