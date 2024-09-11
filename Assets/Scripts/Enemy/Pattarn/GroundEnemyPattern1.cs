
using UnityEngine;

/// <summary>
/// GroundEnemypattern1.cs
/// クラス説明
/// 敵の行動パターン（地上を歩くタイプ１）
///
/// 作成日: 9/3
/// 作成者: 高橋光栄
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObject/EnemyActionPattern/GroundEnemyPattern1")]
public class GroundEnemyPattern1 : PatternBase
{



    private Vector3 _chasePos = default;


    public override void ExecuteAction(EnemyStatusStruct enemyStatusStruct)
    {


        Debug.Log(this);
        //// プレイヤーの位置を更新
        //_chasePos = _player.transform.position;

        //// プレイヤーの方向を計算
        //Vector3 direction = (_chasePos.normalized );
        //print(direction);

        //// プレイヤーに向かって移動
        //transform.position += transform.forward * enemyStatusStruct._moveSpeed * Time.deltaTime;

    }
}