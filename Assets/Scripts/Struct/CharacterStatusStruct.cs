
[System.Serializable]
public struct CharacterStatusStruct
{
    /// <summary>
    /// 最大HP、最大スタミナ
    /// </summary>
    public WrapperPlayerStatus _playerStatus;
    /// <summary>
    /// 移動速度
    /// </summary>
    public float _moveSpeed;
    /// <summary>
    /// 攻撃倍率
    /// </summary>
    public float _attackMultipiler;
    /// <summary>
    /// 防御力
    /// </summary>
    public float _defensePower;
    /// <summary>
    /// スキルタイム
    /// </summary>
    public float _skillTime;
    /// <summary>
    /// スキルクールタイム
    /// </summary>
    public float _skillCoolTime;
    /// <summary>
    /// カウンタータイム
    /// </summary>
    public float _counterTime;
}