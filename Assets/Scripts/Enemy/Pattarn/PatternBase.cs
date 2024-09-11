
using UnityEngine;

/// <summary>
/// PattanBa.cs
/// クラス説明
/// 行動パターンのベース
///
/// 作成日: 9/11
/// 作成者: 湯元
/// </summary>
public abstract class PatternBase : ScriptableObject , IEnemyAction
{

    public abstract void ExecuteAction(EnemyStatusStruct enemyStatusStruct);

}