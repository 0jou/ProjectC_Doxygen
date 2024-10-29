using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConditionInfo;

public class ConditionDataBaseManager : BaseManager<ConditionDataBaseManager>
{
    // �����(�c��)

    [Header("��Ԃ̃f�[�^�x�[�X")]
    [SerializeField]
    private ConditionDataBase m_dataBase = null;


    public ConditionDataBase DataBase { get { return m_dataBase; } }


    //===============================================================================
    //===============================================================================
    //===============================================================================


    #region ���\�b�h����
    ///--------------------------------------
    /// <summary>
    /// ����ID��ConditionData��Ԃ�
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// ����ID�ƈ�v����ConditionData(��v���Ȃ����null��Ԃ�)
    /// </returns>
    /// --------------------------------------
    #endregion
    // ����ID��IngredientData��Ԃ�
    public ConditionData GetConditionData(ConditionID _id)
    {

        // �Ԃ��f�[�^
        ConditionData data = null;


        foreach (var list in m_dataBase.ConditionDataBaseList)
        {


#if DEBUG
            if (!list)
            {

                // �f�[�^�x�[�X��null�łȂ����m�F����
                if (data != null)
                {
                    Debug.LogError("��ԃf�[�^���o�^����Ă��܂���");
                }

                continue;

            }


            if (list.ConditionID == _id)
            {

                // �f�o�b�O����ID������Ă��Ȃ����m�F����
                if (data != null)
                {
                    Debug.LogError("ID��2���݂��܂��B�m�F���Ă�������");
                }

                data = list;

            }


#else


            if (!list)
            {

                continue;

            }

            // ��v�����
            if (list.ConditionID == _id)
            {
                data = list;
                break;
            }

#endif

        }


        if (data == null)
        {
            Debug.LogError(_id + " ����ID�̏�Ԃ͑��݂��܂���B�o�^�ł��Ă��邩�m�F���Ă�������");
        }

        return data;

    }


}
