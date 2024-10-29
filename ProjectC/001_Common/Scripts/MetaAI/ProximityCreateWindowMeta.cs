using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class ProximityCreateWindowMeta : MonoBehaviour, IMetaAI<ProximityCreateWindow>
{
    private void Awake()
    {
        // インスタンスがなければ作成
        if (IMetaAI<ProximityCreateWindow>.Instance == null)
        {
            IMetaAI<ProximityCreateWindow>.Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // あれば作成しない
        else
        {
            Destroy(gameObject);
        }
    }


    HashSet<ProximityCreateWindow> proximityCretaeWindowList = new();

    public HashSet<ProximityCreateWindow> ObjectList => proximityCretaeWindowList;

    public void RegisterObject(ProximityCreateWindow _proximityCretaeWindow)
    {
        if (_proximityCretaeWindow == null) { return; }
        if (proximityCretaeWindowList.Add(_proximityCretaeWindow))
        {

            _proximityCretaeWindow.OnDestroyAsObservable().Subscribe(_ =>
                {
                    proximityCretaeWindowList.Remove(_proximityCretaeWindow);
                }).AddTo(this);
        }
    }
}
