
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

    private EnemyMovementState _movementState = EnemyMovementState.IDLE;
    private EnemyActionState   _actionState   = EnemyActionState.SEARCHING;  

    private void Start()
    {


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

                // プレイヤーを見続ける
                PlayerLook();
                print(Search.BoxCast(_boxCastStruct));

                break;

            // 走り
            case EnemyMovementState.RUNNING:
                print("移動(走る)");

                // プレイヤーを見続ける
                PlayerLook();

                break;

            // ダウン(ブレイク)
            case EnemyMovementState.DOWNED:
                print("ダウン");

                break;

            // のけぞり(カウンターの時)
            case EnemyMovementState.STUNNED:
                print("のけぞり");

                break;

        }

        switch (_movementState)
        {

            // 待機
            case EnemyMovementState.IDLE:
                print("アイドル");

                // プレイヤーを見続ける
                PlayerLook();
                print(Search.BoxCast(_boxCastStruct));

                break;

            // 走り
            case EnemyMovementState.RUNNING:
                print("移動(走る)");

                // プレイヤーを見続ける
                PlayerLook();

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

        // 索敵範囲の距離
        _boxCastStruct._distance = 50f;

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

}