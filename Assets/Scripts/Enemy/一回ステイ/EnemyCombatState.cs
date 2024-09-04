
using UnityEngine;
using System.Collections;

/// <summary>
/// EnemyCombatState.cs
/// クラス説明
/// エネミーの戦闘ステート
///
/// 作成日: 9/3
/// 作成者: 高橋光栄
/// </summary>
public class EnemyCombatState : IEnemyState
{
    public void EnterState(Enemy enemy)
    {
        Debug.Log("戦闘ステートに移行しました");
    }

    public void UpdateState(Enemy enemy)
    {
        enemy.HandleCombat();
    }

    public void ExitState(Enemy enemy)
    {
       
    }
}