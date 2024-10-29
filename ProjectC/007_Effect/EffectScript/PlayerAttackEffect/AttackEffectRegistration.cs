using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class AttackEffectRegistration : MonoBehaviour
{
    //AttackEffectを親のCharacterCoreに登録するコンポーネント（山本）

    [Header("HitStop中に値を0.0fに調節するVFXのPropertiesと元の値")]
    [SerializeField]
    SerializableDictionary<string, float> m_setValuePropertiesList;
    public SerializableDictionary<string, float> SetValueList { get { return m_setValuePropertiesList; } }


    private float m_originAddAngle = 0.0f;
    public float OriginAddAngle { get { return m_originAddAngle; } }

    public void Awake()
    {
        //生成時に親のCharacterCoreにエフェクトを登録する（山本）
        if (transform.root.TryGetComponent(out CharacterCore characterCore))
        {
            //一旦nullに
            characterCore.TempPlayerAttackEffect = null;

            //登録
            if (transform.TryGetComponent(out VisualEffect effect))
            {
                if (effect.HasFloat("AddAngle"))
                {
                    m_originAddAngle = effect.GetFloat("AddAngle");
                }

                characterCore.TempPlayerAttackEffect = effect;

            }

        }

    }


    //登録されているエフェクトのプロパティを引数の値に変更する関数（山本）
    public void SetVFXPropertiesValue(float _propertyValue)
    {
        if (transform.root.TryGetComponent(out CharacterCore characterCore))
        {
            foreach (var property in m_setValuePropertiesList)
            {
                if (characterCore.TempPlayerAttackEffect.HasFloat(property.Key))
                    characterCore.TempPlayerAttackEffect.SetFloat(property.Key, _propertyValue);
            }
        }
    }

    //登録されているエフェクトのプロパティを元のの値に変更する関数（山本）
    public void ReturnVFXPropertiesValue()
    {
        if (transform.root.TryGetComponent(out CharacterCore characterCore))
        {
            foreach (var property in m_setValuePropertiesList)
            {
                if (characterCore.TempPlayerAttackEffect.HasFloat(property.Key))
                    characterCore.TempPlayerAttackEffect.SetFloat(property.Key, property.Value);
            }
        }
    }


}
