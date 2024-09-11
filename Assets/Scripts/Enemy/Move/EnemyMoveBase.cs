
using UnityEngine;
using System.Collections;

/// <summary>
/// EnemyMoveBase.cs
/// クラス説明
/// 敵の移動ベース
///
/// 作成日: /
/// 作成者: 
/// </summary>
public abstract class EnemyMoveBase : ScriptableObject,IEnemyMove
{

    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="moveDirection">移動方向</param>
    /// <param name="speed">プレイヤーのステータス</param>
    public abstract void Execute(Vector2 moveDirection, float speed);

}