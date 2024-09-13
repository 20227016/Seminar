
using UnityEngine;
using System.Collections;
using UniRx.Triggers;
using UniRx;

/// <summary>
/// PlayerRun.cs
/// クラス説明
///
///
/// 作成日: /
/// 作成者: 
/// </summary>
public class PlayerRun : MonoBehaviour, IMove
{
    // rbの取得
    private Rigidbody _rigidBody = default;

    // 移動方向
    private Vector3 _moveDirection = default;

    // 移動方向キャッシュ用
    private Vector3 _cachedMoveDirection = default;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        // FixedUpdateで移動
        this.FixedUpdateAsObservable()
            .Where(_ => _moveDirection != Vector3.zero)
            .Subscribe(_ =>
            {
                
                _rigidBody.MovePosition(_rigidBody.position + _moveDirection);
                transform.rotation = Quaternion.LookRotation(_moveDirection);
            });
    }

    private void FixedUpdate()
    {
        
    }

    public void Move(Vector2 moveDirection, float moveSpeed)
    {
        print("あ");
        // 再利用可能なVector3に変換結果を保存
        _cachedMoveDirection.Set(moveDirection.x, 0, moveDirection.y);
        _moveDirection = _cachedMoveDirection * moveSpeed * Time.deltaTime;

    }
}