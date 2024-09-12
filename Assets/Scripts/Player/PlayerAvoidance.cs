using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

/// <summary>
/// Avoidance.cs
/// クラス説明
/// プレイヤー回避
///
/// 作成日: 9/10
/// 作成者: 高橋光栄
/// </summary>
public class PlayerAvoidance : MonoBehaviour, IAvoidance
{

    private Rigidbody _rigidBody = default;

    // 回避できるか
    private bool _isAvoiding = false;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }


    /// <summary>
    /// 回避呼び出し処理
    /// </summary>
    public void Avoidance(Vector2 avoidanceDirection, float avoidanceDistance, float avoidanceDuration)
    {
        if (!_isAvoiding)
        {
            Vector3 normalizedAvoidanceDirection = new Vector3(avoidanceDirection.x, 0, avoidanceDirection.y).normalized;
            Vector3 avoidanceMovement = normalizedAvoidanceDirection * avoidanceDistance;

            _rigidBody.AddForce(avoidanceMovement, ForceMode.VelocityChange);

            // 回避の実行処理
            AvoidanceTime(avoidanceDuration).Forget();
        }
    }

    /// <summary>
    /// 回避の処理
    /// </summary>
    /// <returns>回避時間</returns>
    private async UniTaskVoid AvoidanceTime(float avoidanceDuration)
    {
        _isAvoiding = true;

        // 経過中、回避判定True
        await UniTask.Delay(TimeSpan.FromSeconds(avoidanceDuration));

        // 速度をゼロに
        _rigidBody.velocity = Vector3.zero;

        _isAvoiding = false;
    }
}