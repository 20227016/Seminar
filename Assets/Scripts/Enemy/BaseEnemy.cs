
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// BaseEnemy.cs
/// クラス説明
///
///
/// 作成日: /
/// 作成者: 
/// </summary>
public abstract class BaseEnemy : MonoBehaviour
{

    [SerializeField, Header("無視するレイヤー")]
    protected LayerMask ignoreLayer = default;
    [SerializeField, Header("キャラクターステータス")]
    protected EnemyStatusStruct _enemyStatusStruct = default;
    [SerializeField, Header("無視するレイヤー")]
    protected List<string> _tags = new List<string>();

    // 探索するときに使用する構造体
    protected BoxCastStruct _boxCastStruct = default;

    protected IEnemyAttack _attack = new EnemyAttack();

    protected IEnemyMove _move = new EnemyMove();


    private void OnDrawGizmos()
    {

        /// 可視化
        Gizmos.color = Color.red;

        // 現在の位置を基準にボックスキャストの範囲を描画
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(_boxCastStruct._originPos, Quaternion.LookRotation(_boxCastStruct._direction), Vector3.one);
        Gizmos.matrix = rotationMatrix;

        // BoxCastの可視化
        Gizmos.DrawWireCube(Vector3.zero, _boxCastStruct._size);

        // レイの先の地点を描画
        Gizmos.DrawWireCube(_boxCastStruct._direction * _boxCastStruct._distance, _boxCastStruct._size);

    }

    protected virtual void BasicRaycast()
    {

        // 自分の目の前から
        // 中心点
        _boxCastStruct._originPos = this.transform.position + this.transform.localScale / 2;
        // 半径（直径ではない）
        _boxCastStruct._size = (transform.localScale - Vector3.forward * transform.localScale.z);
        _boxCastStruct._size += Vector3.right * _boxCastStruct._size.x * 2;
        _boxCastStruct._size -= Vector3.one / 100;
        // 方向
        _boxCastStruct._direction = transform.forward;
        // 距離
        _boxCastStruct._distance = 5f;
        if (_tags.Count != 0)
        {
            _boxCastStruct._tags = _tags.ToArray();

        }
        // 無視するレイヤー
        _boxCastStruct._layerMask = ~ignoreLayer;

    }


}