
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
public class PlayerAttackStrong : IAttackStrong
{
    private IComboCounter _comboCounter;

    public PlayerAttackStrong(IComboCounter comboCounter)
    {
        _comboCounter = comboCounter;
    }

    public void AttackStrong()
    {
        Debug.Log("強攻撃");
        int currentCombo = _comboCounter.GetCombo();
        _comboCounter.AddCombo();
    }

}