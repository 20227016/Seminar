
using UnityEngine;
using System.Collections;

/// <summary>
/// PlayerAnima.cs
/// クラス説明
///
///
/// 作成日: 10/02
/// 作成者: 
/// </summary>
public class PlayerAnima : IAnimation
{

    public void BoolAnimation(Animator animator, string animationName, bool isPlay)
    {
        animator.SetBool(animationName, isPlay);
    }

    public void TriggerAnimation(Animator animator, string animationName)
    {
        animator.SetTrigger(animationName);
    }
}