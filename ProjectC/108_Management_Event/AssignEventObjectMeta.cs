using System.Collections;
using System.Collections.Generic;
using UniRx;
/*
 * @file AssignEventObjectMeta.cs
 * @brief イベントオブジェクトを管理するためのMetaAI
 */

using UniRx.Triggers;
using UnityEngine;


public class AssignEventObjectMeta : MonoBehaviour, IMetaAI<BaseAssignEventObject>
{
    private void Awake()
    {
        if (IMetaAI<BaseAssignEventObject>.Instance == null)
        {
            IMetaAI<BaseAssignEventObject>.Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    HashSet<BaseAssignEventObject> _eventList = new();
    public HashSet<BaseAssignEventObject> ObjectList => _eventList;

    public void RegisterObject(BaseAssignEventObject item)
    {
        if (item == null) { return; }
        if (_eventList.Add(item))
        {
            item.OnDestroyAsObservable()
                .Subscribe(_ =>
                {
                    _eventList.Remove(item);
                }).AddTo(this);
        }
    }
}
