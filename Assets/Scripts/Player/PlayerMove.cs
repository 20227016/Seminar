
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// PlayerMove.cs
/// クラス説明
/// プレイヤー歩行クラス
///
/// 作成日: 9/10
/// 作成者: 山田智哉
/// </summary>
public class PlayerMove : MonoBehaviour,IMove
{
    // rbの取得
    private Rigidbody _rigidBody = default;

    private Vector3 _movement = default;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        // FixedUpdateで移動
        this.FixedUpdateAsObservable()
            .Where(_ => _movement != Vector3.zero)
            .Subscribe(_ =>
            {
                _rigidBody.MovePosition(_rigidBody.position + _movement);
                transform.rotation = Quaternion.LookRotation(_movement);
            });
    }

    public void Move(Vector2 moveDirection, float moveSpeed)
    {
        _movement = new Vector3(moveDirection.x, 0, moveDirection.y) * moveSpeed * Time.deltaTime;
    }
}
