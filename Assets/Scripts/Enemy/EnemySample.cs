
using UnityEngine;
using System;

/// <summary>
/// EnemySample.cs
/// クラス説明
///
///
/// 作成日: /
/// 作成者: 
/// </summary>
public class EnemySample : BaseEnemy
{

    [SerializeField, Header("自分のアニメーター")]
    private Animator _myAnimator = default;

    [SerializeField]
    private EnemyMovementState _movementState = EnemyMovementState.IDLE;
    [SerializeField]
    private EnemyActionState _actionState = EnemyActionState.SEARCHING;

    [SerializeField, Tooltip("探索範囲(前方距離)")]
    private float _searchRange = default;

    [SerializeField, Header("攻撃１の倍率")]
    private float _attackMultiplier1 = default;

    private GameObject _player = default;

    /// <summary>
    /// 初期処理
    /// </summary>
    private void Start()
    {

        //Raycastの基本設定
        BasicRaycast();
        // プレイヤーを取得
        _player = GameObject.FindWithTag("Player");

    }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected void UpdateLogic()
    {

        // レイキャスト設定
        SetPostion();
        switch (_movementState)
        {

            // 待機
            case EnemyMovementState.IDLE:


                break;


            // 移動
            case EnemyMovementState.RUNNING:


                break;


            // ダウン(ブレイク)
            case EnemyMovementState.DOWNED:


                break;

            // 死亡
            case EnemyMovementState.DIE:


                break;

        }


        switch (_actionState)
        {

            // サーチ
            case EnemyActionState.SEARCHING:

                break;


            // 攻撃
            case EnemyActionState.ATTACKING:

                break;
            // ガード
            case EnemyActionState.GUARDING:

                break;

        }

    }

    /// <summary>
    /// Lookat設定
    /// </summary>
    private void PlayerLook()
    {
        // プレイヤーのTransformを取得
        Transform playerTrans = _player.transform;
        // プレイヤーの位置を取得
        Vector3 playerPosition = playerTrans.position;
        // プレイヤーのY軸を無視したターゲットの位置を計算
        playerPosition.y = transform.position.y;
        // プレイヤーの方向に向く
        transform.LookAt(playerPosition);

    }

    /// <summary>
    /// プレイヤーを探す
    /// </summary>
    private void PlayerSearch()
    {
        RaycastHit hit = Search.BoxCast(_boxCastStruct);
        if (hit.collider.gameObject.layer == 6)
        {
            _movementState = EnemyMovementState.IDLE;
            _actionState = EnemyActionState.ATTACKING;
        }
        else
        {
            _movementState = EnemyMovementState.RUNNING;
        }

        if (!hit.collider)
        {
            return;

        }
    }

    ///// <summary>
    ///// 移動処理
    ///// </summary>
    //private void BeBeetleMove()
    //{

    //    _enemyAnimation.Movement(_myAnimator, 4);

    //    _newPosition = transform.position + -transform.right * _enemyStatusStruct._moveSpeed * Time.deltaTime;

    //    transform.position = _newPosition;


    //}

    ///// <summary>
    ///// ダウン処理
    ///// </summary>
    //private async UniTaskVoid BeBeetleDowned(CancellationToken token)
    //{

    //    try
    //    {

    //        _isDowned = true;

    //        transform.position = _hitAttackPos;

    //        _enemyAnimation.Movement(_myAnimator, 5);

    //        int downedTime = _enemyStatusStruct._downedTime * 1000;

    //        await UniTask.Delay(downedTime, cancellationToken: token);

    //        _isDowned = false;

    //        _movementState = EnemyMovementState.IDLE;

    //    }
    //    catch (OperationCanceledException)
    //    {

    //        Debug.Log("タスクがキャンセルされました");
    //    }
    //}

    ///// <summary>
    ///// 攻撃1の処理(2は未定)
    ///// </summary>
    //private async UniTaskVoid RushAttack(CancellationToken token)
    //{

    //    try
    //    {

    //        // 攻撃1の倍率に変更
    //        _currentAttackMultiplier = _attackMultiplier1;

    //        // 攻撃1アニメーション再生
    //        _enemyAnimation.Attack(_myAnimator, 1);

    //        int attackDelayTime = _enemyStatusStruct._attackDelayTime * 1000;

    //        await UniTask.Delay(attackDelayTime, cancellationToken: token);

    //        if (!_isAttack)
    //        {
    //            PlayerLook();
    //            _isAttack = true;
    //        }

    //        // 前に進む
    //        _newPosition = transform.position + transform.forward * _enemyStatusStruct._attackPowerSpeed * Time.deltaTime;

    //        // 突進処理
    //        transform.position = _newPosition;
    //    }
    //    catch (OperationCanceledException)
    //    {

    //        Debug.Log("タスクがキャンセルされました");
    //    }
    //}

    ///// <summary>
    ///// 死亡処理
    ///// </summary>
    ///// <param name="token"></param>
    ///// <returns></returns>
    //private async UniTaskVoid BeBeetleDeath(CancellationToken token)
    //{

    //    // その場で止まる
    //    transform.position = _hitAttackPos;

    //    // 死亡アニメーションを再生
    //    _enemyAnimation.Movement(_myAnimator, 6);

    //    // 現在再生されているアニメーションを取得
    //    AnimatorStateInfo stateInfo = _myAnimator.GetCurrentAnimatorStateInfo(0);

    //    // 現在再生されているアニメーションの時間を代入
    //    float animationLength = stateInfo.length;

    //    // アニメーションが終わるまで待機
    //    await UniTask.Delay(TimeSpan.FromSeconds(animationLength));

    //    // アニメーション終了後にオブジェクトを非表示にする
    //    this.gameObject.SetActive(false);
    //}

    ///// <summary>
    ///// UniTaskの破棄
    ///// </summary>
    //private void OperationCanceledException()
    //{
    //    _cancellatToken.Cancel();
    //}

    ///// <summary>
    ///// 攻撃処理
    ///// </summary>
    ///// <param name="hitCollider"></param>
    //public override void OnTriggerEnter(Collider hitCollider)
    //{

    //    if (_actionState == EnemyActionState.ATTACKING)
    //    {
    //        if (hitCollider.gameObject.layer == 6)
    //        {

    //            base.OnTriggerEnter(hitCollider);

    //            _isDeath = true;

    //            // 当たった位置で止まる
    //            _hitAttackPos = this.transform.position;

    //            _actionState = EnemyActionState.SEARCHING;

    //            _movementState = EnemyMovementState.DIE;

    //        }
    //        else if (hitCollider.gameObject.layer == 8)
    //        {

    //            _isAttack = false;
    //            _hitAttackPos = this.transform.position;
    //            _movementState = EnemyMovementState.DOWNED;
    //            _actionState = EnemyActionState.SEARCHING;
    //        }
    //        else
    //        {
    //            Debug.LogError("想定外のエラー");
    //        }
    //    }
    //}
}