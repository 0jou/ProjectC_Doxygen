using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckChildrenNum : MonoBehaviour
{
    //子オブジェクトが存在しなければ自分自身を破壊する（山本）
    private void Update()
    {
       if( gameObject.transform.childCount == 0)
        {
            Destroy(gameObject );
        }

    }
}
