
using UnityEngine;
using Fusion;
using System.Collections;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// CharacterBase.cs
/// クラス説明
/// キャラクターの基底クラス
///
/// 作成日: 9/2
/// 作成者: 山田智哉
/// </summary>
public abstract class CharacterBase : MonoBehaviour, IReceiveDamage
{
    // ステータス
    [SerializeField, Tooltip("ステータス値")]
    public CharacterStatusStruct _characterStatusStruct = default;

    // ステート
    protected CharacterStateEnum _characterStateEnum = default;

    // 現在のステート
    [HideInInspector]
    public CharacterStateEnum _currentState = default;

    // 現在のHP量
    protected ReactiveProperty<float> _currentHP = new ReactiveProperty<float>();

    // 現在のスタミナ量
    protected ReactiveProperty<float> _currentStamina = new ReactiveProperty<float>();

    // 現在のスキルポイント
    protected ReactiveProperty<float> _currentSkillPoint = new ReactiveProperty<float>();

    // PlayerInputコンポーネントへの参照
    protected PlayerInput _playerInput = default;

    // カメラコントローラー
    protected CameraDirection _cameraDirection = default;

    // 移動方向
    protected Vector2 _moveDirection = default;

    // 入力方向
    protected Vector2 _inputDirection = default;

    // 走るフラグ
    protected bool _isRun = default;

    // 移動速度
    protected float _moveSpeed = default;

    // 自身のトランスフォーム
    protected Transform _playerTransform = default;

    // 各種インターフェース
    protected IMoveProvider _moveProvider = new PlayerMoveProvider();
    protected IMove _move = default;

    protected IAvoidance _avoidance = new PlayerAvoidance();

    protected IAttackProvider _attackProvider = new PlayerAttackProvider();
    protected IAttackLight _playerAttackLight = default;
    protected IAttackStrong _playerAttackStrong = default;
    
    protected ITargetting _target = default;
    protected ISkill _skill = default;
    protected IPassive _passive = default;

    private Animator _animator = default;
    protected IAnimation _animation = new PlayerAnima();

    protected IResurrection _resurrection = new PlayerResurrection();

    #region プロパティ

    public IReadOnlyReactiveProperty<float> CurrentHP => _currentHP;

    public IReadOnlyReactiveProperty<float> CurrentStamina => _currentStamina;

    public IReadOnlyReactiveProperty<float> CurrentSkillPoint => _currentSkillPoint;

    #endregion

    /// <summary>
    /// 起動時処理
    /// </summary>
    protected virtual void Awake()
    {
        Initialize();

        // 最大HPと最大スタミナをリアクティブプロパティに設定
        _currentHP.Value = _characterStatusStruct._playerStatus.MaxHp;
        _currentStamina.Value = _characterStatusStruct._playerStatus.MaxStamina;
        _currentSkillPoint.Value = 0f;

        // 移動処理
        this.UpdateAsObservable()
            // 入力がないときは通らない
            .Where(_ => _inputDirection != Vector2.zero)
            .Subscribe(_ =>
            {

                // メインカメラから移動方向を算出
                _moveDirection = _cameraDirection.GetCameraRelativeMoveDirection(_inputDirection);
                Move(_playerTransform, _moveDirection, _moveSpeed);

            })
            .AddTo(this);

        _currentHP.
            Where(_ => _ <= 0f).
            Subscribe(_ => Death()).
            AddTo(this);
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    protected virtual void Initialize()
    {
        // キャッシュ
        _move = _moveProvider.GetWalk();
        _playerAttackLight = _attackProvider.GetAttackLight();
        _playerAttackStrong = _attackProvider.GetAttackStrong();

        _target = GetComponent<PlayerTargetting>();
        _skill = GetComponent<ISkill>();
        _passive = GetComponent<IPassive>();
        _characterStatusStruct._playerStatus = new WrapperPlayerStatus();
        _cameraDirection = new CameraDirection(Camera.main.transform);
        _playerInput = GetComponent<PlayerInput>();

        _animator = GetComponent<Animator>();

        // 初期化
        _currentState = CharacterStateEnum.IDLE;
        _playerTransform = this.transform;
        _moveSpeed = _characterStatusStruct._walkSpeed;
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
        // 一括登録
        if (isRegister)
        {

            foreach (InputAction action in _playerInput.actions)
            {

                action.performed += HandleInput;
                action.canceled += HandleInput;

            }

        }
        // 一括解除
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
        // 定義されているアクションを取得
        InputActionTypeEnum? actionType = InputActionManager.GetActionType(context.action.name);

        // 定義にないアクションはリターン
        if (actionType == null)
        {
            return;
        }

        switch (actionType.Value)
        {

            case InputActionTypeEnum.Move:
                print("あ");
                _animation.BoolAnimation(_animator, "Walk", !context.canceled);
                _inputDirection = context.ReadValue<Vector2>();
                return;

            case InputActionTypeEnum.Dash:

                _isRun = !context.canceled;
                if (_isRun)
                {
                    _move = _moveProvider.GetRun();
                    _moveSpeed = _characterStatusStruct._runSpeed;
                }
                else
                {
                    _move = _moveProvider.GetWalk();
                    _moveSpeed = _characterStatusStruct._walkSpeed;
                }
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
                Avoidance();
                return;

            case InputActionTypeEnum.Target:

                if (context.canceled) return;
                Targetting();
                return;

            case InputActionTypeEnum.Skill:

                if (context.canceled) return;
                if (_currentSkillPoint.Value >= _characterStatusStruct._skillPointUpperLimit)
                {
                    Skill(this, _characterStatusStruct._skillTime, _characterStatusStruct._skillCoolTime);
                }
                return;

            case InputActionTypeEnum.Resurrection:

                Resurrection();
                return;
        }
    }

    public virtual void Move(Transform transform, Vector2 moveDirection, float moveSpeed)
    {
        _move.Move(transform, moveDirection, moveSpeed);
    }

    public virtual void AttackLight()
    {
        _animation.TriggerAnimation(_animator, "AttackLight");
        _playerAttackLight.AttackLight();
        
    }

    public virtual void AttackStrong()
    {
        _animation.TriggerAnimation(_animator, "AttackStrong");
        _playerAttackStrong.AttackStrong();
    }

    public virtual void Targetting()
    {
        _target.Targetting();
    }

    public virtual void Avoidance()
    {
        _avoidance.Avoidance(_playerTransform, _moveDirection, _characterStatusStruct._avoidanceDistance, _characterStatusStruct._avoidanceDuration);
    }

    public abstract void Skill(CharacterBase characterBase, float skillTime, float skillCoolTime);

    public virtual void Resurrection()
    {
        _resurrection.Resurrection(_characterStatusStruct._ressurectionTime, this.transform);
    }

    public virtual void ReceiveDamage(int damegeValue)
    {
        
        _currentHP.Value -= damegeValue - _characterStatusStruct._defensePower;

        if (_currentHP.Value <= 0)
        {

            _currentHP.Value = 0f;

        }
    }

    public virtual void Death()
    {
        Debug.Log("死んだ");
        _currentState = CharacterStateEnum.DEATH;
        this.gameObject.SetActive(false);
    }
}