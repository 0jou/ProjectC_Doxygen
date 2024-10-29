using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Arbor;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[AddComponentMenu("")]
[AddBehaviourMenu("Customer/AppearEffect")]
public class AppearEffectCustomer : BaseCustomerStateBehaviour
{
    [Header("出現するエフェクト")]
    [SerializeField] private AssetReferenceGameObject m_appearEffectAssetRef = null;

    private GameObject m_appearEffect = null;

    // Use this for initialization
    void Start()
    {

        //シリアライズのアセットがセットされていないなら処理はいらないようにする
        if (m_appearEffectAssetRef == null)
        {
            return;
        }

        if (gameObject)
        {
            Addressables.LoadAssetAsync<GameObject>(m_appearEffectAssetRef)
                .Completed += OnLoadVisualEffect;
        }

    }

    private void OnLoadVisualEffect(AsyncOperationHandle<GameObject> _handle)
    {
        if (gameObject == null)
        {
            return;
        }

        if (_handle.Status == AsyncOperationStatus.Succeeded)
        {
            var data = GetRootGameObject();
            if (data == null)
            {
                return;
            }

            //取得できたエフェクトの作成
            m_appearEffect = Instantiate(_handle.Result, data.transform);

            //エフェクト監視コンポーネントををルートオブジェクトに追加
            var observ = data.AddComponent<EffectObservation>();
            if(observ)
            {
                observ.SetObservationEffect(m_appearEffect);
            }

        }
        else
        {
            //Addressableでの取得に失敗
            Debug.LogError("エフェクトの取得失敗。");
        }
    }

    // Use this for awake state
    public override void OnStateAwake()
    {
    }

    // Use this for enter state
    public override void OnStateBegin()
    {
        

    }

    // Use this for exit state
    public override void OnStateEnd()
    {
    }

    // OnStateUpdate is called once per frame
    public override void OnStateUpdate()
    {
    }

    // OnStateLateUpdate is called once per frame, after Update has finished.
    public override void OnStateLateUpdate()
    {
    }
}
