
using UniRx;
using UnityEngine;

/// <summary>
/// LockOnCursorPresenter.cs
/// クラス説明
/// ロックオンカーソルの仲介クラス
///
/// 作成日: 9/24
/// 作成者: 山田智哉
/// </summary>
public class LockOnCursorPresenter : MonoBehaviour
{
    private PlayerTargetting _player = default;

    private LockOnCursorView _lockOnCursorView = default;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {

        _lockOnCursorView = GetComponent<LockOnCursorView>();

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTargetting>();

        _player.LockOnEvent.Subscribe(target => _lockOnCursorView.UpdateLockOnCursorTarget(target));

    }

}