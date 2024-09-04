
using UnityEngine;

/// <summary>
/// GroundEnemypattern1.cs
/// クラス説明
/// 地上を歩くエネミーの行動パターン
///
/// 作成日: 9/3
/// 作成者: 高橋光栄
/// </summary>
public class GroundEnemyPattern1 : MonoBehaviour
{

    [SerializeField, Tooltip("プレイヤーオブジェクト")]
    private GameObject _player = default;
    [SerializeField, Tooltip("敵のステータス")]
    private EnemyStatusStruct enemyStatusStruct = default;

    private Vector3 _chasePos = default;



    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start()
    {

    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {

        // プレイヤーの位置を更新
        _chasePos = _player.transform.position;

        // プレイヤーの方向を計算
        Vector3 direction = (_chasePos.normalized );
        print(direction);

        // プレイヤーに向かって移動
        transform.position += transform.forward * enemyStatusStruct._moveSpeed * Time.deltaTime;
    }

}