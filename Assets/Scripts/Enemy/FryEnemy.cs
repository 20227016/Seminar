
using UnityEngine;
using System.Collections;

/// <summary>
/// EnemyTest.cs
/// クラス説明
///
///
/// 作成日: /
/// 作成者: 
/// </summary>
[System.Serializable]
public class FryEnemy : BaseEnemy
{


    [SerializeField, Tooltip("プレイヤーオブジェクト")]
    private GameObject _player = default;


    private void Start()
    {

        //例キャスト初期設定
        BasicRaycast();
        _boxCastStruct._distance = 3f;
        

    }

    /// <summary>
    /// 更新処理
    /// </summary>
    protected  void Update()
    {
        /*
         *  〇base.Update()の内容
         *  1.移動と攻撃の条件処理
         *  2.条件達成後、移動処理、攻撃処理実行
         *  移動→プレイヤーから変数内まで移動する
         *  攻撃→移動制限内に侵入したとき、攻撃(アニメーション)する
         */
    }

}