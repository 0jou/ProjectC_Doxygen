using UnityEngine;

public class RotatePosition : MonoBehaviour
{

    public float m_rotateSpeed = 100.0f;
    public Vector3 m_centerPosition = new Vector3(0, 0, 1.5f);

    void Update()
    {
        /* Rotate around a center position. */
        this.gameObject.transform.RotateAround(m_centerPosition, new Vector3(0, 1, 0), Time.deltaTime * m_rotateSpeed);
    }
}