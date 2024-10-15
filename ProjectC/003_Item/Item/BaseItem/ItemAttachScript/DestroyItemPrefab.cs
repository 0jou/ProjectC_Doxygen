using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItemPrefab : MonoBehaviour
{
    // 田内

    [Header("自身の削除判定に使用するオブジェクト")]
    [SerializeField]
    private GameObject m_object=null;



    private void Update()
    {

        CheckDestroy();

    }


    private void CheckDestroy()
    {
        // 登録したオブジェクトがnullになれば
        if(m_object==null)
        {
            // 自身も削除する
            Destroy(gameObject);
        }
    }


}
