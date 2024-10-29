using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

// A behaviour that is attached to a playable
public class ContinuePlayableBehaviour : PlayableBehaviour
{

    private Image m_image;
    public Image Image { set { m_image = value; } }

    Color m_color;

    // Called when the owning graph starts playing
    public override void OnGraphStart(Playable playable)
    {
        if (!m_image)
        { return; }

        if (m_image.material)
            m_color = m_image.color;
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {
        if (!m_image)
        { return; }

        if (!m_image?.material)
        {
            return;
        }

        m_color.a = 0.0f;
        m_image.color = m_color;
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
        if (!m_image)
        { return; }

        if (!m_image?.material)
        {
            return;
        }

        if (m_image.color.a >= 0.0f)
        {
            m_color.a -= Time.deltaTime;
            m_image.color = m_color;
        }
    }
}
