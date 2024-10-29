using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;

using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class PopupEmotion : MonoBehaviour
{
    [SerializeField] float m_destoryTime = 3;

    //テスト用
    //public async void Start()
    //{
    //    await UniTask.Delay(1000);
    //    Popup("Emotion_Anger");
    //}

    public async void Popup(string objectKey)
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            var handle = Addressables.LoadAssetAsync<GameObject>("Emotion_Balloon");
            await handle;
            cancelToken.ThrowIfCancellationRequested();

            var obj = Instantiate(handle.Result, this.gameObject.transform);

            obj.AddComponent<EmotionBase>().Emotion = objectKey;
            obj.GetComponent<EmotionBase>().Parent = obj;

            obj.OnDestroyAsObservable().Subscribe(_ =>
            {
                Addressables.Release(handle);
            });

            Destroy(obj, m_destoryTime);
        }
        catch(System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }
    }
}
