
using TMPro;

/// <summary>
/// ComboCountView.cs
/// クラス説明
///
///
/// 作成日: 9/27
/// 作成者: 山田智哉
/// </summary>
public class ComboCountView
{

    public void UpdateText(int value, TextMeshProUGUI text)
    {

        text.text = value.ToString();

        if(value <= 0)
        {
            text.gameObject.SetActive(false);
        }
        else
        {
            text.gameObject.SetActive(true);
        }
    }

}