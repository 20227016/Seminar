
using UnityEngine;

public interface IMove
{
    /// <summary>
    /// 移動処理
    /// </summary>
    /// <param name="moveDirection">移動方向</param>
    void Move(Vector2 moveDirection, float moveSpeed);
}