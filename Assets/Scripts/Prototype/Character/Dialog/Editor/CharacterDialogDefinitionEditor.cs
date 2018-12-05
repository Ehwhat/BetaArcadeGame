using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterDialogDefinition))]
public class CharacterDialogDefinitionEditor : Editor {

    private string[] foldoutNames;

    private SerializedProperty editorLists;

    private void OnEnable()
    {
        var values = System.Enum.GetNames(typeof(DialogContext));
        foldoutNames = new string[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            foldoutNames[i] = values[i];
        }

        editorLists = serializedObject.FindProperty("editorDialogLists");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Contexts", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        for (int i = 0; i < foldoutNames.Length; i++)
        {
            EditorPrefs.SetBool("CharacterDialogDefinitionEditorFoldout" + i, EditorGUILayout.Foldout(EditorPrefs.GetBool("CharacterDialogDefinitionEditorFoldout" + i), foldoutNames[i], true));
            if (EditorPrefs.GetBool("CharacterDialogDefinitionEditorFoldout" + i))
            {
                SerializedProperty list = editorLists.GetArrayElementAtIndex(i).FindPropertyRelative("list");
                for (int j = 0; j < list.arraySize; j++)
                {
                    SerializedProperty item = list.GetArrayElementAtIndex(j);
                    EditorGUILayout.BeginHorizontal();
                    if (item.objectReferenceValue != null)
                    {
                        if (!item.isExpanded)
                        {
                            if (GUILayout.Button("▼", EditorStyles.miniButtonMid, GUILayout.Width(20)))
                            {
                                item.isExpanded = true;
                            }
                        }
                        else
                        {
                            if (GUILayout.Button("▲", EditorStyles.miniButtonMid, GUILayout.Width(20)))
                            {
                                item.isExpanded = false;
                            }
                        }
                    }

                    EditorGUILayout.PropertyField(item);
                    if(GUILayout.Button("-", EditorStyles.miniButtonMid, GUILayout.Width(20)))
                    {
                        if (item.objectReferenceValue != null)
                        {
                            list.DeleteArrayElementAtIndex(j);
                        }
                        list.DeleteArrayElementAtIndex(j);
                    }
                    EditorGUILayout.EndHorizontal();

                    if (item.isExpanded && item.objectReferenceValue != null)
                    {
                        EditorGUI.indentLevel++;

                        Editor editor = CreateEditor(item.objectReferenceValue);
                        editor.DrawDefaultInspector();

                        EditorGUI.indentLevel--;
                    }
                }
                if(GUILayout.Button("+", EditorStyles.miniButtonMid))
                {
                    list.InsertArrayElementAtIndex(list.arraySize);
                }
            }
        }
        EditorGUI.indentLevel--;
        serializedObject.ApplyModifiedProperties();
    }

}
