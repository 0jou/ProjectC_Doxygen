using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[System.Serializable]
[VolumeComponentMenu("Map")]
public class MapPostProcessVolume : VolumeComponent
{
    //Defalt値はその処理が働かない値にした方がよい（チェック外れたときにDefalt値を返すから）
    public ClampedFloatParameter ContrastPower = new ClampedFloatParameter(5, 0, 20);

    public ClampedFloatParameter HueShift = new ClampedFloatParameter(0, -1, 1);
    public ClampedFloatParameter Saturation = new ClampedFloatParameter(1, 0, 2);
    public ClampedFloatParameter Value = new ClampedFloatParameter(0, -1, 1);

    public BoolParameter ToonAble = new BoolParameter(false);

}
