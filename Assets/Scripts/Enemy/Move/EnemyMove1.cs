
using UnityEngine;
using System.Collections;

/// <summary>
/// EnemyMove.cs
/// クラス説明
/// 敵の攻撃（○○なタイプ）
///
/// 作成日: 9/11
/// 作成者: 湯元
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObject/Enemymove/EnemyMove1", fileName = "EnemyMove1")]
public class EnemyMove1 : EnemyMoveBase
{

    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="moveDirection">移動方向</param>
    /// <param name="speed">プレイヤーのステータス</param>
    public override void Execute(Vector2 moveDirection, float speed)
    {

        Debug.Log("敵の移動");

    }

}