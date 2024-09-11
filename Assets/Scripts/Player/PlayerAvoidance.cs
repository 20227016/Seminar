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
    // プレイヤー構造体
    private CharacterStatusStruct _characterStatusStruct = default;

    private Rigidbody _rb = default;

    // 回避できるか
    private bool _isAvoiding = false;

    // 回避の持続時間
    private float _avoidanceTime = 1f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }


    /// <summary>
    /// 回避呼び出し処理
    /// </summary>
    public void Avoidance(Vector2 avoidanceDirection)
    {
        if (!_isAvoiding)
        {
            // 回避の実行処理
            AvoidanceTime(avoidanceDirection).Forget();
        }
    }

    /// <summary>
    /// 回避の処理
    /// </summary>
    /// <returns></returns>
    private async UniTaskVoid AvoidanceTime(Vector2 avoidanceDirection)
    {

        print("回避開始");

        _isAvoiding = true;

        // 回避の移動処理
        Vector3 normalizedAvoidanceDirection = new Vector3(avoidanceDirection.x, 0, avoidanceDirection.y).normalized;
        Vector3 avoidanceMovement = normalizedAvoidanceDirection * 1;

        // 回避の加速
        _rb.AddForce(avoidanceMovement, ForceMode.VelocityChange);

        // 経過中、回避判定True
        await UniTask.Delay(TimeSpan.FromSeconds(_avoidanceTime));

        _isAvoiding = false;

        print("回避終了");
    }
}