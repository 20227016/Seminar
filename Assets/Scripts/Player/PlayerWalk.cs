
using UnityEngine;

/// <summary>
/// PlayerMove.cs
/// クラス説明
/// プレイヤー歩行クラス
///
/// 作成日: 9/10
/// 作成者: 山田智哉(高橋光栄)
/// </summary>
public class PlayerWalk : MonoBehaviour,IMove
{

    // プレイヤー構造体
    CharacterStatusStruct _characterStatusStruct = new CharacterStatusStruct();

    // rbの取得
    private Rigidbody _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>(); // Rigidbodyの取得
    }

    // プレイヤーの移動方向
    public void Move(Vector2 moveDirection)
    {
        // 移動ロジック
        Vector3 movement = new Vector3(moveDirection.x, 0, moveDirection.y) * 1 * Time.deltaTime;

        // 動け、ゴラァ
        _rb.MovePosition(_rb.position + movement);

    }
}
