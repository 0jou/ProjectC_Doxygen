using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class StopEffect : MonoBehaviour
{
    //エフェクトを止めるための処理

    [Header("何秒後にエフェクト止めるか(童話スキルの滞在時間)")]
    [SerializeField]
    private float m_waitTime = 5.0f;
    public float WaitTime { get { return m_waitTime; } set { m_waitTime = value; } }

    [Header("どのエフェクトを止めるのか")]
    [SerializeField]
    private VisualEffect m_effect;


    async void Start()
    {
        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            //指定時間待機
            await UniTask.Delay(TimeSpan.FromSeconds(m_waitTime));
            cancelToken.ThrowIfCancellationRequested();

            if (m_effect)
            {
                //エフェクト止める
                m_effect.Stop();

                //エフェクトが完全に消失するまで待機する
                await UniTask.WaitUntil(() => m_effect.HasAnySystemAwake() == false);
                cancelToken.ThrowIfCancellationRequested();

                //オブジェクト破壊
                Destroy(gameObject);
            }
        }
        catch(System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }


    }





}
