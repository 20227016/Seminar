
using UnityEngine;
using System.Collections;

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

　　/// <summary>
    /// 初期化 
    /// </summary>
    private void Awake()
    {
        //Rayの位置更新
        SetPostion();
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected void Update()
    {

        // レイキャスト設定
        RayCastSetting();

        switch (_movementState)
        {

            // 待機
            case EnemyMovementState.IDLE:
               
                    _enemyAnimation.Movement(_myAnimator, 0);

                break;


            // 走り
            case EnemyMovementState.RUNNING:

                _enemyAnimation.Movement(_myAnimator, 4);

                break;


            // ダウン(ブレイク)
            case EnemyMovementState.DOWNED:

                _enemyAnimation.Movement(_myAnimator, 5);

                break;

        }

        
        switch (_actionState)
        {

            // サーチ
            case EnemyActionState.SEARCHING:

                // プレイヤーを見続ける
                PlayerLook();

                // RayHit判定
                PlayerSearch();

                break;


            // 攻撃
            case EnemyActionState.ATTACKING:

                // 攻撃1アニメーション再生
                _enemyAnimation.Attack(_myAnimator, 1);

               

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
        print(lookPosition+"前");

        // プレイヤーの方向に向く
        transform.LookAt(lookPosition);
        print(transform.rotation + "あと");

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
            _actionState = EnemyActionState.ATTACKING;
        }
        else
        {
            print("プレイヤー以外に触れました");
            _movementState = EnemyMovementState.RUNNING;
        }

        if(hit.collider)
        {
            return;

        }
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    /// <param name="hitCollider"></param>
    private void OnTriggerEnter(Collider hitCollider)
    {
        if (hitCollider.gameObject.layer == 6)
        {
            print("プレイヤーに当たったお☆");
        }
        else if (hitCollider.gameObject.layer == 8)
        {
            print("壁にぶっ刺さったお☆");
            _actionState = EnemyActionState.SEARCHING;
        }
        else
        {
            print("何も感じない");
        }
    }
}