using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using IngredientInfo;

public class SettingIngredientItemPoint : BaseSettingIngredientItem
{

    //===============================================================



    [Header("設置する確率")]
    [SerializeField]
    [Range(0, 100)]
    private int m_random = 100;


    //======================================================================

    private void Start()
    {
        Setting();
    }


    // アイテムを設置
    private void Setting()
    {

        // 設置しない
        if (m_random < Random.Range(0, 100)) return;

        
        var data = ItemDataBaseManager.instance.GetItemData(ItemTypeID.Ingredient, (uint)m_ingredientID);
        if (data == null)
        {
            Debug.LogError("アイテムが存在しません。登録されているか確認してください。");
            return;
        }

        GameObject prefab = data.ItemPrefab;
        if (prefab == null)
        {
            Debug.LogError("プレハブが存在しません。");
            return;
        }

        // 回転角度
        Quaternion randomRotation = transform.rotation;
        randomRotation.y = Random.Range(0, 359);


        RaycastHit hit = new();
        if (RayHitPosition(gameObject.transform.position,out hit))
        {
            // インスタンスを作成する
            Instantiate(prefab, hit.point, randomRotation);

        }
        else
        {
            Debug.LogError("設置できませんでした");
        }

    }

}
