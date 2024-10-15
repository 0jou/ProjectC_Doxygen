using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;

[AddComponentMenu("")]
public class WarpPlayerBehind : StateBehaviour
{

    //プレイヤーの背後に童話キャラをワープする処理（山本）

    [SerializeField]
    private FlexibleTransform m_playerTransform = new FlexibleTransform();
    [SerializeField]
    private FlexibleFloat m_playerPosDist = new FlexibleFloat();


    private Vector3 m_playerPosition = new Vector3();
    private Vector3 m_warpPosition = new Vector3();

    // Use this for initialization
    void Start()
    {

    }

    // Use this for awake state
    public override void OnStateAwake()
    {
        base.OnStateAwake();
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
        base.OnStateBegin();
        m_playerPosition = m_playerTransform.value.position;
        m_warpPosition = m_playerPosition - m_playerTransform.value.forward * m_playerPosDist.value;

        transform.position = m_warpPosition;

    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
        base.OnStateUpdate();
    }

   
}
