using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineExtentionLayerMask : CinemachineExtension
{
    [SerializeField]
    private LayerMask _layers;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        Camera.main.cullingMask = _layers;
    }

}
