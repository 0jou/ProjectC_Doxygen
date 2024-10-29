using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

// 設置アイテムを監視できるMetaAI（伊波）

public class PutItemMeta : MonoBehaviour, IMetaAI<AssignItemID>
{
    private void Awake()
    {
        if (IMetaAI<AssignItemID>.Instance == null)
        {
            IMetaAI<AssignItemID>.Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    HashSet<AssignItemID> _putItemList = new();
    public HashSet<AssignItemID> ObjectList => _putItemList;

    public void RegisterObject(AssignItemID item)
    {
        if (item == null) { return; }
        if (_putItemList.Add(item))
        {
            item.OnDestroyAsObservable()
                .Subscribe(_ =>
                {
                    _putItemList.Remove(item);
                }).AddTo(this);
        }
    }
}
