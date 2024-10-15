using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;

using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class EmotionBase : MonoBehaviour
{
    public string Emotion { get; set; }
    public GameObject Parent { get; set; }

    public async UniTask<GameObject> PopupEmotion()
    {
        if (!Parent) { return null; }

        var handle = Addressables.LoadAssetAsync<GameObject>(Emotion);
        await handle;

        if (!Parent) { return null; }

        var obj = Instantiate(handle.Result, Parent.transform);

        obj.OnDestroyAsObservable().Subscribe(_ =>
        {
            Addressables.Release(handle);
        });

        return obj;
    }


    public async void Start()
    {
        await UniTask.Delay(500);
        await PopupEmotion();
    }
}
