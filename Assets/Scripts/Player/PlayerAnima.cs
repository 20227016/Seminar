
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

    public float TriggerAnimation(Animator animator, string animationName)
    {
        // パラメーターを配列に取得
        AnimatorControllerParameter[] parameters = animator.parameters;

        // 各パラメーターを調べてBool型の場合、リセットする
        foreach (AnimatorControllerParameter parameter in parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(parameter.name, false);
            }
        }

        animator.SetTrigger(animationName);

        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        float clipTime = clipInfo[0].clip.length;

        return 1;
    }
}