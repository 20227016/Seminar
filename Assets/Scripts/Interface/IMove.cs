
using UnityEngine;

/// <summary>
/// IMove.cs
/// クラス説明
///
///
/// 作成日: 9/2
/// 作成者: 山田智哉
/// </summary>
public interface IMove
{
    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="moveDirection">移動方向</param>
    void Move(Vector2 moveDirection);
}