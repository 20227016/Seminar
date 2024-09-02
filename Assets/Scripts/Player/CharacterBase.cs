
using UnityEngine;
using System.Collections;

/// <summary>
/// CharacterBase.cs
/// クラス説明
/// キャラクターの基底クラス
///
/// 作成日: 9/2
/// 作成者: 山田智哉
/// </summary>
public class CharacterBase : MonoBehaviour, IMove, IAttack, IAvoidance, IComboCounter, IDamage, ITarget
{
    public CharacterStatus characterStatus;

    public void Attack()
    {
        
    }

    public void Avoidance()
    {
        
    }

    public void ComboCounter()
    {
        
    }

    public void Damage()
    {
        
    }

    public void Move()
    {
        
    }

    public void Target()
    {
        
    }
}