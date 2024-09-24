
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Threading;
using System;

/// <summary>
/// EnemyTest.cs
/// クラス説明
/// ビービートル(エネミー)クラス
///
/// 作成日: 9/13
/// 作成者: 高橋光栄
/// </summary>
[System.Serializable]
public class BeBeetle : BaseEnemy
{

    // UniTaskキャンセルトークン
    private CancellationTokenSource _cancellatToken = default;

    [SerializeField, Header("自分のアニメーター")]
    private Animator _myAnimator = default;

    [SerializeField, Header("プレイヤーのトランスフォーム")]
    private Transform _playerTrans = default;

    [SerializeField, Tooltip("探索範囲(前方距離)")]
    private float _searchRange = default;

    [SerializeField, Header("攻撃１の倍率")]
    private float _attackMultiplier1 = 1;

    [SerializeField]
    private EnemyMovementState _movementState = EnemyMovementState.IDLE;
    [SerializeField]
    private EnemyActionState   _actionState   = EnemyActionState.SEARCHING;

    [SerializeField,Tooltip("攻撃用コライダーを格納")]
    private BoxCollider _attackCollider = default;


    // 自分の現在の位置を格納
    private Vector3 _newPosition = default;

    private Vector3 _hitAttackPos = default;

    // 攻撃中か
    private bool _isAttack = false;
    // ダウン中か
    private bool _isDowned = false;

　　/// <summary>
    /// 初期化 
    /// </summary>
    private void Awake()
    {
        // Rayの位置更新
        SetPostion();

        // キャンセルトークン生成
        _cancellatToken = new CancellationTokenSource();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected void Update()
    {
        print(_movementState);
        // レイキャスト設定
        RayCastSetting();

        switch (_movementState)
        {

            // 待機
            case EnemyMovementState.IDLE:
               
                _enemyAnimation.Movement(_myAnimator, 0);

                break;


            // 移動
            case EnemyMovementState.RUNNING:

                BeBeetleMove();

                break;


            // ダウン(ブレイク)
            case EnemyMovementState.DOWNED:

                BeBeetleDowned(_cancellatToken.Token).Forget();

                break;

        }

        
        switch (_actionState)
        {

            // サーチ
            case EnemyActionState.SEARCHING:

                if(!_isDowned)
                {
                    // プレイヤーを見続ける
                    PlayerLook();

                    // RayHit判定
                    PlayerSearch();
                }

                break;


            // 攻撃
            case EnemyActionState.ATTACKING:


                // 攻撃処理
                RushAttack(_cancellatToken.Token).Forget();

                break;
        }
    }

    /// <summary>
    /// レイキャスト設定
    /// </summary>
    private void RayCastSetting()
    {

        //例キャスト初期設定
        BasicRaycast();

        // 中心点を取得
        _boxCastStruct._originPos = this.transform.position;

        // 自分のスケール(x)を取得
        float squareSize = transform.localScale.x;

        // BoxCastを正方形のサイズにする
        _boxCastStruct._size = new Vector2(squareSize, squareSize);
    }

    /// <summary>
    /// レイキャストの距離(探索範囲)
    /// </summary>
    protected override void SetDistance()
    {
        base.SetDistance();
        _boxCastStruct._distance = _searchRange;

    }

    /// <summary>
    /// Lookat設定
    /// </summary>
    private void PlayerLook()
    {
        // プレイヤーのTransformを取得
        Transform playerTrans = _playerTrans;

        // プレイヤーの位置を取得
        Vector3 playerPosition = playerTrans.position;

        // プレイヤーのY軸を無視したターゲットの位置を計算
        Vector3 lookPosition = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);

        // プレイヤーの方向に向く
        transform.LookAt(lookPosition);

    }

    /// <summary>
    /// プレイヤーを探す
    /// </summary>
    private void PlayerSearch()
    {
        RaycastHit hit = Search.BoxCast(_boxCastStruct);
        if(hit.collider.gameObject.layer == 6)
        {
            print("プレイヤーに触れました");
            _movementState = EnemyMovementState.IDLE;
            _actionState = EnemyActionState.ATTACKING;
        }
        else
        {
            print("プレイヤー以外に触れました");
            _movementState = EnemyMovementState.RUNNING;
        }

        if(!hit.collider)
        {
            return;

        }
    }

    /// <summary>
    /// 移動処理
    /// </summary>
    private void BeBeetleMove()
    {

        _enemyAnimation.Movement(_myAnimator, 4);

        _newPosition = transform.position + -transform.right * _enemyStatusStruct._moveSpeed * Time.deltaTime;

        transform.position = _newPosition;


    }

    /// <summary>
    /// ダウン処理
    /// </summary>
    private async UniTaskVoid BeBeetleDowned(CancellationToken token)
    {

        try
        {

            _isDowned = true;

            transform.position = _hitAttackPos;

            _enemyAnimation.Movement(_myAnimator, 5);

            int downedTime = _enemyStatusStruct._downedTime * 1000;

            await UniTask.Delay(downedTime, cancellationToken: token);

            _isDowned = false;

            _movementState = EnemyMovementState.IDLE;

        }
        catch (OperationCanceledException)
        {

            Debug.Log("タスクがキャンセルされました");
        }
    }

    /// <summary>
    /// 攻撃1の処理(2は未定)
    /// </summary>
    private async UniTaskVoid RushAttack(CancellationToken token)
    {

        try
        {
            
            // 攻撃1アニメーション再生
            _enemyAnimation.Attack(_myAnimator, 1);

            int attackDelayTime = _enemyStatusStruct._attackDelayTime * 1000;

            await UniTask.Delay(attackDelayTime, cancellationToken: token);

            if(!_isAttack)
            {
                PlayerLook();
                _isAttack = true;
            }

            // 前に進む
            _newPosition = transform.position + transform.forward * _enemyStatusStruct._attackPowerSpeed * Time.deltaTime;

            // 突進処理
            transform.position = _newPosition;
        }
        catch (OperationCanceledException)
        {

            Debug.Log("タスクがキャンセルされました");
        }
    }

    /// <summary>
    /// UniTaskの破棄
    /// </summary>
    private void OperationCanceledException()
    {
        _cancellatToken.Cancel();
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    /// <param name="hitCollider"></param>
    private void OnTriggerEnter(Collider hitCollider)
    {
        if (hitCollider.gameObject.layer == 6)
        {
            
        }
        else if (hitCollider.gameObject.layer == 8)
        {
            print("壁にぶっ刺さったお☆");
            _isAttack = false;
            _hitAttackPos = this.transform.position;
            _movementState = EnemyMovementState.DOWNED;
            _actionState = EnemyActionState.SEARCHING;
        }
        else
        {
           
        }
    }
}