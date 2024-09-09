
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
public class CharacterBase : MonoBehaviour, IMove, IAvoidance, IComboCounter, ReceiveDamage, ITarget
{
    // ステータス
    [SerializeField]
    private CharacterStatusStruct _characterStatusStruct = default;

    // ステート
    private CharacterStateEnum _characterStateEnum = default;

    // PlayerInputコンポーネントへの参照
    private PlayerInput _playerInput = default;

    // 現在のステート
    private CharacterStateEnum _currentState = default;

    // 移動方向
    private Vector2 _moveDirection = Vector2.zero;
    
    // カメラ
    private Camera _camera;

    /// <summary>
    /// 起動時処理
    /// </summary>
    private void Awake()
    {
        // 初期化
        _currentState = CharacterStateEnum.IDLE;

        // PlayerInputコンポーネントを取得
        _playerInput = GetComponent<PlayerInput>();

        // カメラを取得
        _camera = Camera.main;

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
    public void OnMove(InputAction.CallbackContext context)
    {
        // 移動入力を更新
        _moveDirection = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// 攻撃入力
    /// </summary>
    public void OnAttack(InputAction.CallbackContext context)
    {
        // Attackメソッドを呼び出す
        Attack();
    }

    /// <summary>
    /// 回避入力
    /// </summary>
    public void OnAvoidance(InputAction.CallbackContext context)
    {
        // Avoidanceメソッドを呼び出す
        Avoidance();
    }

    private void Update()
    {
        // 移動処理を毎フレーム実行
        Move(_moveDirection);
    }

    public void Move(Vector2 moveDirection)
    {
        if (moveDirection.sqrMagnitude < 0.01f)
        {
            // 入力がごく小さい場合は処理を行わない
            return;
        }

        // カメラの正面方向を取得
        Vector3 cameraForward = _camera.transform.forward;
        cameraForward.y = 0; // Y軸方向の移動は無視する
        cameraForward.Normalize();

        // 移動方向をカメラの正面方向に変換
        Vector3 move = cameraForward * moveDirection.y + _camera.transform.right * moveDirection.x;

        // 移動ベクトルを計算
        move *= _characterStatusStruct._moveSpeed * Time.deltaTime;

        // プレイヤーを移動方向に向ける
        Quaternion targetRotation = Quaternion.LookRotation(move);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);

        // transformを使って移動
        transform.position += move;

        //print("移動" + moveDirection.ToString());
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

    public void ReceiveDamage()
    {
        // ダメージ処理を実装
    }

    public void Target()
    {
        // ターゲット処理を実装
    }
}