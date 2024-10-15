using Cysharp.Threading.Tasks;
using DG.Tweening;
using KinematicCharacterController;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.TextCore.Text;
using UnityEngine.Timeline;

public partial class CharacterCore : MonoBehaviour
{
    //客のCharacterCore(山本)

    //待機&移動
    [System.Serializable]
    public class ActionState_CustomerIdleAndMove : CharacterCore.ActionState_Base
    {

    }

    //椅子に座っている
    [System.Serializable]
    public class ActionState_CustomerSitSheat : CharacterCore.ActionState_Base
    {

    }

    //食事
    [System.Serializable]
    public class ActionState_CustomerEat : CharacterCore.ActionState_Base
    {

    }

}
