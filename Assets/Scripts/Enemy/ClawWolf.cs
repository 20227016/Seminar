
using UnityEngine;
using System.Collections;

/// <summary>
/// ClawWolf.cs
/// クラス説明
///
///
/// 作成日: 9/12
/// 作成者: 湯元
/// </summary>
public class ClawWolf : BaseEnemy
{

    [SerializeField, Header("自分のアニメーター")]
    private Animator _myAnimator = default;
    [SerializeField, Header("プレイヤーのトランスフォーム")]
    private Transform _playerTrans = default;
    [SerializeField,Header("探索距離")]
    private float _searhDistance = 2f;

    private EnemyMovementState _movementState = EnemyMovementState.IDLE;
    private EnemyActionState _actionState = EnemyActionState.SEARCHING;


    //private int _attackNo = 1;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {

        //Raycastの基本設定
        BasicRaycast();
        _boxCastStruct._distance = _searhDistance;

    }

    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start()
    {



    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {

        //Rayの位置更新
        SetPostion();
        switch (_movementState)
        {

            // 待機
            case EnemyMovementState.IDLE:


                break;
            // 歩き
            case EnemyMovementState.WALKING:


                break;
            // 走り
            case EnemyMovementState.RUNNING:


                break;
            // ダウン
            case EnemyMovementState.DOWNED:


                break;
            // のけぞり
            case EnemyMovementState.STUNNED:


                break;



        }


    }

}
//if (Input.GetKeyDown(KeyCode.S))
//{

//    _myAnimator.Play("Die",2);

//}
//if (Input.GetKeyDown(KeyCode.Space))
//{

//    _myAnimator.SetTrigger("Attack"+ _attackNo);

//}
//if (Input.GetKeyDown(KeyCode.UpArrow))
//{

//    _attackNo += 1;

//}
//if (Input.GetKeyDown(KeyCode.DownArrow))
//{

//    _attackNo -= 1;

//}

//_attackNo = Mathf.Clamp(_attackNo,0,3);


