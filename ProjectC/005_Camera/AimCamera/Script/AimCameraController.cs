using Arbor;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;


public class AimCameraController : MonoBehaviour
{

    private IInputProvider m_inputProvider = null;
    [SerializeField] private CinemachineInputProvider m_cinemachineInputProvider = null;

    [SerializeField]
    private Transform m_verticalTarget = null;

    [SerializeField]
    private Transform m_horizontalTarget = null;

    [SerializeField]
    private AxisState m_vertical;
    public AxisState Vertical { get { return m_vertical; } }
    [SerializeField]
    private AxisState m_horizontal;
    public AxisState Horizontal { get { return m_horizontal; } }

    void Awake()
    {
        m_inputProvider = new PlayerInputProvider();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVec =new(
            m_cinemachineInputProvider.GetAxisValue(0) * -1.0f,// リバースしているので-1をかける
            m_cinemachineInputProvider.GetAxisValue(1));
        //Vector2 inputVec = m_inputProvider.ThrowAim;
        m_vertical.m_InputAxisValue = inputVec.y;
        m_horizontal.m_InputAxisValue = inputVec.x;

        m_vertical.Update(Time.deltaTime);
        m_horizontal.Update(Time.deltaTime);

        var horizontalRotation = Quaternion.AngleAxis(m_horizontal.Value, Vector3.up);
        var verticalRotation = Quaternion.AngleAxis(m_vertical.Value, Vector3.right);
        m_horizontalTarget.rotation = horizontalRotation;
        m_verticalTarget.localRotation = verticalRotation;

    }

    private void OnDisable()
    {
        m_verticalTarget.localRotation = Quaternion.identity;
        m_horizontalTarget.rotation = Quaternion.identity;
        m_vertical.Value = 0;
        m_horizontal.Value = 0;
    }

    private void OnEnable()
    {
        Vector3 angle = Camera.main.transform.localRotation.eulerAngles;
        m_horizontalTarget.eulerAngles = new Vector3(0,angle.y,0);

        Vector3 horizonVec = m_horizontalTarget.rotation.eulerAngles;
        m_horizontal.Value = horizonVec.y;
    }
}
