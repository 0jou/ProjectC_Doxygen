using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// InputProvider の interface スクリプト (濱口)

// ---------------------------------------
// CharacterのInputProvider(略)CharacterIP
// ---------------------------------------
//本体
public interface IInputProvider
{
    Vector3 LookVector { get; }

    // 進行方向
    Vector3 MoveVector { get; }

    // Attackトリガー 攻撃種類によって変えるためintに変更(倉田)
    int AttackType { get; set; }

    // dashトリガー
    bool DoDush { get; }
    bool OnPressedDush { get; }
    bool OnReleasedDush { get; }

    //Rollingトリガー(山本海豪)
    bool DoRolling { get; }

    bool SelectLeftItem { get; }
    bool SelectRightItem { get; }
    bool UseItem { get; }

    //童話スキル1発動トリガー（山本）
    bool UseStorySkill_1 { get; }
    /// <summary>
    /// 童話スキル2発動トリガー（山本）
    /// </summary>
    bool UseStorySkill_2 { get; }

    // アイテム採取
    bool Gathering { get; }

    // アイテム設置
    bool PutItem { get; }

    // アイテム投げる準備
    bool ReadyThrowItem { get; }

    // アイテム投げる
    bool ThrowItem { get; }

    // アイテム投げる準備時、投げる方向（マウス座標）
    Vector2 ThrowAim { get; }

    //キャンセル
    bool Cancel { get; }

    //食べる
    bool Eat { get; }

    // 敵側
    public bool IsArrive { get; }

    public Vector3 Destination { get;}
}
// PlayerIPの初期化用クラス
public class NullCharacterIP : IInputProvider
{
    public static NullCharacterIP NullInstance = new();

    public Vector3 LookVector { get; } = Vector3.zero;
    public Vector3 MoveVector { get;} = Vector3.zero;


    public int AttackType { get; set; } = 0;

    public bool DoDush { get; } = false;
    public bool OnPressedDush { get; } = false;
    public bool OnReleasedDush { get; } = false;


    public bool DoRolling { get; } = false;

    public bool UseStorySkill_1 { get; } = false;
    /// <summary>
    /// 追加（山本）
    /// </summary>
    public bool UseStorySkill_2 { get; } = false;

    public bool SelectLeftItem { get; } = false;
    public bool SelectRightItem { get; } = false;
    public bool UseItem { get; } = false;

    public bool Gathering { get; } = false;
    public bool PutItem { get; } = false;
    public bool ReadyThrowItem { get; } = false;
    public bool ThrowItem { get; } = false;
    public Vector2 ThrowAim { get; } = Vector2.zero;
    public bool Cancel { get; }
    public bool Eat { get; }
    // 敵側
    public bool IsArrive { get; } = false;

    public Vector3 Destination { get; } = Vector3.zero;
}



// ---------------------------------------
// CameraInputProvider(略)CameraIP
// ---------------------------------------
// 本体
public interface ICameraInputProvider
{
    //カメラ移動
    public Vector2 CameraXY { get; }
}

// CameraIPの初期化用クラス
public class NullCameraIP : ICameraInputProvider
{
    public static NullCameraIP NullInstance = new();

    public Vector2 CameraXY { get; } = Vector2.zero;
}
