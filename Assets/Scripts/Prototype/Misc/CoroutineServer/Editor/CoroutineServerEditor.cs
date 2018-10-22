using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CoroutineServer))]
public class CoroutineServerEditor : Editor {

    GUIStyle listBoxStyle;

    public void OnEnable()
    {

    }

    public override void OnInspectorGUI()
    {
        //if (Application.isEditor)
        //{
        //    EditorGUILayout.LabelField("Currently In Editor, no Coroutines in action", EditorStyles.largeLabel);
        //    return;
        //}

        EditorGUILayout.LabelField("Currently Running Coroutines", EditorStyles.largeLabel);

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        for (int i = 0; i < CoroutineServer.runningCoroutines.Count; i++)
        {
            IEnumerator e = CoroutineServer.runningCoroutines[i];
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(e.ToString());

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();

    }

}
