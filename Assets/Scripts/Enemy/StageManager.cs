
using UnityEngine;
using System.Collections;
using UniRx;

/// <summary>
/// StageManager.cs
/// クラス説明
/// ステージシーンを管理するためのクラス
///
/// 作成日: 9/30
/// 作成者: 高橋光栄
/// </summary>
public class StageManager : MonoBehaviour
{

    [SerializeField]
    private StageEnemyManagement enemyManagement;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {

        // StageEnemyManagementのイベントを購読
        enemyManagement.AllEnemiesDefeated.Subscribe(_ => HandleAllEnemiesDefeated());
    }

    /// <summary>
    /// 更新前処理
    /// </summary>
    private void Start()
    {

    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {

    }

    // シーン移動管理
    private void HandleAllEnemiesDefeated()
    {
        print("イベント取得");
    }

}