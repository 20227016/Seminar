
using UnityEngine;
using Cysharp.Threading.Tasks;

/// <summary>
/// Avoidance.cs
/// クラス説明
/// プレイヤー回避
///
/// 作成日: 9/10
/// 作成者: 高橋光栄
/// </summary>
public class PlayerAvoidance : IAvoidance
{
    // 回避中フラグ
    private bool _isAvoiding = false;


    public void Avoidance(Transform transform, Vector2 avoidanceDirection, float avoidanceDistance, float avoidanceDuration)
    {
        // 回避中はリターン
        if (_isAvoiding) return;

        // 正規化した移動方向を算出
        Vector3 normalizedAvoidanceDirection = new Vector3(avoidanceDirection.x, 0, avoidanceDirection.y).normalized;

        // 開始位置と終了位置を取得
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + normalizedAvoidanceDirection * avoidanceDistance;

        // 回避処理
        AvoidanceCoroutine(transform, startPosition, endPosition, avoidanceDuration).Forget();
    }

    /// <summary>
    /// 回避処理の非同期コルーチン
    /// </summary>
    private async UniTaskVoid AvoidanceCoroutine(Transform transform, Vector3 startPosition, Vector3 endPosition, float duration)
    {
        _isAvoiding = true;
        float elapsedTime = 0f;

        // 回避の持続時間が経過するまでループ
        while (elapsedTime < duration)
        {

            // 時間計測
            elapsedTime += Time.deltaTime;

            // `progress` は回避処理の進行度を表す割合。0 で開始位置、1 で終了位置。
            float progress = Mathf.Clamp01(elapsedTime / duration);

            // `progress` を使って、開始位置から終了位置までを補間
            transform.position = Vector3.Lerp(startPosition, endPosition, progress);

            // フレームごとに待機
            await UniTask.Yield();

        }

        _isAvoiding = false;
    }
}