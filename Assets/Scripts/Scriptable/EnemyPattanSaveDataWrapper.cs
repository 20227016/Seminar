using UnityEngine;
using System.Collections;

/// <summary>
/// EnemyPattanSaveDataW.cs
/// クラス説明
///
///
/// 作成日: 9/10
/// 作成者: 
/// </summary>
[System.Serializable]
public class EnemyPattanSaveDataWrapper
{



    public GameObject Key { get => _key; set => _key = value; }
    public int Valeu { get => _valeu; set => _valeu = value; }

    private GameObject _key = default;
    private int _valeu = default;

    public EnemyPattanSaveDataWrapper(GameObject key, int valeu)
    {

        this._valeu = valeu;
        this._key = key;

    }

}