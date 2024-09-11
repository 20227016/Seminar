
using UnityEngine;

/// <summary>
/// PattanBa.cs
/// クラス説明
/// 敵の行動パターンベース
///
/// 作成日: 9/11
/// 作成者: 湯元
/// </summary>
public abstract class PatternBase : ScriptableObject , IEnemyAction
{

    public abstract void Execute(EnemyStatusStruct enemyStatusStruct);

}