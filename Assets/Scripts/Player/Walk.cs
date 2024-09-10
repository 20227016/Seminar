
using UnityEngine;

/// <summary>
/// Walk.cs
/// クラス説明
/// 歩かせるクラス
///
/// 作成日: 9/10
/// 作成者: 高橋光栄
/// </summary>
public class Walk : MonoBehaviour,IMove
{

    // プレイヤー構造体
    CharacterStatusStruct _characterStatusStruct = new CharacterStatusStruct();

    // rbの取得
    private Rigidbody _rb;

    // プレイヤーの移動方向
    public void Move(Vector2 moveDirection)
    {
        // 移動ロジック
        Vector3 movement = new Vector3(moveDirection.x, 0, moveDirection.y) * _characterStatusStruct._walkSpeed * Time.deltaTime;

        // 動け、ゴラァ
        _rb.MovePosition(_rb.position + movement);

    }

}