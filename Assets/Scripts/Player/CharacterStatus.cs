
/// <summary>
/// CharacterStatus.cs
/// クラス説明
/// キャラクターのステータス構造体
/// 
/// 作成日: 9/2
/// 作成者: 山田智哉
/// </summary>

[System.Serializable]
public struct CharacterStatus
{
    public int _hp;
    public int _power;
    public int _moveSpeed;
    public int _attackSpeed;
    public int _defence;
    public float _skillTime;
    public float _skillCoolTime;
    public float _counterTime;
}