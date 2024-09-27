
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
public class PlayerAttackLight : IAttackLight
{
    private IComboCounter _comboCounter;

    public PlayerAttackLight(IComboCounter comboCounter)
    {
        _comboCounter = comboCounter;
    }

    public void AttackLight()
    {
        Debug.Log("弱攻撃");


        int currentCombo = _comboCounter.GetCombo();
        _comboCounter.AddCombo();
    }
}