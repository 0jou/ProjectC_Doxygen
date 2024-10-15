using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using ConditionInfo;


[RequireComponent(typeof(HorizontalLayoutGroup))]
public class CreateConditionImage : MonoBehaviour
{

    // 選択中のアイテムの状態イメージを作成するスクリプト
    // GridLayOutGroupをアタッチし、Child AlignmentをMiddle Centerにしてください
    // 田内


    [Header("画像のサイズ")]
    [SerializeField]
    private Vector2 m_imageSize = new Vector2(100, 100);


    [Header("画像カラーの最高値")]
    [SerializeField]
    private Color m_maxColor = new Color(1, 1, 1, 1);

    [Header("画像カラーの最低値")]
    [SerializeField]
    private Color m_minColor = new Color(0, 0, 0, 1);


    // 作成したイメージを保持するリスト
    private List<GameObject> m_imageList = new();

    // 状態
    private ConditionID m_conditionID = new();

    // レベル
    private uint m_level = 0;


    // 状態イメージを作成する
    public bool CreateImage(BaseItemData _itemData)
    {
        // 既存のものを削除する
        DestroyConditionImage();


        // キャストできるか確認
        if (_itemData is BaseEdibleItem == false) return false;
        var edibleData = _itemData as BaseEdibleItem;


        // 状態
        m_conditionID = edibleData.ConditionID;
        if (m_conditionID == ConditionID.Normal) return false;


        // レベル
        m_level = edibleData.ItemLevel;


        // レベルの分作成
        for (int i = (int)m_level - 1; i >= 0; --i)
        {
            GameObject imageObj = new GameObject("Image");
            imageObj.transform.SetParent(gameObject.transform);


            // 画像をセット
            SetImage(imageObj, i);

            // サイズをセット
            SetRectTransform(imageObj);



            m_imageList.Add(imageObj);
        }

        return true;

    }

    void SetImage(GameObject _obj, int _current)
    {

        var data = ConditionDataBaseManager.instance.GetConditionData(m_conditionID);
        if (data == null)
        {
            Debug.LogError("この状態データは存在しません");
            return;
        }

        // Imageを追加
        Image image = _obj.AddComponent<Image>();
        image.sprite = data.ConditionSprite;


        // colorを更新
        Color color = m_maxColor - m_minColor;
        color /= m_level;

        image.color = m_maxColor;
        image.color = image.color - (color * _current);

    }


    void SetRectTransform(GameObject _obj)
    {
        RectTransform rectTransform = _obj.GetComponent<RectTransform>();
        if (rectTransform)
        {
            rectTransform.sizeDelta = m_imageSize;
        }

    }



    private void DestroyConditionImage()
    {
        foreach (var list in m_imageList)
        {
            // 削除する
            Destroy(list);
        }

        m_imageList = new();
    }

}
