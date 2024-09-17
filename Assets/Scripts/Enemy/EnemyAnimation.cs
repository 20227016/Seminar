
using UnityEngine;
using System.Collections;

/// <summary>
/// EnemyAnimation.cs
/// クラス説明
///
///
/// 作成日: /
/// 作成者: 
/// </summary>
public class EnemyAnimation : IEnemyAnimation
{


    /// <summary>
    /// 移動
    /// 方向の引数は
    /// １：全身
    /// ２：後退
    /// ３：右歩き
    /// ４：左歩き
    /// </summary>
    /// <param name="_animator">呼び出したオブジェクトのアニメーター</param>
    /// <param name="dirctionID">移動方向のID</param>
    public void Move(Animator _animator, int dirctionID)
    {

        switch (dirctionID)
        {

            //前進
            case 1:

                _animator.Play("WalkFWD",2);

                break;
            //後退
            case 2:

                _animator.Play("WalkBWD", 2);

                break;
            //右歩き
            case 3:

                _animator.Play("WalkRight", 2);

                break;
            //左歩き
            case 4:

                _animator.Play("WalkLeft", 2);

                break;

        }


    }

    /// <summary>
    /// 攻撃アニメーション
    /// </summary>
    /// <param name="_animator">呼び出したオブジェクトのアニメーター</param>
    /// <param name="_attackID">攻撃ナンバー</param>
    public void Attack(Animator _animator, int _attackID)
    {

        _animator.Play("Attack" + _attackID,1);

    }

}