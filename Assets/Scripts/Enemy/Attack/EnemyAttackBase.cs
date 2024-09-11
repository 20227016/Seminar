
using UnityEngine;
using System.Collections;

/// <summary>
/// AttackBase.cs
/// クラス説明
/// 敵の攻撃ベース
///
/// 作成日: 9/11
/// 作成者: 湯元
/// </summary>
public abstract class EnemyAttackBase : ScriptableObject,IEnemyAttack
{

    /// <summary>
    /// 攻撃倍率
    /// </summary>
    protected  float _attackMultiplier = default;

    public abstract void Execute();

}