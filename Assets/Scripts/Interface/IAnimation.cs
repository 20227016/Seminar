
using UnityEngine;

public interface IAnimation
{
    float TriggerAnimation(Animator animator, string animationName);
    void BoolAnimation(Animator animator, string animationName, bool isPlay);
}