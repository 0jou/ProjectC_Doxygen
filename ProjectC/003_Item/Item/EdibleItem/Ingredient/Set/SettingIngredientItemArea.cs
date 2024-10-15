using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemInfo;
using System.Linq; // リンクも使います
using IngredientInfo;

public class SettingIngredientItemArea : BaseSettingIngredientItem
{

    // 制作者 (田内)

    [Header("ランダムにずらす座標値")]
    [SerializeField]
    private Vector3 m_randomPos = new();


    [Header("確実に設置する数")]
    [SerializeField]
    private int m_setNum = 0;


    [Header("ランダムに変更する設置数")]
    [Header("※確実に設置する数+ランダムに出たこの数")]
    [SerializeField]
    private int m_randomSetNum = 0;


    [Header("ランダムに設置する確率")]
    [SerializeField]
    [Range(0, 100)]
    private int m_random = 100;


    [Header("オブジェクトごとの最低距離間隔")]
    [SerializeField]
    private float m_distance = 1.0f;


    //======================================================================

    private void Start()
    {
        Setting();
    }

    // アイテムを設置
    private void Setting()
    {

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


        // 設置数
        int num = m_setNum;
        for (int i = 0; i < m_randomSetNum; ++i)
        {
            if (m_random < Random.Range(0, 100)) continue;
            num++;
        }

        List<GameObject> list = new();

        for (int i = 0; i < num; ++i)
        {



            Vector3 randomPos = transform.position;
            randomPos.x += Random.Range(-m_randomPos.x, m_randomPos.x);
            randomPos.z += Random.Range(-m_randomPos.z, m_randomPos.z);



            Quaternion randomRotation = transform.rotation;
            randomRotation.y = Random.Range(0, 359);


            RaycastHit hit = new();
            if (RayHitPosition(randomPos, out hit))
            {

                if (list.Where(_ => Vector3.Distance(hit.point, _.transform.position) < m_distance).Any())
                {
                    // 設置近くにアイテムが存在すれば作成しない
                    Debug.Log("アイテムの近くなので作成できませんでした");
                    continue;
                }

                // インスタンスを作成する
                list.Add(Instantiate(prefab, hit.point, randomRotation));

            }
            else
            {

                Debug.Log("作成できませんでした");
            }

        }

    }

}
