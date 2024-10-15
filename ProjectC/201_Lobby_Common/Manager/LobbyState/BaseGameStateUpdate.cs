using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

public class BaseGameStateUpdate : MonoBehaviour
{
    // 制作者 田内
    // ゲームの進行処理の基底クラス

    //===========================================
    // ステート

    virtual public int GetState()
    {
        Debug.LogError("オーバーライドしてください");
        return 0;
    }

    //============================================
    // 次に進むステート

    virtual public int GetNextState()
    {
        Debug.LogError("オーバーライドしてください");
        return 0;
    }

    //=============================================
    // 終了したか

    private bool m_isEnd = false;

    /// <summary>
    /// 終了したか
    /// </summary>
    public bool IsEnd
    {
        get { return m_isEnd; }
    }

    //======================================================
    //              実行処理
    //======================================================


    /// <summary>
    /// 初期処理
    /// </summary>
    virtual public async UniTask OnInitialize()
    {
        await UniTask.CompletedTask;
    }


    /// <summary>
    /// 実行処理
    /// </summary>
    virtual public async UniTask OnUpdate()
    {
        await UniTask.CompletedTask;
    }


    /// <summary>
    /// 終了処理
    /// </summary>
    virtual public async UniTask OnExit()
    {
        await UniTask.CompletedTask;
    }


    /// <summary>
    /// 削除処理
    /// </summary>
    virtual public async UniTask OnDestroy()
    {
        Destroy(gameObject);
        await UniTask.CompletedTask;
    }


    /// <summary>
    /// ステート処理を終了する
    /// </summary>
    virtual protected void SetEnd()
    {
        m_isEnd = true;
    }

}
