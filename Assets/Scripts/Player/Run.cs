
using UnityEngine;

/// <summary>
/// Run.cs
/// クラス説明
/// 走らせるクラス
///
/// 作成日: 9/10
/// 作成者: 高橋光栄
/// </summary>
public class Run : MonoBehaviour,IMove
{
    // プレイヤー構造体
    private CharacterStatusStruct _characterStatusStruct;

    // rbの取得
    private Rigidbody _rb;

    // プレイヤーの移動方向
    public void Move(Vector2 moveDirection)
    {
        // 移動ロジック
        Vector3 movement = new Vector3(moveDirection.x, 0, moveDirection.y) * _characterStatusStruct._runSpeed * Time.deltaTime;

        // 動け、ゴラァ
        _rb.MovePosition(_rb.position + movement);

    }

}