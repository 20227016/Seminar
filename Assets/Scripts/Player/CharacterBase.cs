
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

    // 各種インターフェース
    private IMove _move = default;
    private IAvoidance _avoidance = default;
    private IAttackLight _playerAttackLight = default;
    private IAttackStrong _playerAttackStrong = default;

    // 移動方向
    private Vector2 _moveDirection = default;

    // 走るフラグ
    private bool _isRun = default;

    // カメラのトランスフォーム
    private Transform _cameraTransform;

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

        // キャッシュ
        _cameraTransform = Camera.main.transform;
        _playerInput = GetComponent<PlayerInput>();
        _move = GetComponent<PlayerMove>();
        _playerAttackLight = GetComponent<PlayerAttackLight>();
        _playerAttackStrong = GetComponent<PlayerAttackStrong>();
        _avoidance = GetComponent<PlayerAvoidance>();
        
        // ラッパークラスをインスタンス化
        _characterStatusStruct._playerStatus = new WrapperPlayerStatus();

        // 最大HPと最大スタミナをリアクティブプロパティに設定
        _currentHP.Value = _characterStatusStruct._playerStatus.MaxHp;
        _currentStamina.Value = _characterStatusStruct._playerStatus.MaxStamina;

        // 入力アクションを登録
        RegisterInputActions(true);
    }

    private void OnDisable()
    {
        // 入力アクション解除
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
                action.performed += HandleInput;
                action.canceled += HandleInput;
            }
        }
        // コールバック解除
        else
        {
            foreach (InputAction action in _playerInput.actions)
            {
                action.performed -= HandleInput;
                action.canceled -= HandleInput;
            }
        }
    }

    /// <summary>
    /// 入力アクション管理
    /// </summary>
    private void HandleInput(InputAction.CallbackContext context)
    {
        InputActionTypeEnum? actionType = InputActionManager.GetActionType(context.action.name);

        if (actionType == null)
        {
            Debug.LogWarning("未定義のアクション: " + context.action.name);
            return;
        }

        switch (actionType.Value)
        {
            case InputActionTypeEnum.Move:
                _moveDirection = GetCameraRelativeMoveDirection(context.ReadValue<Vector2>());
                Move(_moveDirection, _isRun ? _characterStatusStruct._runSpeed : _characterStatusStruct._walkSpeed);
                return;

            case InputActionTypeEnum.Dash:
                _isRun = !context.canceled;
                Move(_moveDirection, _isRun ? _characterStatusStruct._runSpeed : _characterStatusStruct._walkSpeed);
                break;

            case InputActionTypeEnum.AttackLight:
                if (context.canceled) return;
                AttackLight();
                break;

            case InputActionTypeEnum.AttackStrong:
                if (context.canceled) return;
                AttackStrong();
                break;

            case InputActionTypeEnum.Avoidance:
                if (context.canceled) return;
                Avoidance(_moveDirection, _characterStatusStruct._avoidanceDistance, _characterStatusStruct._avoidanceDuration);
                break;
        }
    }

    public void Move(Vector2 moveDirection, float moveSpeed)
    {
        _moveDirection = moveDirection;
        _move.Move(moveDirection, moveSpeed);
    }

    public void AttackLight()
    {
        _playerAttackLight.AttackLight();
    }

    public void AttackStrong()
    {
        _playerAttackStrong.AttackStrong();
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

    public void Avoidance(Vector2 avoidanceDirection, float avoidanceDistance, float avoidanceDuration)
    {
        _avoidance.Avoidance(avoidanceDirection, avoidanceDistance, avoidanceDuration);
    }

    /// <summary>
    ///  カメラ基準の移動方向を計算
    /// </summary>
    /// <param name="inputDirection">入力方向</param>
    /// <returns><カメラ基準の移動方向/returns>
    private Vector2 GetCameraRelativeMoveDirection(Vector2 inputDirection)
    {
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.Normalize();
        right.Normalize();

        // 入力された移動方向をカメラの向きに変換
        Vector3 moveDirection = forward * inputDirection.y + right * inputDirection.x;

        return new Vector2(moveDirection.x, moveDirection.z);
    }
}