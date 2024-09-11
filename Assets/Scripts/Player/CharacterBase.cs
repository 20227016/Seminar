
using UnityEngine;
using Fusion;
using System.Collections;
using UnityEngine.InputSystem;
using UniRx;

/// <summary>
/// CharacterBase.cs
/// クラス説明
/// キャラクターの基底クラス
///
/// 作成日: 9/2
/// 作成者: 山田智哉
/// </summary>
public class CharacterBase : MonoBehaviour, IAttackLight, IAttackStrong, IMove, IAvoidance, IComboCounter, IReceiveDamage, ITarget
{

    // ステータス
    [SerializeField, Tooltip("ステータス値")]
    private CharacterStatusStruct _characterStatusStruct = default;

    // ステート
    private CharacterStateEnum _characterStateEnum = default;

    // PlayerInputコンポーネントへの参照
    private PlayerInput _playerInput = default;

    // 現在のステート
    private CharacterStateEnum _currentState = default;

    // 現在HP量
    private ReactiveProperty<float> _currentHP = new ReactiveProperty<float>();

    // 現在スタミナ量
    private ReactiveProperty<float> _currentStamina = new ReactiveProperty<float>();

    #region プロパティ

    public IReadOnlyReactiveProperty<float> CurrentHP => _currentHP;

    public IReadOnlyReactiveProperty<float> CurrentStamina => _currentStamina;

    #endregion

    /// <summary>
    /// 起動時処理
    /// </summary>
    private void Awake()
    {
        // 初期化
        _currentState = CharacterStateEnum.IDLE;

        // ラッパークラスをインスタンス化
        _characterStatusStruct._playerStatus = new WrapperPlayerStatus();

        // 最大HPと最大スタミナをリアクティブプロパティとして設定
        _currentHP.Value = _characterStatusStruct._playerStatus.MaxHp;
        _currentStamina.Value = _characterStatusStruct._playerStatus.MaxStamina;

        // PlayerInputコンポーネントを取得
        _playerInput = GetComponent<PlayerInput>();

        // 入力アクションを一元管理
        RegisterInputActions(true);
    }

    private void OnDisable()
    {
        // アクション解除
        RegisterInputActions(false);
    }

    /// <summary>
    /// 入力アクションを一元管理して登録/解除する
    /// </summary>
    private void RegisterInputActions(bool isRegister)
    {
        // コールバック登録
        if (isRegister)
        {
            foreach (InputAction action in _playerInput.actions)
            {
                action.performed += context => HandleInput(context);

                // buttonタイプ以外の場合、キャンセル処理も登録
                if (action.type != InputActionType.Button)
                {
                    action.canceled += context => HandleInput(context);
                }
            }
        }
        // コールバック解除
        else
        {
            foreach (InputAction action in _playerInput.actions)
            {
                action.performed -= HandleInput;

                // buttonタイプ以外の場合、キャンセル処理も解除
                if (action.type != InputActionType.Button)
                {
                    action.canceled -= HandleInput;
                }
            }
        }
    }

    /// <summary>
    /// 入力管理
    /// </summary>
    private void HandleInput(InputAction.CallbackContext context)
    {

        // アクション名で入力処理を分岐
        switch (context.action.name)
        {
            case "Move":
                Move(context.ReadValue<Vector2>());
                break;

            case "AttackLight":
                AttackLight();
                break;

            case "AttackStrong":
                AttackStrong();
                break;

            case "Avoidance":
                Avoidance();
                break;

            default:
                Debug.LogWarning("未定義のアクション: " + context.action.name);
                break;
        }
    }

    public void Move(Vector2 moveDirection)
    {
        print(moveDirection);
    }

    public void AttackLight()
    {
        print("弱攻撃");
    }

    public void AttackStrong()
    {
        print("強攻撃");
    }

    public void Avoidance()
    {
        print("回避");
    }

    public void ComboCounter()
    {
        // コンボカウンター処理を実装
    }

    public void ReceiveDamage()
    {
        // ダメージ処理を実装
    }

    public void Target()
    {
        // ターゲット処理を実装
    }
}