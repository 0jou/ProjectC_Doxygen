using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class SetRigInfo : MonoBehaviour
{
    //Rig情報をセットするコンポーネント（山本）
    //CharcterCoreからアクセスしやすくするため
    //Targetコーポネントの位置をターゲット先に変更することで、向きを変える
    [SerializeField] Rig m_useRig;
    [Header("RigのターゲットコンポーネントのTransform")]
    [SerializeField] private Transform m_targetTransform;


    public Transform TargetTransform
    {
        set { m_targetTransform = value; }
        get { return m_targetTransform; }
    }

    
    public Rig UseRig
    {
        set { m_useRig = value; }
        get { return m_useRig; }
    }

}
