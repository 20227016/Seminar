
using UnityEngine;
using System.Collections;

/// <summary>
/// EnemyTest.cs
/// クラス説明
///
///
/// 作成日: /
/// 作成者: 
/// </summary>
[System.Serializable]
public class Enemy : BaseEnemy
{


    [SerializeField, Tooltip("プレイヤーオブジェクト")]
    private GameObject _player = default;
    [SerializeField, Tooltip("敵のステータス")]
    private EnemyStatusStruct _enemyStatusStruct = default;

    private IEnemyAction _enemyAction = default;

    /// <summary>
    /// 更新処理
    /// </summary>
    protected override void Update()
    {
        // 移動と攻撃の処理
        base.Update();
    }

}