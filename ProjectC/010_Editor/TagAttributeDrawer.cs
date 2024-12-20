using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

#region 説明
/// <summary>
/// タグの専用UIを表示させるための属性
/// </summary>
#endregion
[AttributeUsage(AttributeTargets.Field)]
public class TagAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR
#region 説明
/// <summary>
/// タグ名の専用UIを表示させるためのPropertyDrawer
/// </summary>
#endregion
[CustomPropertyDrawer(typeof(TagAttribute))]
public class TagAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // 対象のプロパティが文字列かどうか
        if (property.propertyType != SerializedPropertyType.String)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        // タグフィールドを表示
        var tag = EditorGUI.TagField(position, label, property.stringValue);

        // タグ名を反映
        property.stringValue = tag;
    }
}
#endif