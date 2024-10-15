using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// オーナー情報保存用コンポーネント(倉田)
public class OwnerInfoTag : MonoBehaviour
{
    public CharacterGroupNumber GroupNo {  get;set; }

    public CharacterCore Characore { get; set; }
}
