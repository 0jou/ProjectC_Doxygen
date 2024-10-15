using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TimelineController : MonoBehaviour
{
    //タイムラインコントロール用クラス
    //タイムライン中に発生させたいイベントとかを書く（山本）

    [Header("アクティブをコントロールするオブジェクトのリスト")]
    [SerializeField] private SerializableDictionary<string, GameObject[]>
        m_dictionaryActiveControllList;

    [Header("カメラで写さない対象をまとめたリスト")]
    [SerializeField]
    private SerializableDictionary<string, LayerMask>
        m_dictionaryCameraMasklList;

    //プレイヤー
    [Header("プレイヤーのキャラクターコア")]
    [SerializeField]private CharacterCore m_playerCharacterCore;

    //カメラの写す対象
    [SerializeField]
    private LayerMask m_layerMask;

    //リストのオブジェクトを非アクティブ化
    public void ActiveFalse(string _string)
    {
        if(m_dictionaryActiveControllList.TryGetValue(_string,out GameObject[]list))
        {
           foreach(var obj in list)
            {
                obj.SetActive(false);
            }
        }

    }

    //リストのオブジェクトをアクティブ化
    public void ActiveTrue(string _string)
    {
        if (m_dictionaryActiveControllList.TryGetValue(_string, out GameObject[] list))
        {
            foreach (var obj in list)
            {
                obj.SetActive(true);
            }
        }

    }

    public void CreateGameOverWindow()
    {
        
        if(gameObject.TryGetComponent(out CreateNonProXiWindow window))
        {
            _ = window.CreateWindow();

        }

    }

    public void HideUI()
    {
        if (gameObject.TryGetComponent(out CreateNonProXiWindow window))
        {
            _ = window.HideOtherUI();
        }
    }

    public void PlayerContinue()
    {
        if(m_playerCharacterCore)
        {
            m_playerCharacterCore.m_animator.SetTrigger("Continue");
        }
    }

    public void ChangeCameraLayerMask(string _layerName)
    {
       if(_layerName.Length==0)
        {
            return;
        }

       if(m_dictionaryCameraMasklList.TryGetValue(_layerName,out LayerMask mask))
        {
            Camera.main.cullingMask = mask;
        }

    }

}
