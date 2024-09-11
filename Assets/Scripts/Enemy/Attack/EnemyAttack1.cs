
using UnityEngine;
using System.Collections;

/// <summary>
/// EnemyAttack.cs
/// クラス説明
/// 敵の攻撃（○○なタイプ）
///
/// 作成日: 9/11
/// 作成者: 湯元
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObject/EnemyAttack/EnemyAttack1",fileName = "EnemyAttack1")]
public class EnemyAttack1 : EnemyAttackBase
{

    public override void Execute()
    {

        Debug.Log("敵の攻撃");

    }

}