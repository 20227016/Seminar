
using UnityEngine;
using System.Collections;

/// <summary>
/// PlayerAttackLight.cs
/// クラス説明
/// プレイヤー弱攻撃
///
/// 作成日: 9/10
/// 作成者: 山田智哉
/// </summary>
public class PlayerAttackLight : IAttackLight, IComboCounter
{

    public void AttackLight()
    {
        Debug.Log("弱攻撃");
    }

    public void ComboCounter()
    {
        
    }
}