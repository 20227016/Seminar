
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
public class EnemyTest : MonoBehaviour 
{

    
    [SerializeField, Tooltip("プレイヤーオブジェクト")]
    private GameObject _player = default;
    [SerializeField,Tooltip("行動パターン")]
    private PatternBase _actionPatternBase = default;
    [SerializeField, Tooltip("敵のステータス")]
    private EnemyStatusStruct _enemyStatusStruct = default;
    [SerializeField, Tooltip("無視するレイヤー")]
    private LayerMask _ignoreLayer = default;

    private IEnemyAction _enemyAction = default;
   


    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Awake()
    {

       

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
      

        if (Input.GetKey(KeyCode.A))
        {

            _enemyAction = _actionPatternBase;
            _enemyAction.ExecuteAction(_enemyStatusStruct);

        }

    }

}