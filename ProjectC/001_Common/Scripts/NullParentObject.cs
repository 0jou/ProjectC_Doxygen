using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullParentObject : MonoBehaviour
{
    //親子関係を解除するスクリプト（山本）
    public void NullParent()
    {
        if (transform.root.gameObject)
        {
            transform.position = transform.root.position + new Vector3(0,0.25f,0);

            transform.parent = null;

        }

    }

}
