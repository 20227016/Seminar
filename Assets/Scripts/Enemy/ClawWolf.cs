
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
    [SerializeField, Header("探索距離")]
    private float _searhDistance = 2f;

    private EnemyMovementState _movementState = EnemyMovementState.IDLE;
    private EnemyActionState _actionState = EnemyActionState.SEARCHING;

    private float _attackID = default;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {

        //Raycastの基本設定
        BasicRaycast();
        _boxCastStruct._distance = _searhDistance;
        _myAnimator.SetFloat("Attack1", _enemyStatusStruct._attackPowerSpeed);
        _myAnimator.SetFloat("Attack2", _enemyStatusStruct._attackPowerSpeed);
        _myAnimator.SetFloat("Attack3", _enemyStatusStruct._attackPowerSpeed);

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
        RaycastHit _hit = default;
        switch (_movementState)
        {

            // 待機
            case EnemyMovementState.IDLE:

                _hit = Search.BoxCast(_boxCastStruct);
                if (_hit.collider)
                {

                    _enemyAnimation.Attack(_myAnimator, 1);

                }

                break;
            // 歩き
            case EnemyMovementState.WALKING:


                break;
            // 走り
            case EnemyMovementState.RUNNING:


                break;
            // ダウン(ブレイク)
            case EnemyMovementState.DOWNED:


                break;
            // のけぞり(カウンターの時)
            case EnemyMovementState.STUNNED:


                break;

        }

    }

    private void OnTriggerExit(Collider other)
    {

        IReceiveDamage receiveDamage = other.GetComponent<IReceiveDamage>();
        receiveDamage.ReceiveDamage(_enemyStatusStruct._attackPower);

    
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


