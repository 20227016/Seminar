
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editoraaaaa.cs
/// クラス説明
///
///
/// 作成日: /
/// 作成者: 
/// </summary>
[CustomEditor(typeof(EnemyTest))]
public class Editoraaaaa : Editor
{

    EnemyTest _target = default;
    private SerializedProperty _enemyAction;

    private void OnEnable()
    {

        _target = (EnemyTest)target;
        _enemyAction = serializedObject.FindProperty("EnemyAction");
        if (_enemyAction == null)
        {
            Debug.LogError("プロパティいない");
            return;
        }
        else
        {
            Debug.LogError("プロパティいる");

        }

    }

    public override void OnInspectorGUI()
    {

       
        //// 元のインスペクターGUIを描画
        //DrawDefaultInspector();
        //serializedObject.Update();
        //// オブジェクトフィールドの作成
        //EditorGUILayout.PropertyField(_enemyAction);
        
        //serializedObject.ApplyModifiedProperties();

    }

}