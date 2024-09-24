
/// <summary>
/// ComboCounter.cs
/// クラス説明
/// コンボのシングルトンクラス
///
/// 作成日: 9/24
/// 作成者: 山田智哉
/// </summary>
public class ComboCounter : IComboCounter
{
    private static ComboCounter _instance;
    private int _comboCount;

    /// <summary>
    /// コンボカウンターのシングルトン化
    /// </summary>
    public static ComboCounter Instance
    {

        get
        {

            if (_instance == null)
            {

                _instance = new ComboCounter();

            }

            return _instance;

        }

    }

    public void AddCombo()
    {
        _comboCount++;
    }

    public int GetCombo()
    {
        return _comboCount;
    }
}