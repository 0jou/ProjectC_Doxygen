using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SetPositionCharacterController : MonoBehaviour
{
    [Header("CharacterCoreをセットする")]
    [SerializeField]
    private MyCharacterController[] m_myCharacterController;


    [Header("Object生成位置からどのくらいの高さにCharacterCoreを設置するか")]
    [SerializeField]
    private float m_hight = 5.0f;


    public void SetCharacterCore()
    {
        if (m_myCharacterController.Length == 0)
        {
            return;
        }

        foreach (var characterCore in m_myCharacterController)
        {
            characterCore.SetPositionMotor(transform.position + new Vector3(0, m_hight, 0));
        }


    }

}
