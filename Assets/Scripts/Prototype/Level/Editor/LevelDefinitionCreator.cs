using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

[CreateAssetMenu(menuName = "Tanks/Singletons/New Level Creator")]
public class LevelDefinitionCreator : ScriptableObject {

    public GameObject[] requiredPrefabs;
    public static LevelDefinitionCreator instance;
    public string levelFolderPath = "/Scenes/";

    private void OnEnable()
    {
        Debug.Log("Created Level Creator");
        instance = this;
    }

    [MenuItem("Create New Level",menuItem = "Tools/Tanks/Create New Level")]
    private static void CreateLevelDefinition()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Level", "New Level", "asset", "Save Level To Here");
        if (path.Length >= 0) {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SceneManager.SetActiveScene(scene);
            for (int i = 0; i < instance.requiredPrefabs.Length; i++)
            {
                Instantiate(instance.requiredPrefabs[i]);
            }
            EditorSceneManager.SaveScene(scene, instance.levelFolderPath);

            LevelDefinition definition = CreateInstance<LevelDefinition>();
            string filename = Path.GetFileNameWithoutExtension(path);
            
        }
    }

}
