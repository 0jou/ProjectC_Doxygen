using Arbor;
using ItemInfo;
using KinematicCharacterController;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using static CharacterCore;

public class EnemyParameters : MonoBehaviour
{
    [SerializeField] private EnemyID m_enemyID;
    public bool IsBoss { get { return EnemyDataBaseManager.instance.GetEnemyData(m_enemyID).IsBoss; } }

    [Header("Weightをコントロールするリグ")]
    [SerializeField] public Rig m_rig = null;

    private ArborFSM m_arborFSM;
    public ArborFSM Arbor { get { return m_arborFSM; } set { m_arborFSM = value; } }

    // =====================
    // 敵の戦闘中パラメータ（伊波）
    // =====================
    public EnemyData GetEnemyData()
    {
        return EnemyDataBaseManager.instance.GetEnemyData(m_enemyID);
    }
    public ChaseData GetChaseData()
    {
        return EnemyDataBaseManager.instance.GetEnemyData(m_enemyID).EnemyChaseData;
    }
    private EnemyAttackData m_nowAttackData;
    public EnemyAttackData NowAttackData { get { return m_nowAttackData; } set { m_nowAttackData = value; } }

    public EnemyAttackData GetFirstAttackData()
    {
        return EnemyDataBaseManager.instance.GetEnemyData(m_enemyID).FirstAttackData;
    }
    public EnemyAttackData GetSecondAttackData()
    {
        return EnemyDataBaseManager.instance.GetEnemyData(m_enemyID).SecondAttackData;
    }
    private int m_nowAttackDataType = 1;
    public int NowAttackDataType { get { return m_nowAttackDataType; } set { m_nowAttackDataType = value; } }
    private bool m_validSecondAttack = false;
    public bool ValidSecondAttack => m_validSecondAttack;


    private Transform m_target;
    public Transform Target { get { return m_target; } set { m_target = value; } }

    private Vector3 m_attackedVec;
    public Vector3 AttackedVec { get { return m_attackedVec; } set { m_attackedVec = value; } }

    private SearchTargets m_searchTargets = SearchTargets.Player | SearchTargets.FoodItem;
    public SearchTargets SearchTargets { get { return m_searchTargets; } set { if (m_searchTargets != value) { m_searchTargets = value; } } }

    private int m_dropNum = 1;
    public int DropNum { get { return m_dropNum; } set { m_dropNum = value; } }


    public void Start()
    {
        transform.TryGetComponent(out m_arborFSM);
        if (GetSecondAttackData().LotteryData.Count > 0)
        {
            m_validSecondAttack = true;
        }
        m_nowAttackData = GetFirstAttackData();
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
        List<DropItemInfo> dropItems = GetEnemyData().m_dropItemInfo;
        foreach (DropItemInfo info in dropItems)
        {
            allWeight += info.m_weightLottery;
        }

        if (!TryGetComponent(out CapsuleCollider capsuleCollider))
        {
            Debug.LogError("CapsuleColliderがみつかりませんでした。");
        }
        for (int dropNum = 0; dropNum < m_dropNum; ++dropNum)
        {
            int rand = UnityEngine.Random.Range(0, allWeight);
            for (int i = 0; i < dropItems.Count; ++i)
            {
                Quaternion randomRotation = Quaternion.identity;
                randomRotation.y = UnityEngine.Random.Range(0, 359);
                if (rand < dropItems[i].m_weightLottery)
                {
                    var itemData =
                        ItemDataBaseManager.instance.GetItemData(ItemTypeID.Ingredient,
                        (uint)dropItems[i].m_ingredientID);
                    // アイテムドロップ
                    float adjust = ((dropNum == 2) ? -1 : dropNum) * capsuleCollider.radius;
                    GameObject obj = Instantiate(itemData.ItemPrefab, transform.position + new Vector3(adjust, 0, adjust), randomRotation);
                    //obj.AddComponent<DroppingItemState>();
                    break;
                }
                rand -= dropItems[i].m_weightLottery;
            }
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
