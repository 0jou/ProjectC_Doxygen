using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

public class PopUpWindow : BaseWindow
{

    // ポップアップウィンドウ
    // 制作者　田内

    [Header("表示し続けるか")]
    [SerializeField]
    private bool m_endless = false;

    [Header("終了する時間")]
    [SerializeField]
    private float m_endTime = 10.0f;

    private float m_currentTime = 0.0f;



    public override async UniTask OnUpdate()
    {

        var cancelToken = this.GetCancellationTokenOnDestroy();

        try
        {
            // 時間が経過するまで
            while (cancelToken.IsCancellationRequested == false)
            {
                await base.OnUpdate();
                cancelToken.ThrowIfCancellationRequested();

                if (CalcTime())
                {
                    break;
                }

                await UniTask.DelayFrame(1);
                cancelToken.ThrowIfCancellationRequested();

            }
        }
        catch (System.OperationCanceledException ex)
        {
            Debug.Log(ex);
        }

    }

    private bool CalcTime()
    {
        if (m_endless) return false;

        m_currentTime += Time.deltaTime;

        if (m_endTime <= m_currentTime)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
