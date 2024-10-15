using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;
using ButtonInfo;

public class SelectProvideFoodMenuUI : BaseProvideFoodUI
{

    // 制作者　田内
    // 提供する料理の提供数を選択するUI

    [Header("提供料理スロット作成")]
    [SerializeField]
    private CreateProvideFoodSlotList m_createProvideFoodSlotList = null;

    [Header("選択中の提供料理スロット")]
    [SerializeField]
    private SelectProvideFoodSlotData m_selectProvideFoodSlotData = null;

    [Header("スクロール")]
    [SerializeField]
    private ChangeScrollViewPosition m_changeScrollViewPosition = null;

    public override async UniTask OnInitialize()
    {
        #region nullチェック
        if (m_createProvideFoodSlotList == null)
        {
            Debug.LogError("提供料理作成スクリプトがシリアライズされていません");
            return;
        }
        #endregion

        m_createProvideFoodSlotList.OnInitialize();

        await UniTask.CompletedTask;
    }

    public override async UniTask OnUpdate()
    {
        #region nullチェック

        if (m_selectUIController == null)
        {
            Debug.LogError("選択コントローラーがシリアライズされていません");
            return;
        }
        if (m_changeScrollViewPosition == null)
        {
            Debug.LogError("ChangeScrollViewPosコンポーネントがアタッチされていません");
            return;
        }
        if (m_selectProvideFoodSlotData == null)
        {
            Debug.LogError("m_selectProvideFoodSlotDataがシリアライズされていません");
            return;
        }
        #endregion

        // 存在しないUIを取り除く
        m_selectUIController.NullCheck();

        // スクロールビュー更新
        m_changeScrollViewPosition.OnUpdate();

        // 提供料理スロット作成
        CreateSlot();

        // 選択中料理表示スロット更新
        m_selectProvideFoodSlotData.OnUpdate();

        await UniTask.CompletedTask;
    }


    public override async UniTask OnSelectInitialize()
    {
        #region nullチェック
        if (m_selectProvideFoodSlotData == null)
        {
            Debug.LogError("m_selectProvideFoodSlotDataがシリアライズされていません");
            return;
        }
        #endregion

        // 選択中料理表示スロットのアクティブを停止
        m_selectProvideFoodSlotData.gameObject.SetActive(false);

        await UniTask.CompletedTask;
    }

    public override async UniTask OnSelectUpdate()
    {
        #region nullチェック

        if (m_selectUIController == null)
        {
            Debug.LogError("選択コントローラーがシリアライズされていません");
            return;
        }
        #endregion

        // UIの選択更新
        m_selectUIController.OnUpdate();

        await UniTask.CompletedTask;
    }

    public override async UniTask OnSelectExit()
    {
        #region nullチェック
        if (m_selectProvideFoodSlotData == null)
        {
            Debug.LogError("m_selectProvideFoodSlotDataがシリアライズされていません");
            return;
        }
        #endregion

        // 選択中料理表示スロットのアクティブを開始
        m_selectProvideFoodSlotData.gameObject.SetActive(true);

        await UniTask.CompletedTask;
    }


    private void CreateSlot()
    {
        #region nullチェック
        if (m_createProvideFoodSlotList == null)
        {
            Debug.LogError("提供料理作成スクリプトがシリアライズされていません");
            return;
        }
        #endregion

        // 変更が加えられていなければ
        if (ManagementProvideFoodManager.instance.IsListChange == false) return;

        // スロットを作成する
        m_createProvideFoodSlotList.CreateSlot();

    }

}
