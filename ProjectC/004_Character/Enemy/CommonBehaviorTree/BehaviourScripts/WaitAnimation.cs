/*!
 * @file WaitAnimation.cs
 * @brief 指定のアニメーションになるまで待機して次のステートに遷移するクラス
 * @author 伊波
 */

using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
    using Arbor.Utilities;

    // 指定のアニメーションになるまで待機する(伊波)

    [AddComponentMenu("")]
    [AddBehaviourMenu("WaitAnimation")]
    [BuiltInBehaviour]
    public sealed class WaitAnimation : StateBehaviour
    {
        #region Serialize fields
        [SerializeField]
        [SlotType(typeof(Animator))]
        private FlexibleComponent m_animator = new FlexibleComponent(FlexibleHierarchyType.RootGraph);

        [SerializeField]
        private FlexibleString m_layerName = new FlexibleString(string.Empty);

        [SerializeField]
        private FlexibleString m_stateName = new FlexibleString(string.Empty);

        [SerializeField]
        [FormerlySerializedAs("nextState")]
        private StateLink _NextState = new StateLink();

        #endregion

        //! @brief 指定のアニメーションになったら次のステートに遷移
        void CheckTransition()
        {
            Animator animator = m_animator.value as Animator;
            if (animator == null)
            {
                return;
            }

            string layerName = m_layerName.value;
            string stateName = m_stateName.value;

            int layerIndex = AnimatorUtility.GetLayerIndex(animator, layerName);
            if (layerIndex < 0)
            {
                Debug.LogError("指定のレイヤーが見つかりません。" + gameObject.name);
                return;
            }

            if (animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName))
            {
                if (!animator.IsInTransition(layerIndex))
                {
                    Transition(_NextState);
                }
            }
        }
        public override void OnStateBegin()
        {
            CheckTransition();
        }

        public override void OnStateUpdate()
        {
            CheckTransition();
        }
    }
}
