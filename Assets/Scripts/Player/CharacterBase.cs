
using UnityEngine;
using Fusion;
using System.Collections;
using UnityEngine.InputSystem;

/// <summary>
/// CharacterBase.cs
/// クラス説明
/// キャラクターの基底クラス
///
/// 作成日: 9/2
/// 作成者: 山田智哉
/// </summary>
public class CharacterBase : MonoBehaviour, IMove, IAvoidance, IComboCounter, IDamage, ITarget
{
    // ステータス
    [SerializeField]
    private CharacterStatusStruct _characterStatusStruct = default;

    // ステート
    private CharacterStateEnum _characterStateEnum = default;

    // PlayerInputコンポーネントへの参照
    private PlayerInput _playerInput = default;

    /// <summary>
    /// 起動時処理
    /// </summary>
    private void Awake()
    {
        // PlayerInputコンポーネントを取得
        _playerInput = GetComponent<PlayerInput>();

        // 各アクションにコールバックを登録
        _playerInput.actions["Move"].performed += OnMove;
        _playerInput.actions["Move"].canceled += OnMove;
        _playerInput.actions["Attack"].performed += OnAttack;
        _playerInput.actions["Avoidance"].performed += OnAvoidance;
    }

    /// <summary>
    /// 非アクティブ時処理
    /// </summary>
    private void OnDisable()
    {
        // 各アクションのコールバックを解除
        _playerInput.actions["Move"].performed -= OnMove;
        _playerInput.actions["Move"].canceled -= OnMove;
        _playerInput.actions["Attack"].performed -= OnAttack;
        _playerInput.actions["Avoidance"].performed -= OnAvoidance;
    }

    /// <summary>
    /// 移動入力
    /// </summary>
    /// <param name="context"></param>
    public void OnMove(InputAction.CallbackContext context)
    {
        // 移動入力を更新
        Vector2 moveDirection = context.ReadValue<Vector2>();

        // Moveメソッドを呼び出す
        Move(moveDirection);
    }

    /// <summary>
    /// 攻撃入力
    /// </summary>
    /// <param name="context"></param>
    public void OnAttack(InputAction.CallbackContext context)
    {
        // Attackメソッドを呼び出す
        Attack();
    }

    /// <summary>
    /// 回避入力
    /// </summary>
    /// <param name="context"></param>
    public void OnAvoidance(InputAction.CallbackContext context)
    {
        // Avoidanceメソッドを呼び出す
        Avoidance();
    }

    public void Move(Vector2 moveDirection)
    {
        print("移動" + moveDirection.ToString());
        // 移動処理を実装
    }
    
    public void Attack()
    {
        print("攻撃");
        // 攻撃処理を実装
    }

    public void Avoidance()
    {
        print("回避");
        // 回避処理を実装
    }

    public void ComboCounter()
    {
        // コンボカウンター処理を実装
    }

    public void Damage()
    {
        // ダメージ処理を実装
    }

    public void Target()
    {
        // ターゲット処理を実装
    }
}