using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseManager<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;

    [Header("シーン変更後も削除しない")]
    [SerializeField] 
    bool m_dontDestroyOnLoad = true;

    protected virtual void Awake()
    {
        // インスタンスがなければ作成
        if (instance == null)
        {
            instance = (T)FindObjectOfType(typeof(T));
            if(m_dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        // あれば作成しない
        else
        {
            Destroy(gameObject);
        }
    }

    public void DeleteInstance()
    {
        if(instance != null)
        {
            Destroy(instance);
            instance = null;
        }
    }
}
