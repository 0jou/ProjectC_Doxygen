using Arbor;
using ItemInfo;
using KinematicCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static CharacterCore;

public class EnemyParameters : MonoBehaviour
{
    [Header("ボス個体か(BGM変化等対応するか)")]
    [SerializeField] private bool m_isBoss = false;
    public bool IsBoss { get { return m_isBoss; } }

    // =====================
    // アイテムドロップ用（吉田）
    // =====================
    [Serializable]
    public struct DropItemInfo
    {
        public IngredientInfo.IngredientID m_ingredientID;
        public int m_weightLottery;
    }
    [SerializeField] public List<DropItemInfo> m_dropItemInfo;

    [Header("Weightをコントロールするリグ")]
    [SerializeField] public Rig m_rig = null;

    private ArborFSM m_arborFSM;
    public ArborFSM Arbor { get { return m_arborFSM; } set { m_arborFSM = value; } }

    public void Awake()
    {
        transform.root.TryGetComponent(out m_arborFSM);
    }

    //＝======================================================================
    //　シグナル利用した敵消滅用処理(TimeLine終了後にDeleteとアイテムドロップ)
    //========================================================================
    public void DestroyEnemy()
    {
        //消滅
        Destroy(transform.root.gameObject);
    }

    public void DropItem()
    {
        int allWeight = 0;
        foreach (DropItemInfo info in m_dropItemInfo)
        {
            allWeight += info.m_weightLottery;
        }

        int rand = UnityEngine.Random.Range(0, allWeight);
        for (int i = 0; i < m_dropItemInfo.Count; ++i)
        {
            if (rand < m_dropItemInfo[i].m_weightLottery)
            {
                var itemData =
                    ItemDataBaseManager.instance.GetItemData(ItemTypeID.Ingredient,
                    (uint)m_dropItemInfo[i].m_ingredientID);
                // アイテムドロップ
                Instantiate(itemData.ItemPrefab, transform.root.position, Quaternion.identity);
                break;
            }
            rand -= m_dropItemInfo[i].m_weightLottery;
        }
    }

    public void NoHitPlayer()
    {
        //レイヤーをNoHitPlayerに変更

        int layerNo = 7;

        if (gameObject.TryGetComponent(out MyCharacterController myCharacterController))
        {
            myCharacterController.AddNoHitTag("Player");
        }

        SetLayer(this.gameObject, layerNo, true);

    }

    //子のレイヤーもすべて変更する関数
    private void SetLayer(GameObject gameObject, int layerNo, bool needSetChildrens = true)
    {
        if (gameObject == null)
        {
            return;
        }
        gameObject.layer = layerNo;

        //子に設定する必要がない場合はここで終了
        if (!needSetChildrens)
        {
            return;
        }

        //子のレイヤーにも設定する
        foreach (Transform childTransform in gameObject.transform)
        {
            SetLayer(childTransform.gameObject, layerNo, needSetChildrens);
        }
    }

}
