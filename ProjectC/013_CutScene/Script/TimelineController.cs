using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

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

    //エントリー時のポータル
    [Header("エントリーポータルのTransform")]
    [SerializeField] private Transform m_entryPortalTransform;

    //カメラ
    [Header("カリングするカメラ")]
    [SerializeField] private Camera m_mainCamera;

    //ActionItemWindow
    [Header("ActionItemWindow")]
    [SerializeField] private GameObject m_actionItemWindow;


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

    //UI隠す
    public void HideUI()
    {
        if (gameObject.TryGetComponent(out CreateNonProXiWindow window))
        {
            _ = window.HideOtherUI();
        }
    }

    //UI表示
    public void ShowUI()
    {
        if (gameObject.TryGetComponent(out CreateNonProXiWindow window))
        {
            _ = window.ShowOtherUI();
        }
    }


    public void PlayerContinue()
    {
        if(m_playerCharacterCore)
        {
            m_playerCharacterCore.m_animator.SetTrigger("Continue");
        }
    }

    //カメラのレイヤーマスク変更
    public void ChangeCameraLayerMask(string _layerName)
    {
        if(m_mainCamera==null)
        {
            return;
        }

       if(_layerName.Length==0)
        {
            return;
        }

       if(m_dictionaryCameraMasklList.TryGetValue(_layerName,out LayerMask mask))
        {
            m_mainCamera.cullingMask = mask;
        }

    }

    //プレイヤーAnimatorの状態遷移
    public void PlayerChangeState(string _trigerName)
    {
        if(m_playerCharacterCore)
        {
            m_playerCharacterCore.m_animator.SetTrigger(_trigerName);
        }
    }

    //ポータル開く
    public void ProtalOpen()
    {
        if(m_entryPortalTransform)
        {
            m_entryPortalTransform.DOScale(2.0f, 1.0f).SetEase(Ease.InOutBack);
        }
    }

    //ポータル閉じる
    public void ProtalCloth()
    {
        if (m_entryPortalTransform)
        {
            m_entryPortalTransform.DOScale(0.0f, 1.0f).SetEase(Ease.InOutBack).OnComplete(() => 
            {
                Destroy(m_entryPortalTransform.gameObject);
            }
                );


        }
    }

    //ActionItemWindow活性
    public void ActiveItemWindow()
    {
        if(m_actionItemWindow)
        {
            m_actionItemWindow.SetActive(true);
        }
    }

    //ActionItemWindow非活性
    public void NoActiveItemWindow()
    {
        if (m_actionItemWindow)
        {
            m_actionItemWindow.SetActive(false);
        }
    }


}
