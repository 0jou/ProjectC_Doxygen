using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;
using static CharacterCore;

// MetaAI基底クラス＆呼び出し？（伊波）

public interface IMetaAI<T>
{
    public static IMetaAI<T> Instance { get; set; }

    // クラスTリスト
    HashSet<T> ObjectList { get; }

    void RegisterObject(T chara);
}
