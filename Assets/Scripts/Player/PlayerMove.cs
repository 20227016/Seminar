
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// PlayerMove.cs
/// クラス説明
/// プレイヤー歩行クラス
///
/// 作成日: 9/10
/// 作成者: 山田智哉(高橋光栄)
/// </summary>
public class PlayerMove : MonoBehaviour,IMove
{
    // rbの取得
    private Rigidbody _rb;

    private Vector3 movement = default;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>(); // Rigidbodyの取得

        this.FixedUpdateAsObservable()
            .Subscribe(_ =>
            {
                // 動け、ゴラァ
                _rb.MovePosition(_rb.position + movement * Time.deltaTime);
            });
    }


    // プレイヤーの移動方向
    public void Move(Vector2 moveDirection, float moveSpeed)
    {
        // 移動ロジック
        movement = new Vector3(moveDirection.x, 0, moveDirection.y) * moveSpeed;
    }
}
