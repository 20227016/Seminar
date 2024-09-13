

using UnityEngine;

/// <summary>
/// NormalCharacter.cs
/// クラス説明
/// ノーマルキャラクター
///
/// 作成日: 9/3
/// 作成者: 山田智哉
/// </summary>
public class NormalCharacter : CharacterBase
{
    public override void Passive()
    {
        Debug.Log("ノーマルのパッシブ");
    }

    public override void Skill()
    {
        Debug.Log("ノーマルのスキル");
    }
}