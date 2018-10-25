using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

public class TankWeaponCreator : EditorWindow {

    private const string weaponInstancePath = "Assets/Gameplay Assets/Resources/Definitions/Weapons/";

    GUIStyle listStyle;
    GUIStyle listBoxStyle;

    int listTypeSelection = 0;
    int listWeaponSelection = 0;

    TankWeapon[] weapons;
    System.Type[] weaponTypes;

    int newItemType;

    [MenuItem("Tools/Tanks/Weapon Creator")]
    static void Init()
    {
        TankWeaponCreator window = GetWindow<TankWeaponCreator>();
        window.Show();
    }

    private void OnEnable()
    {
        
    }

    private void OnGUI()
    {
        GetAllWeapons();
        GetAllWeaponTypes();


        CreateStyles(); // Move to OnEnable on finish



        EditorGUILayout.BeginHorizontal();
        DrawListView();
        DrawSelected();
        EditorGUILayout.EndHorizontal();
    }

    private void DrawListView()
    {
        EditorGUILayout.BeginVertical(listStyle,GUILayout.Width(220));

        listTypeSelection = GUILayout.Toolbar(listTypeSelection, new string[] { "Weapons", "Projectiles" }, EditorStyles.miniButtonMid);
        EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));

        for (int i = 0; i < weapons.Length; i++)
        {
            if(DrawListBoxItem(weapons[i], listWeaponSelection == i))
            {
                listWeaponSelection = i;
            }

        }
        if (DrawListboxAddition())
        {
            CreateNewWeapon();
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndVertical();
    }

    private void DrawSelected()
    {
        Rect area = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
        if(weapons.Length > 0 && weapons[listTypeSelection] != null)
        {
            Editor editor = Editor.CreateEditor(weapons[listWeaponSelection]);
            editor.DrawDefaultInspector();
        }
        

        EditorGUILayout.EndVertical();
    }

    private void CreateStyles()
    {
        listStyle = new GUIStyle(EditorStyles.helpBox);
        listStyle.padding = new RectOffset(0,1,-1,0);
        listStyle.margin = new RectOffset();

        listBoxStyle = new GUIStyle(EditorStyles.miniLabel);
        listBoxStyle.alignment = TextAnchor.MiddleCenter;
        listBoxStyle.wordWrap = false;
    }

    private bool DrawListBoxItem(TankWeapon weapon, bool selected)
    {
        bool hit = false;
        GUILayout.BeginHorizontal();
        GUIStyle style = listBoxStyle;
        if (selected)
        {
            style = new GUIStyle(EditorStyles.miniButtonMid);
            style.normal = style.onActive;
        }
        hit = GUILayout.Button(weapon.name, style, GUILayout.ExpandWidth(true));

        if(GUILayout.Button("-", EditorStyles.miniButton, GUILayout.ExpandWidth(false)))
        {
            if (EditorUtility.DisplayDialog("Deleting " + weapon.name, "Are you sure you want to delete " + weapon.name + "?", "Yes", "Cancel"))
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(weapon));
                AssetDatabase.SaveAssets();
            }
        }

        GUILayout.EndHorizontal();

        return hit;
    }

    private bool DrawListboxAddition()
    {
        GUILayout.BeginHorizontal();

        string[] names = new string[weaponTypes.Length];
        for (int i = 0; i < weaponTypes.Length; i++)
        {
            names[i] = weaponTypes[i].Name;
        }

        GUILayout.BeginVertical();
        newItemType = EditorGUILayout.Popup(newItemType, names);

        GUILayout.EndVertical();
        bool hit = GUILayout.Button("+", EditorStyles.miniButton, GUILayout.ExpandWidth(false));

        GUILayout.EndHorizontal();
        return hit;
    }

    private void CreateNewWeapon()
    {
        System.Type weaponType = weaponTypes[newItemType];
        TankWeapon newWeapon = (TankWeapon)CreateInstance(weaponType.Name);

        string path = EditorUtility.SaveFilePanel("Save New Weapon", weaponInstancePath, "New " + weaponType.Name, "asset");

        path = FileUtil.GetProjectRelativePath(path);
        if (path.Length > 0)
        {
            AssetDatabase.CreateAsset(newWeapon, path);

            AssetDatabase.SaveAssets();
        }
    }

    private void GetAllWeaponTypes()
    {
        var baseType = typeof(TankWeapon);
        var assembly = baseType.Assembly;

        weaponTypes = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType) || t == baseType).ToArray();
    }

    private void GetAllWeapons()
    {
        weapons = Resources.LoadAll<TankWeapon>("");
    }


}
