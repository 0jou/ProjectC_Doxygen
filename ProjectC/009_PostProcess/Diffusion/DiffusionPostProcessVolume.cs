using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
[VolumeComponentMenu("Diffusion")]
public class DiffusionPostProcessVolume : VolumeComponent
{
    //Defalt値はその処理が働かない値にした方がよい（チェック外れたときにDefalt値を返すから）
    public ClampedFloatParameter GaussDispersion = new ClampedFloatParameter(3, 0, 10);
    public ClampedIntParameter GaussSmaplingTexelAmount = new ClampedIntParameter(9, 0, 32);
    public ClampedFloatParameter ScreenBlend = new ClampedFloatParameter(1, 0, 3);

}
