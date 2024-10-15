using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class StartPlayerPlayableBehaviour : PlayableBehaviour
{
    public GameObject charaObj = null;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        if (charaObj == null)
        {
            return;
        }

        if (charaObj.TryGetComponent(out CharacterCore characterCore))
        {
            //characterCore.SetRotateToTarget(characterCore.transform.forward);
        }
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {

    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {

    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {

    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        if(charaObj==null)
        {
            return;
        }

        if(charaObj.TryGetComponent(out CharacterCore characterCore))
        {
            characterCore.Move(2.0f);
        }

    }
}
