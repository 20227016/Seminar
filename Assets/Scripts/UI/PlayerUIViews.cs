
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System;

/// <summary>
/// PlayerUIViews.cs
/// クラス説明
/// プレイヤー強攻撃
///
/// 作成日: 9/10
/// 作成者: 山田智哉
/// </summary>
public class PlayerUIViews : MonoBehaviour
{
    [SerializeField, Tooltip("HPゲージ")]
    private Slider _hpBar = default;

    [SerializeField, Tooltip("スタミナゲージ")]
    private Slider _staminaBar = default;

    // アニメーション速度の調整用
    [SerializeField, Tooltip("HP/スタミナの減少アニメーション速度")]
    private float animationSpeed = 0.5f;

    private IDisposable _hpAnimationDisposable;
    private IDisposable _staminaAnimationDisposable;


    public void UpdateHP(float newValue)
    {
        if (_hpAnimationDisposable != null)
        {
            _hpAnimationDisposable.Dispose();
        }

        _hpAnimationDisposable = Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                // HPゲージを滑らかに減らす
                _hpBar.value = Mathf.Lerp(_hpBar.value, newValue, Time.deltaTime * animationSpeed);

                // 目標値に近づいたらアニメーションを終了する
                if (Mathf.Abs(_hpBar.value - newValue) < 0.01f)
                {
                    _hpBar.value = newValue;
                    _hpAnimationDisposable.Dispose();
                }
            });
    }


    public void UpdateStamina(float newValue)
    {
        if (_staminaAnimationDisposable != null)
        {
            _staminaAnimationDisposable.Dispose();
        }

        _staminaAnimationDisposable = Observable.EveryUpdate()
            .Subscribe(_ =>
            {
                // スタミナゲージを滑らかに減らす
                _staminaBar.value = Mathf.Lerp(_staminaBar.value, newValue, Time.deltaTime * animationSpeed);
                
                // 目標値に近づいたらアニメーションを終了する
                if (Mathf.Abs(_staminaBar.value - newValue) < 0.01f)
                {
                    _staminaBar.value = newValue;
                    _staminaAnimationDisposable.Dispose();
                }
            });
    }

    private void OnDesable()
    {
        // Dispose
        _hpAnimationDisposable?.Dispose();
        _staminaAnimationDisposable?.Dispose();
    }
}