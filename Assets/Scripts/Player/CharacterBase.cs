
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
public abstract class CharacterBase : MonoBehaviour, IAttackLight, IAttackStrong, IMove, IAvoidance, IComboCounter, IReceiveDamage, ITarget, ISkill, IPassive
{
    // ステータス
    [SerializeField, Tooltip("ステータス値")]
    protected CharacterStatusStruct _characterStatusStruct = default;

    // ステート
    protected CharacterStateEnum _characterStateEnum = default;

    // 現在のステート
    protected CharacterStateEnum _currentState = default;

    // 現在HP量
    protected ReactiveProperty<float> _currentHP = new ReactiveProperty<float>();

    // 現在スタミナ量
    protected ReactiveProperty<float> _currentStamina = new ReactiveProperty<float>();

    // PlayerInputコンポーネントへの参照
    protected PlayerInput _playerInput = default;

    // カメラコントローラー
    protected CameraDirection _cameraController;

    // 移動方向
    protected Vector2 _moveDirection = default;

    // 走るフラグ
    protected bool _isRun = default;

    // 各種インターフェース
    protected IMove _move = default;
    protected IAvoidance _avoidance = default;
    protected IAttackLight _playerAttackLight = default;
    protected IAttackStrong _playerAttackStrong = default;

    #region プロパティ

    public IReadOnlyReactiveProperty<float> CurrentHP => _currentHP;

    public IReadOnlyReactiveProperty<float> CurrentStamina => _currentStamina;

    #endregion

    /// <summary>
    /// 起動時処理
    /// </summary>
    protected virtual void Awake()
    {
        // 初期化
        _currentState = CharacterStateEnum.IDLE;

        IMoveProvider moveProvider = new PlayerMoveProvider();

        // キャッシュ
        _cameraController = new CameraDirection(Camera.main.transform);
        _playerInput = GetComponent<PlayerInput>();
        _move = moveProvider.GetWalk();
        _playerAttackLight = GetComponent<PlayerAttackLight>();
        _playerAttackStrong = GetComponent<PlayerAttackStrong>();
        _avoidance = GetComponent<PlayerAvoidance>();
        _characterStatusStruct._playerStatus = new WrapperPlayerStatus();

        // 最大HPと最大スタミナをリアクティブプロパティに設定
        _currentHP.Value = _characterStatusStruct._playerStatus.MaxHp;
        _currentStamina.Value = _characterStatusStruct._playerStatus.MaxStamina;

        // 入力アクションを登録
        RegisterInputActions(true);
    }

    /// <summary>
    /// 非アクティブ時処理
    /// </summary>
    protected virtual void OnDisable()
    {
        // 入力アクション解除
        RegisterInputActions(false);
    }

    /// <summary>
    /// 入力アクションを一元管理して登録/解除する
    /// </summary>
    private void RegisterInputActions(bool isRegister)
    {
        if (isRegister)
        {
            foreach (InputAction action in _playerInput.actions)
            {
                action.performed += HandleInput;
                action.canceled += HandleInput;
            }
        }
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
            return;
        }

        switch (actionType.Value)
        {
            case InputActionTypeEnum.Move:
                _moveDirection = _cameraController.GetCameraRelativeMoveDirection(context.ReadValue<Vector2>());
                Move(_moveDirection, _isRun ? _characterStatusStruct._runSpeed : _characterStatusStruct._walkSpeed);
                return;

            case InputActionTypeEnum.Dash:
                _isRun = !context.canceled;
                Move(_moveDirection, _isRun ? _characterStatusStruct._runSpeed : _characterStatusStruct._walkSpeed);
                return;

            case InputActionTypeEnum.AttackLight:
                if (context.canceled) return;
                AttackLight();
                return;

            case InputActionTypeEnum.AttackStrong:
                if (context.canceled) return;
                AttackStrong();
                return;

            case InputActionTypeEnum.Avoidance:
                if (context.canceled) return;
                Avoidance(_moveDirection, _characterStatusStruct._avoidanceDistance, _characterStatusStruct._avoidanceDuration);
                return;

            case InputActionTypeEnum.Target:
                if (context.canceled) return;
                Target();
                return;

            case InputActionTypeEnum.Skill:
                if (context.canceled) return;
                Skill();
                return;
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
        Debug.Log(gameObject.name + "が被弾");
    }

    public void Target()
    {
        Debug.Log("ターゲッティング");
    }

    public void Avoidance(Vector2 avoidanceDirection, float avoidanceDistance, float avoidanceDuration)
    {
        _avoidance.Avoidance(avoidanceDirection, avoidanceDistance, avoidanceDuration);
    }

    public abstract void Skill();

    public abstract void Passive();
}