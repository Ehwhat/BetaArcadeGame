using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using JoshExtensions;

public class OutlineSpriteCreatorWindow : EditorWindow {

    [SerializeField]
    Sprite currentSprite;
    [SerializeField]
    Gradient outlineGradient;
    [SerializeField]
    int outlineWidth = 1;
    [SerializeField]
    Texture2D currentOutline;
    Texture2D previewOutline;

    OutlineSpriteFactory factory;

    SerializedObject serializedObject;
    SerializedProperty gradientProperty;
    SerializedProperty spriteProperty;
    SerializedProperty widthProperty;

    private void OnEnable()
    {
        outlineGradient = new Gradient();
        outlineGradient.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0), new GradientColorKey(Color.white, 1) }, new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) });
        serializedObject = new SerializedObject(this);
        gradientProperty = serializedObject.FindProperty("outlineGradient");
        widthProperty = serializedObject.FindProperty("outlineWidth");
        spriteProperty = serializedObject.FindProperty("currentSprite");
        factory = new OutlineSpriteFactory();
    }

    [MenuItem("Tools/Outline Sprite Creator")]
    static void Init()
    {
        OutlineSpriteCreatorWindow window = GetWindow<OutlineSpriteCreatorWindow>();
        window.Show();
        window.minSize = new Vector2(300, 500);
    }

    private void OnGUI()
    {
        

        EditorGUILayout.BeginVertical();

        EditorGUI.BeginChangeCheck();

        EditorGUILayout.ObjectField(spriteProperty, new GUIContent("Sprite To Copy"));
        EditorGUILayout.PropertyField(gradientProperty);
        widthProperty.intValue = EditorGUILayout.IntField("Width",widthProperty.intValue);

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }

        if (GUILayout.Button("Create Outline"))
        {
            currentOutline = factory.CreateSpriteOutline(currentSprite, outlineWidth, outlineGradient);
            currentOutline.Apply();
            previewOutline = currentOutline.CreateScaledCopy(256, 256);
            previewOutline.Apply();
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Preview");
        GUIContent content = new GUIContent(previewOutline);
        GUILayout.Box(previewOutline, GUILayout.ExpandWidth(true), GUILayout.Height(256));

        if (currentOutline)
        {
            EditorGUILayout.LabelField("Output Size: " + currentOutline.width + "," + currentOutline.height);
        }
        else
        {
            EditorGUILayout.LabelField("Output Size: NaN");
        }


        EditorGUILayout.Space();

        if (GUILayout.Button("Save Outline"))
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Outline Where?", "New Outline", "png", "Save");
            if (path.Length > 0)
            {
                currentOutline.SaveTexture(path);
                AssetDatabase.Refresh();
            }
        }

        if (GUILayout.Button("Batch Create Outlines from Folder"))
        {
            string spriteFolderPath = EditorUtility.OpenFolderPanel("Select folder with sprites", "", "");
            if (spriteFolderPath.Length > 0)
            {
                Sprite[] sprites = GetAtPath<Sprite>(FileUtil.GetProjectRelativePath(spriteFolderPath));
                if(sprites.Length > 0)
                {
                    System.IO.Directory.CreateDirectory(spriteFolderPath + "/Outlines");
                    EditorUtility.DisplayProgressBar("Sprite Outline Creation", "", 0);
                    for (int i = 0; i < sprites.Length; i++)
                    {
                        Texture2D newTexture = factory.CreateSpriteOutline(currentSprite, outlineWidth, outlineGradient);
                        newTexture.SaveTexture(spriteFolderPath + "/Outlines/" + sprites[i].name + "_Outline.png");
                        EditorUtility.DisplayProgressBar("Sprite Outline Creation", "", (float)(i+1)/sprites.Length);
                    }
                    EditorUtility.ClearProgressBar();
                }
                AssetDatabase.Refresh();
            }
        }

        EditorGUILayout.EndVertical();

        
       
    }

    private T[] GetAtPath<T>(string path) where T : Object
    {
        string globalPath = Application.dataPath + "/" + path.Substring(7);
        List<T> assetsFound = new List<T>();
        string[] fileEntries = Directory.GetFiles(globalPath);

        foreach (string fileName in fileEntries)
        {
            Object t = AssetDatabase.LoadAssetAtPath(FileUtil.GetProjectRelativePath(fileName), typeof(T));

            if (t != null && t is T)
                assetsFound.Add((T)t);
        }

        return assetsFound.ToArray();
    }

}
