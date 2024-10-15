using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Unity.VisualScripting;
using UnityEditor;
using CriWare;


public class EatingEffectContoroll : MonoBehaviour
{
    //食べられる際に指定されたエフェクトが表示される処理（山本）

    private GameObject m_mealEffect = null;


    [Header("表示するエフェクトのプレハブ")]
    [SerializeField] private AssetReferenceGameObject m_mealEffectAssetReference = null;


    [Header("食べ物の原点からどの高さにエフェクトを表示するか")]
    [SerializeField] private float m_offsetHeight = 0.0f;

    void Start()
    {
        //シリアライズのアセットがセットされていないなら処理はいらないようにする
        if (m_mealEffectAssetReference == null)
        {
            return;
        }

        if (gameObject)
        {
            Addressables.LoadAssetAsync<GameObject>(m_mealEffectAssetReference)
                .Completed += OnLoadVisualEffect;
        }

    }

    private void OnLoadVisualEffect(AsyncOperationHandle<GameObject> _handle)
    {
        if (!gameObject)
        {
            return;
        }

        if (_handle.Status == AsyncOperationStatus.Succeeded)
        {
            //所持地点
            Vector3 pos = gameObject.transform.position + new Vector3(0.0f, m_offsetHeight, 0.0f);
            //取得できたエフェクトの作成
            m_mealEffect = Instantiate(_handle.Result, pos, Quaternion.identity);
            // m_mealEffect.transform.position = new Vector3(gameObject.transform.position.x, 
            //gameObject.transform.position.y + 1.0f,
            //gameObject.transform.position.x);

            if(m_mealEffect.TryGetComponent(out CriAtomSource _cri))
            {
                _cri.Stop();
            }

        }
        else
        {
            //Addressableでの取得に失敗
            Debug.LogError("エフェクトの取得失敗。");
        }
    }

    void Update()
    {

    }

    //Effect再生
    public void OnPlayEffect()
    {
        if (m_mealEffect == null) { return; }

        if (m_mealEffect.TryGetComponent<VisualEffect>(out var effect))
        {
            effect.Play();

            if (m_mealEffect.TryGetComponent(out CriAtomSource _cri))
            {
                _cri.Play();
            }

        }
    }

    //Effect停止
    public void OnStopEffect()
    {
        if (m_mealEffect == null) { return; }

        if (m_mealEffect.TryGetComponent<VisualEffect>(out var effect))
        {
            effect.Stop();
        }

    }

    //Effect位置の更新
    public void SetEffectPosition(Vector3 _pos)
    {
        if (m_mealEffect == null) { return; }

        m_mealEffect.transform.position = _pos;
    }


    public void OnDestroy()
    {
        if (m_mealEffect)
        {
            Destroy(m_mealEffect);
            m_mealEffect = null;
        }
    }

}
