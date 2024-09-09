
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

    }

    public override void OnInspectorGUI()
    {
        //aaaaaa

        // 元のインスペクターGUIを描画
        DrawDefaultInspector();
        // オブジェクトフィールドの作成
        MonoBehaviour newTarget = (MonoBehaviour)EditorGUILayout.ObjectField(
            "Target", _target, typeof(MonoBehaviour), true);




    }

}