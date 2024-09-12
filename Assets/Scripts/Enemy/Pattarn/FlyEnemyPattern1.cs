
using UnityEngine;
using System.Collections;

/// <summary>
/// FlyEnemyPattern1.cs
/// クラス説明
/// 敵の行動パターン（飛ぶタイプ１）
///
/// 作成日: 9/11
/// 作成者: 湯元＆高橋
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObject/EnemyActionPattern/FlyEnemyPattern1")]
public class FlyEnemyPattern1 : PatternBase
{

    BoxCastStruct _boxCastStruct = default;

    [SerializeField, Header("無視するレイヤー")]
    private LayerMask ignoreLayer = default;

    public override void Execute(EnemyStatusStruct enemyStatusStruct)
    {
        

    }

}