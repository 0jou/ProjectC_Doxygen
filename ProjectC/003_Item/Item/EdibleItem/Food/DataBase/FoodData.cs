using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IngredientInfo;
using FoodInfo;
using ItemInfo;
using PocketItemDataInfo;

[CreateAssetMenu]
[System.Serializable]
public class FoodData : BaseEdibleItem
{

    // 制作者(田内)

    public override void SetData()
    {
        m_itemTypeID = ItemTypeID.Food;
        m_itemID = (uint)m_foodID;
    }


    //============================
    // この料理のID


    [Header("材料のID")]
    [SerializeField]
    private FoodID m_foodID = FoodID.Omelette;


    //============================
    // 経営部分で使用するプレハブ


    [Header("経営部分のプレハブ(※インスタンスを作成する際に使用します)")]
    [SerializeField]
    private GameObject m_managementFoodPrefab = null;

    public GameObject ManagementFoodPrefab
    {
        get
        {
            if (m_managementFoodPrefab == null)
            {
                Debug.LogError("料理:" + m_foodID + "の経営用プレハブが登録されていません");
            }

            return m_managementFoodPrefab;
        }
    }


    //====================================
    // この料理に必要な素材(レシピ)


    // 料理作成に必要な材料のデータをまとめたクラス
    [System.Serializable]
    public class NeedIngredientObject
    {

        // ID
        [Header("材料ID")]
        [SerializeField]
        private IngredientID m_ingredientID = IngredientID.NormalMushroom;


        public IngredientID IngredientID { get { return m_ingredientID; } }

        // 必要な数
        [Header("必要な数")]
        [SerializeField]
        [Range(1, 10)]
        private int m_num;

        public int Num { get { return m_num; } }

    }


    [Header("料理に必要な材料リスト(レシピ)")]
    [SerializeField]
    private List<NeedIngredientObject> m_needIngredientObjectList = new();


    #region プロパティ説明
    ///--------------------------------------
    /// <summary>
    /// この料理に必要な材料の種類をまとめたリストを返す、読み取り専用プロパティ
    /// </summary>
    /// -------------------------------------
    /// <returns>
    /// 料理に必要な材料の種類をまとめたリスト
    /// </returns>
    /// --------------------------------------
    #endregion
    public List<NeedIngredientObject> NeedIngredientObjectList { get { return m_needIngredientObjectList; } }


    //===================================
    // 料理にかかる時間

    [Header("料理作成にかかる待機時間")]
    [SerializeField]
    [Range(0, 100)]
    private int m_createDelay = 2;

    public int CreateDelay { get { return m_createDelay; } }

    //===================================
    // 金額

    [Header("価格")]
    [SerializeField]
    [Range(0, 10000)]
    private uint m_price = 100;

    public uint Price { get { return m_price; } }


    //===================================
    // 投げたときの範囲

    [Header("投げた際の効果範囲")]
    [SerializeField]
    [Range(1, 100)]
    private float m_throwRange = 4.0f;

    public float ThrowRange { get { return m_throwRange; } }

    //=========================================================
    //                      処理
    //=========================================================

    /// <summary>
    /// 引数料理を作成後、ポケットに追加する
    /// </summary>
    public static void CreateFood(PocketType _pocketType, FoodID _foodID, bool _removeFlg)
    {
        // 作成できるか確認
        if (_removeFlg)
        {
            if (IsCreate(_pocketType, _foodID) == false) return;
        }

        // 料理を作成
        _pocketType.GetPocketItemDataManager().AddItem(ItemTypeID.Food, (uint)_foodID);

        // 使用した食材を取り除く
        if (_removeFlg) RemoveNeedIngredient(_pocketType, _foodID);

    }


    /// <summary>
    /// 引数料理が作成できるか確認するメソッド
    /// </summary>
    public static bool IsCreate(PocketType _pocketType, FoodID _id)
    {

        var data = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)_id);
        if (data == null)
        {
            Debug.LogError("作成しようとしている料理のデータは存在しません");
            return false;
        }


        // 所持しているか確認
        foreach (var list in data.NeedIngredientObjectList)
        {
            if (list == null) continue;

            // 対応するポケットから必要な材料があるか確認
            var itemNum = _pocketType.GetPocketItemDataManager().GetItemNum(ItemTypeID.Ingredient, (uint)list.IngredientID);


            if (itemNum < list.Num)
            {
                // 所持数が必要数より少なければ
                return false;
            }

        }

        // 作成可能であれば
        return true;

    }


    /// <summary>
    /// 引数料理が作成できる数を確認するメソッド
    /// </summary>
    public static int GetCreateNum(PocketType _pocketType, FoodID _id)
    {

        var data = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)_id);

        if (data == null)
        {
            Debug.LogError("作成しようとしている料理のデータは存在しません");
            return 0;
        }

        int num = 0;

        // ループ
        while (true)
        {
            // 必要な素材リスト
            foreach (var needIngredient in data.NeedIngredientObjectList)
            {
                if (needIngredient == null) continue;

                // 対応するポケットから素材所持数
                var itemNum = _pocketType.GetPocketItemDataManager().GetItemNum(ItemTypeID.Ingredient, (uint)needIngredient.IngredientID);

                // 現在の所持数が必要数を下回っていれば
                if (itemNum < needIngredient.Num * (num + 1)) return num;
            }

            // 作成可能数を加算
            num++;
        }
    }


    // 食材を減算する
    public static void RemoveNeedIngredient(PocketType _pocketType, FoodID _foodID)
    {
        var data = ItemDataBaseManager.instance.GetItemData<FoodData>(ItemTypeID.Food, (uint)_foodID);

        // 必要な素材リスト
        foreach (var list in data.NeedIngredientObjectList)
        {
            if (list == null) continue;

            // 必要な素材数分
            for (int i = 0; i < list.Num; ++i)
            {
                // 必要な素材を取り除く
                _pocketType.GetPocketItemDataManager().
                    RemoveItem(ItemTypeID.Ingredient, (uint)list.IngredientID);
            }
        }
    }

}
