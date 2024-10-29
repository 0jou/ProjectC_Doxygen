using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalControll : MonoBehaviour
{
    // アクション→経営に移動するポータルを操作するクラス
    // 制作者　山本/田内


    [Header("パーティクル")]
    [SerializeField]
    private List<ParticleSystem> m_particleList = null;


    [Header("停止するオブジェクト")]
    [SerializeField]
    private List<GameObject> m_objectList = new();


    [Header("キャラクターコア")]
    [SerializeField] private CharacterCore m_characterCore = null;

    void Start()
    {
        SetInitialize();
    }


    void Update()
    {
        CheckActive();
    }

    private void SetInitialize()
    {
        foreach (var particle in m_particleList)
        {
            particle.Stop();
        }

        foreach (var obj in m_objectList)
        {
            if (obj == null) continue;
            obj.SetActive(false);
        }
    }


    private void CheckActive()
    {
        // ターゲットのコアが存在していれば
        if (m_characterCore != null) return;

        // パーティクルを再生
        foreach (var particle in m_particleList)
        {
            if (particle.isPlaying == false)
                particle.Play();
        }

        // オブジェクトを動作
        foreach (var obj in m_objectList)
        {
            if (obj != null && obj.activeSelf == false)
            {
                obj.SetActive(true);
            }
        }

    }

}
