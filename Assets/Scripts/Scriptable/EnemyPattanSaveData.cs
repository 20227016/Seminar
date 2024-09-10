using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Scriqtaple.cs
/// クラス説明
///
///
/// 作成日: /
/// 作成者: 
/// </summary>
[System.Serializable]
[CreateAssetMenu(fileName = "EnemyPattanSaveData", menuName = "Scriptable/EnemyPattanSaveData")]
public class EnemyPattanSaveData : ScriptableObject
{

    public List<EnemyPattanSaveDataWrapper> TargetsInfo { get => _targetsInfo; set => _targetsInfo = value; }

    private List<EnemyPattanSaveDataWrapper> _targetsInfo = new List<EnemyPattanSaveDataWrapper>();

}
