using UnityEngine;
using UnityEditor;
using ItemInfo;

namespace ItemIDEditor
{
    public class ItemIDConditionalDisableInInspectorAttribute : PropertyAttribute
    {
        public readonly string PropertyName;
        public readonly ItemTypeID ItemTypeID;
       

        public ItemIDConditionalDisableInInspectorAttribute(string propertyName, ItemTypeID itemTypeID)
        {
            this.PropertyName = propertyName;
            this.ItemTypeID = itemTypeID;
           //this.a = sObj;
        }
    }
}