
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

    [SerializeField]
    private EnemyAttackBase _enemyAttackBase = default;
    [SerializeField]
    private EnemyMoveBase _enemyMoveBase= default;

    private IEnemyAttack _iEnemyAttack= default;
    private IEnemyMove _iEnemyMove = default;

    private Vector3 _chasePos = default;


    public override void Execute(EnemyStatusStruct enemyStatusStruct)
    {

        Debug.Log("GroundEnemyPattern1");
        _iEnemyAttack = _enemyAttackBase;
        _iEnemyMove = _enemyMoveBase;

        if (Input.GetKey(KeyCode.A))
        {

            Debug.Log("A");
            _iEnemyAttack.Execute();

        }
        if (Input.GetKey(KeyCode.S))
        {

            Debug.Log("B");
            _iEnemyMove.Execute(Vector2.one , 0f);

        }
        //// プレイヤーの位置を更新
        //_chasePos = _player.transform.position;

        //// プレイヤーの方向を計算
        //Vector3 direction = (_chasePos.normalized );
        //print(direction);

        //// プレイヤーに向かって移動
        //transform.position += transform.forward * enemyStatusStruct._moveSpeed * Time.deltaTime;

    }
}