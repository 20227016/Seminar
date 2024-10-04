
using Fusion;
using UnityEngine;

/// <summary>
/// PlayerMove.cs
/// クラス説明
/// プレイヤー歩行クラス
///
/// 作成日: 9/10
/// 作成者: 山田智哉
/// </summary>
public class PlayerWalk :IMove
{
    // 移動方向キャッシュ用
    private Vector3 _cachedMoveDirection = default;

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void Move(Transform transform, Vector2 moveDirection, float moveSpeed)
    {

        // Vector2をVector3に変換
        _cachedMoveDirection.Set(moveDirection.x, 0, moveDirection.y);

        // 移動量を計算 
        Vector3 moveVector = _cachedMoveDirection * moveSpeed * Time.deltaTime;

        // 現在の位置に移動量を加算して移動
        transform.position += moveVector;

        // 移動方向が変わった場合は、その方向にキャラクターを向ける
        if (_cachedMoveDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(_cachedMoveDirection);
        }
    }
}
