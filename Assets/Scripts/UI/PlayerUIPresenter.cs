
using UnityEngine;
using UniRx;

/// <summary>
/// PlayerUIPresenter.cs
/// クラス説明
///
///
/// 作成日: 9/10
/// 作成者: 山田智哉
/// </summary>
public class PlayerUIPresenter : MonoBehaviour
{
    // モデルのプレイヤー
    private CharacterBase _player = default;

    // Viewクラス
    private PlayerUIViews _playerUIViews = default;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {
        _playerUIViews = GetComponent<PlayerUIViews>();

        try
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBase>();
        }
        catch
        {
            Debug.LogWarning("プレイヤーがおらへん");
        }

        _player.CurrentHP.Subscribe(_ => _playerUIViews.UpdateHP(_));

        _player.CurrentStamina.Subscribe(_ => _playerUIViews.UpdateStamina(_));

    }

}