
using UnityEngine;
using System.Collections;
using UniRx;
using UnityEngine.UI;

/// <summary>
/// PlayerUIViews.cs
/// クラス説明
/// 
///
/// 作成日: 9/10
/// 作成者: 山田智哉
/// </summary>
public class PlayerUIViews : MonoBehaviour
{
    [SerializeField, Tooltip("HPゲージ")]
    private Slider _hpBar = default;

    [SerializeField, Tooltip("HPゲージ")]
    private Slider _staminaBar = default;

    public void UpdateHP(float currentValue)
    {
        _hpBar.value = currentValue;
    }

    public void UpdateStamina(float currentValue)
    {
        _staminaBar.value = currentValue;
    }
}