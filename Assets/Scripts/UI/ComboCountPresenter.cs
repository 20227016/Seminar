
using UnityEngine;
using TMPro;
using UniRx;

/// <summary>
/// ComboCountPresenter.cs
/// クラス説明
/// 
///
/// 作成日: 9/27
/// 作成者: 山田智哉
/// </summary>
public class ComboCountPresenter : MonoBehaviour
{
    private ComboCounter _comboCounter = default;

    private ComboCountView _comboCountView = default;

    [SerializeField]
    private TextMeshProUGUI _comboCountText = default;

    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start()
    {
        _comboCounter = ComboCounter.Instance;
        _comboCountView = new ComboCountView();
        _comboCounter.ComboCount.Subscribe(value => _comboCountView.UpdateText(value, _comboCountText));
    }

}