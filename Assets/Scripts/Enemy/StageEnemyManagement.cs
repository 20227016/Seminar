
using UnityEngine;
using System;
using System.Collections.Generic;
using UniRx;

/// <summary>
/// StageEnemyManagement.cs
/// クラス説明
///　ステージに設置したエネミーを管理するためのクラス
///
/// 作成日: 9/30
/// 作成者: 高橋光栄
/// </summary>
public class StageEnemyManagement : MonoBehaviour
{

    private Subject<Unit> OnAllEnemiesDefeated = new Subject<Unit>();

    [SerializeField,Tooltip("このリストにはステージに設置したエネミーのオブジェクトをいれてください")]
    private List<GameObject> enemyOBJ = new List<GameObject>();

    private int _enemyObjCount = default;

    private int _enemyLayer = default;


    public IObservable<Unit> AllEnemiesDefeated => OnAllEnemiesDefeated;

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {

        // リストの要素数を取得
        _enemyObjCount = enemyOBJ.Count;

        // エネミーのレイヤー(int)を取得
        _enemyLayer = LayerMask.NameToLayer("Enemy");

    }

    private void Start()
    {

        // リストに格納されたすべてのオブジェクトを確認し、Enemyレイヤーだけを追加
        CheckAndAddEnemiesFromList();
    }

    /// <summary>
    /// リストのすべてのオブジェクトのレイヤーを確認し、Enemyレイヤーのみ追加
    /// </summary>
    private void CheckAndAddEnemiesFromList()
    {

        // Enemyレイヤーでないものを削除
        for (int i = enemyOBJ.Count - 1; i >= 0; i--)
        {
            GameObject obj = enemyOBJ[i];

            if (obj.layer != _enemyLayer)
            {
                Debug.Log(obj.name + " はEnemyレイヤーではありません。削除します");
                enemyOBJ.RemoveAt(i);
            }
        }
    }

    /// <summary>
    /// 更新処理
    /// </summary>
    private void Update()
    {

        // リストに格納されたオブジェクトがすべて非アクティブになっているか
        if (AllObjectHidden())
        {
            OnAllObjectHidden();
        }

    }

    private bool AllObjectHidden()
    {

        // すべてのオブジェクトのアクティブ状態を確認
        foreach (GameObject obj in enemyOBJ)
        {
            if (obj.activeSelf)
            {
                // 1つでもアクティブなオブジェクトがあればfalseを返す
                return false;
            }
        }

        // すべて非アクティブになった場合、Trueを返す
        return true;
    }

    private void OnAllObjectHidden()
    {
        // UniRxでステージクリアのイベントを発行。
        // ステージマネージャーでそのイベントを取得し、
        // シーンを移動させる。この際、使いまわせるように、シーン(i)で指定して、できるようにする
        // これにより、イベント発行毎にintを＋して、Stage2.Stage3と移動できるようにする

        OnAllEnemiesDefeated.OnNext(Unit.Default);
    }


}