using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//AnimatorEventから任意のタイミングでカメラを揺らす（山本）
[System.Serializable]
public class AnimatorEventTriggerCameraShake : AnimatorEvents.EventNodeBase
{
    public override void OnEvent(Animator animator)
    {
       if(animator.transform.root.TryGetComponent(out CinemachineImpulseSource source))
        {
            //振動させる
            source.GenerateImpulse();
        }

    }

}
