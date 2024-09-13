
using UnityEngine;
using System.Collections;

/// <summary>
/// EnemyTest.cs
/// クラス説明
/// ビービートル(エネミー)クラス
///
/// 作成日: 9/13
/// 作成者: 高橋光栄
/// </summary>
[System.Serializable]
public class BeBeetle : BaseEnemy
{


    [SerializeField, Tooltip("プレイヤーオブジェクト")]
    private GameObject _player = default;


    private void Start()
    {


    }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected  void Update()
    {
        //例キャスト初期設定
        BasicRaycast();
        _boxCastStruct._distance = 3f;
    }

}