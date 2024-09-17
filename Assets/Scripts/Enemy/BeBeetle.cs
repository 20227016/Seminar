
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

    [SerializeField]
    private Animator _animator = default;

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

        // 中心点を取得
        _boxCastStruct._originPos = this.transform.position;

        // 索敵範囲の距離
        _boxCastStruct._distance = 1f;

        // 自分のスケール(x)を取得
        float squareSize = transform.localScale.x;

        // BoxCastを正方形のサイズにする
        _boxCastStruct._size = new Vector2(squareSize, squareSize);



        // マウスクリックで攻撃アニメーションに切り替える
        if (Input.GetMouseButtonDown(0))  // 左クリック
        {
            _animator.SetInteger("", 1);  // 攻撃アニメーションに切り替え
        }
        else
        {
            _animator.SetInteger("", 0);  // アイドルアニメーションに戻す
        }
    }

}