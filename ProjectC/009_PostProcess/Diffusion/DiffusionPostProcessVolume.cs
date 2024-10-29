using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
[VolumeComponentMenu("Diffusion")]
public class DiffusionPostProcessVolume : VolumeComponent
{
    //Defalt�l�͂��̏����������Ȃ��l�ɂ��������悢�i�`�F�b�N�O�ꂽ�Ƃ���Defalt�l��Ԃ�����j
    public ClampedFloatParameter GaussDispersion = new ClampedFloatParameter(3, 0, 10);
    public ClampedIntParameter GaussSmaplingTexelAmount = new ClampedIntParameter(9, 0, 32);
    public ClampedFloatParameter ScreenBlend = new ClampedFloatParameter(1, 0, 3);

}
