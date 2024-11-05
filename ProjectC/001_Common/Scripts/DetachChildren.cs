using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-200)]
//　自身の子オブジェクトとの関係を解除するコンポーネント（山本）
public class DetachChildren : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        //　子オブジェクトをすべて解除
        transform.DetachChildren();
        // 自分自身は不要なので削除
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
