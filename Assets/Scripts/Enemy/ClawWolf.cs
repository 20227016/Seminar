
using UnityEngine;
using System.Collections;

/// <summary>
/// ClawWolf.cs
/// クラス説明
///
///
/// 作成日: /
/// 作成者: 
/// </summary>
public class ClawWolf : BaseEnemy
{

    [SerializeField]
    private Animator _myAnimator = default;
    private EnemyMovementState _movementState = default;

    private int _attackNo = 1;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {

        //Raycastの基本設定
        BasicRaycast();

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

        if (Input.GetKeyDown(KeyCode.S))
        {

            _myAnimator.Play("Die",2);

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {

            _myAnimator.SetTrigger("Attack"+ _attackNo);

        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {

            _attackNo += 1;

        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {

            _attackNo -= 1;

        }

        _attackNo = Mathf.Clamp(_attackNo,0,3);

    }

}