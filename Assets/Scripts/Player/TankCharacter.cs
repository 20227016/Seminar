
using UnityEngine;
using System.Collections;

/// <summary>
/// TankCharacter.cs
/// クラス説明
///
///
/// 作成日: 9/13
/// 作成者: 山田智哉
/// </summary>
public class TankCharacter : CharacterBase
{
    public override void Passive()
    {
        Debug.Log("タンクのパッシブ");
    }

    public override void Skill()
    {
        Debug.Log("タンクのスキル");
    }
}