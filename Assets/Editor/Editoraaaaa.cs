using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editoraaaaa.cs
/// クラス説明
///
///　クラスを入れる枠を作り
///　クラスからインターフェースとりEnemyTestに格納
/// 作成日: /
/// 作成者: 
/// </summary>
[CustomEditor(typeof(EnemyTest))]
public class Editoraaaaa : Editor
{

    /// <summary>
    /// 対象のクラス
    /// </summary>
    private EnemyTest _target = default;
    /// <summary>
    /// データの保管場所
    /// </summary>
    EnemyPattanSaveData _data = default;
    /// <summary>
    /// IEnemyActionを継承しているクラスが入る
    /// </summary>
    private Type[] _types = default;
    /// <summary>
    /// _targetsInfoのデータを保管しているスクリクタプルのパス
    /// </summary>
    private const string _dataPaht = "Assets/ScriptableObject/EnemyPattanSaveData.asset";
    /// <summary>
    /// IEnemyActionを継承しているクラスが入る
    /// </summary>
    private string[] _typeNames = default;
    /// <summary>
    /// 現在のターゲットが選択しているインデックス
    /// </summary>
    private int _currentSelectIndex = default;

    /// <summary>
    /// 起動時処理
    /// </summary>
    private void OnEnable()
    {

        //データの保管場所を取得
        _data = AssetDatabase.LoadAssetAtPath<EnemyPattanSaveData>(_dataPaht);
        //現在参照中のクラス取得
        _target = (EnemyTest)target;
        if (_data.TargetsInfo.Count == 0)
        {

            _data.TargetsInfo.Add(new EnemyPattanSaveDataWrapper(_target.gameObject, 0));

        }
        //現在参照しているクラスが選んでいるインデックスを取得
        for (int i = 0; i < _data.TargetsInfo.Count; i++)
        {

            //現在参照しているクラスと一致したとき
            if (_data.TargetsInfo[i].Key == _target.gameObject)
            {

                _currentSelectIndex = _data.TargetsInfo[i].Valeu;
                break;

            }
            //リストにないときにリスト追加
            if (i == _data.TargetsInfo.Count - 1)
            {

                _data.TargetsInfo.Add(new EnemyPattanSaveDataWrapper(_target.gameObject, 0));

            }

        }

        /*
          AppDomain.CurrentDomain
          現在実行中のアプリケーションドメインを表す(unityエディタが動作している環境)
          GetAssemblies()
          現在のアプリケーションドメインにロードされているアセンブリを取得
        */
        _types = AppDomain
        .CurrentDomain.GetAssemblies()
        /*
          SelectMany
          Linqのメソッドで各アセンブリの全ての方を列挙後リスト化
          Assembly.GetTypes()
          
        */
        .SelectMany(Assembly => Assembly.GetTypes())
        /*
          typeof(IEnemyAction)　IsAssignableFrom(type)
          IEnemyAction型を取得し、IEnemyActionを継承しているクラスにtrueを返し特定する
          !type.IsInterface
          インターフェースそのものを除外
        */
        .Where(type => typeof(IEnemyAction).IsAssignableFrom(type) && !type.IsInterface)
        /*
         配列化
        */
        .ToArray();

        // クラス名の配列を作成
        _typeNames = _types.Select(type => type.FullName).ToArray();

    }

    public override void OnInspectorGUI()
    {

        // 元のインスペクターGUIを描画
        DrawDefaultInspector();

        // ドロップダウンメニューでクラスを選択
        int SelectIndex = EditorGUILayout.Popup(_currentSelectIndex, _typeNames);
        //変更がなかったら
        if (_currentSelectIndex == SelectIndex)
        {

            return;

        }
        //選択情報更新
        _currentSelectIndex = SelectIndex;
        //現在参照しているオブジェクトの選択情報更新
        for (int i = 0; i < _data.TargetsInfo.Count; i++)
        {

            //現在参照しているクラスと一致したとき
            if (_data.TargetsInfo[i].Key == _target.gameObject)
            {

                _data.TargetsInfo[i].Valeu = _currentSelectIndex;
                break;

            }

        }
        //選択したクラス名からクラスを取得
        Type selectType = Type.GetType(_typeNames[_currentSelectIndex]);
        //対象のクラスへ使用するインターフェースを渡す
        _target.EnemyAction = (IEnemyAction)selectType;
        //データの保管
        _data.TargetsInfo = _data.TargetsInfo;
        // アセットを保存
        EditorUtility.SetDirty(_data);
        EditorUtility.SetDirty(target);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

}