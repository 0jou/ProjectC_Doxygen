using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ActionUIController : MonoBehaviour
{
    public enum MainActionUIState
    {
        None = 0,
        Attack = 1,
        //HoldItem = 1 << 1,
    }

    public enum AnyActionUIState
    {
        None = 0,
        AbleGatherings = 1 << 2,
        ReadyToThrow = 1 << 3,
        AbleToCooking = 1 << 4,
        AbleToPortal = 1 << 5,
    }

    public enum ActionUIState
    {

        None = 0,

        // 好戦的状態
        Attack = 1 << 0,

        // アイテム持ち状態がなくなったのでコメントアウト（吉田）
        //// アイテム持ち状態
        //HoldItem = 1 << 1,



        // 拾える状態
        AbleGatherings = 1 << 2,

        // 狙い状態
        ReadyToThrow = 1 << 3,

        // 作る状態
        AbleToCooking = 1 << 4,

        // ポータルで状態
        AbleToPortal = 1 << 5,

    }

    // 状態を管理する
    private ReactiveProperty<ActionUIState> m_state = new ReactiveProperty<ActionUIState>(ActionUIState.None);
    public void AddState(ActionUIState _state) { m_state.Value |= _state; }
    public void RemoveState(ActionUIState _state) { m_state.Value &= ~_state; }

    // AnyActionUIの状態を管理する
    private ReactiveProperty<ActionUIState> m_anyActionState = new ReactiveProperty<ActionUIState>(ActionUIState.None);
    public void AddAnyActionState(ActionUIState _state, float _distance) { SwitchAnyAction(_state, _distance); }
    public void RemoveAnyActionState(ActionUIState _state) { if(m_anyActionState.Value == _state) SwitchAnyAction(ActionUIState.None); }
    /// <summary>
    /// 現在のアクション起点との距離
    /// </summary>
    private float m_nowAnyActionDistance = -1.0f;
    public void SwitchAnyAction(ActionUIState _state, float _distance = -1.0f)
    {
        if (_state == ActionUIState.None)
        {
            m_nowAnyActionDistance = -1.0f;
            m_anyActionState.Value = _state;
            return;
        }

        if (m_nowAnyActionDistance < 0.0f)
        {
            m_nowAnyActionDistance = _distance;
            m_anyActionState.Value = _state;
            return;
        }


        if (m_nowAnyActionDistance < _distance) return;
        m_nowAnyActionDistance = _distance;
        m_anyActionState.Value = _state;
    }



    /// <summary>
    /// AnyActionUIの表示するコントローラー
    /// </summary>
    [SerializeField]
    private AnyActionUIController m_anyActionUIController = null;


    /*

    // 生成したUIを格納するDictionary
    private Dictionary<string, GameObject> m_actionDictionary =
        new Dictionary<string, GameObject>();


    [Header("任意のタイミングで出すUIの場所")]
    [SerializeField] private Transform m_actionUIPlace;

    */


    // Start is called before the first frame update
    async void Start()
    {

        m_state.Value = ActionUIState.None;
        m_anyActionState.Value = ActionUIState.None;

        if (m_anyActionUIController == null)
        {
            Debug.LogError("m_anyActionUIControllerが設定されていません。");
        }

        /*

        if (m_actionUIPlace == null)
        {
            Debug.Log("m_actionUIPlaceが設定されていません。");
            m_actionUIPlace = transform;
        }

        List<AsyncOperationHandle<GameObject>> operationHandles = new List<AsyncOperationHandle<GameObject>>();
        //Addressableから取得
        //確実にLoadを実行するためにawait追加（山本）
        var load = LoadPrefab("ActionUIGroupPrefab_Attack", transform, operationHandles);
        await load;
        load = LoadPrefab("ActionUIGroupPrefab_HoldItem", transform, operationHandles);
        await load;
        load = LoadPrefab("ActionUIPrefab_Gathering", m_actionUIPlace, operationHandles);
        await load;
        load = LoadPrefab("ActionUIPrefab_Throw", m_actionUIPlace, operationHandles);
        await load;
        load = LoadPrefab("ActionUIPrefab_Cooking", m_actionUIPlace, operationHandles);
        await load;
        load = LoadPrefab("ActionUIPrefab_Portal", m_actionUIPlace, operationHandles);
        await load;

        AllLoadCompleted(operationHandles);

        */

        SwitchState();
    }

    private void SwitchState()
    {

        // 状態変化時の処理
        m_state.Subscribe(async state =>
        {

            /*
            
            if (state.HasFlag(ActionUIState.AbleGatherings))
            {
                await OnShowUI("ActionUI_Gathering");
                //Debug.Log("AbleGatherings");
            }
            else
            {
                await OnHideUI("ActionUI_Gathering");
            }

            if (state.HasFlag(ActionUIState.ReadyToThrow))
            {
                await OnShowUI("ActionUI_Throw");
                //Debug.Log("ReadyToThrow");
            }
            else
            {
                await OnHideUI("ActionUI_Throw");
            }

            if (state.HasFlag(ActionUIState.AbleToCooking))
            {
                await OnShowUI("ActionUI_Cooking");
                //Debug.Log("AbleToCooking");
            }
            else
            {
                await OnHideUI("ActionUI_Cooking");
            }

            if (state.HasFlag(ActionUIState.AbleToPortal))
            {
                await OnShowUI("ActionUI_Portal");
                //Debug.Log("AbleToPortal");
            }
            else
            {
                await OnHideUI("ActionUI_Portal");
            }

            */

            // 常に表示することになったので一旦コメント（吉田）
            //// 右下のUI
            //// 消す処理を先に書く
            //if (!state.HasFlag(ActionUIState.Attack))
            //{
            //    await OnHideUI("ActionUIGroup_Attack");
            //}
            //else if (!state.HasFlag(ActionUIState.HoldItem))
            //{
            //    await OnHideUI("ActionUIGroup_HoldItem");
            //}
            //// 表示処理
            //if (state.HasFlag(ActionUIState.HoldItem))
            //{
            //    await OnShowUI("ActionUIGroup_HoldItem");
            //}
            //else if (state.HasFlag(ActionUIState.Attack))
            //{
            //    await OnShowUI("ActionUIGroup_Attack");
            //}

        });

        m_anyActionState.Subscribe(_state =>
        {
            // AnyActionUIの表示を変更
            if (m_anyActionUIController)
            {
                m_anyActionUIController.ChangeStateUI(_state);
            }
        });
    }

    /*

    private async UniTask OnShowUI(string _name)
    {
        //中に入ってなけらば早期リターン（山本）
        if (m_actionDictionary.Count == 0)
        {
            return;
        }

        if (m_actionDictionary.ContainsKey(_name))
        {
            if (m_actionDictionary[_name].activeInHierarchy) return;

            m_actionDictionary[_name].SetActive(true);
            if (m_actionDictionary[_name].TryGetComponent<ActionUIBaseController>(out ActionUIBaseController controller))
            {
                await controller.OnShow();
            }
        }
        else
        {
            Debug.LogError(_name + "が登録されてないです。");
        }

    }
    private async UniTask OnHideUI(string _name)
    {
        //中に入ってなけらば早期リターン（山本）
        if (m_actionDictionary.Count == 0)
        {
            return;
        }

        if (m_actionDictionary.ContainsKey(_name))
        {
            if (!m_actionDictionary[_name].activeInHierarchy) return;

            if (m_actionDictionary[_name].TryGetComponent<ActionUIBaseController>(out ActionUIBaseController controller))
            {
                await controller.OnClose();
            }
            m_actionDictionary[_name].SetActive(false);
        }
        else
        {
            Debug.LogError(_name + "が登録されてないです。");
        }

    }

    private async Task<AsyncOperationHandle<GameObject>> LoadPrefab(
        string assetPath, Transform _parent,
        List<AsyncOperationHandle<GameObject>> handles = null)// 読み込み調べるためのリスト
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(assetPath);
        //handle.Completed += (op) => OnGameObjectLoaded(op, _parent);

        await handle.Task;

        OnGameObjectLoaded(handle, _parent);

        if (handles != null)
        {
            handles.Add(handle);
        }
        return handle;
    }

    private void OnGameObjectLoaded(AsyncOperationHandle<GameObject> handle, Transform _parent)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            string name = handle.Result.name;
            GameObject obj = Instantiate(handle.Result, _parent);
            obj.SetActive(false);
            m_actionDictionary.Add(name, obj);

            if (obj.TryGetComponent<ActionUIBaseController>(out ActionUIBaseController controller))
            {
                controller.OnInitialize();
            }

        }
        else
        {
            // Addressableでの取得に失敗した場合
            Debug.LogError("ゲームオブジェクト取得失敗。");
        }
    }

    private async void AllLoadCompleted(List<AsyncOperationHandle<GameObject>> _operationHandles)
    {
        try
        {
            foreach (var handle in _operationHandles)
            {
                // 待機
                await handle.Task;
            }

            // 全部読み込み終わった後に処理----------------------------------------------------

            SwitchState();

        }
        catch
        {
            Debug.LogError("待機処理失敗");
        }
    }

    */

}
