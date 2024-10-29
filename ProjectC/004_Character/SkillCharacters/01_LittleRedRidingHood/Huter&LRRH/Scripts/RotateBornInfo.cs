using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBornInfo : MonoBehaviour
{
    //回転するボーン情報を登録するコンポーネント（敵に合わせて腰や頭を回転させる）(山本)
    [SerializeField] Transform m_rotateBone;

    public void RotateBone(Vector3 targetPos)
    {
        if (m_rotateBone == null) return;

        m_rotateBone.transform.LookAt(targetPos);

    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //m_rotateBone.LookAt(new Vector3(2,1,10));
    }
}
