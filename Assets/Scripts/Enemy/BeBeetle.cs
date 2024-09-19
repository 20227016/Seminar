
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

    private EnemyMovementState _movementState = EnemyMovementState.IDLE;
    private EnemyActionState   _actionState   = EnemyActionState.SEARCHING;

　　/// <summary>
    /// 初期化 
    /// </summary>
    private void Awake()
    {
        _searchRange = _boxCastStruct._distance;
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
                print("アイドル");

                _enemyAnimation.Movement(_myAnimator, 0);

                break;


            // 走り
            case EnemyMovementState.RUNNING:
                print("移動(走る)");

                _enemyAnimation.Movement(_myAnimator, 4);

                break;


            // ダウン(ブレイク)
            case EnemyMovementState.DOWNED:
                print("ダウン");

                _enemyAnimation.Movement(_myAnimator, 5);

                break;

        }

        
        switch (_actionState)
        {

            // サーチ
            case EnemyActionState.SEARCHING:
                print("サーチ");

                // プレイヤーを見続ける
                PlayerLook();

                // RayHit判定
                PlayerSearch();

                break;


            // 攻撃
            case EnemyActionState.ATTACKING:
                print("攻撃");

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
            print("プレイヤーに当たった");
            _actionState = EnemyActionState.ATTACKING;
        }
        else
        {
            print("プレイヤー以外に当たった");
            _movementState = EnemyMovementState.RUNNING;
        }
    }
}