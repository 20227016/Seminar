
using UnityEngine;
using System.Collections;

/// <summary>
/// PlayerAttackStrong.cs
/// クラス説明
/// プレイヤー強攻撃
///
/// 作成日: 9/10
/// 作成者: 山田智哉
/// </summary>
public class PlayerAttackStrong : MonoBehaviour, IAttackStrong
{
    public void AttackStrong()
    {
        Debug.Log("強攻撃");
    }
}