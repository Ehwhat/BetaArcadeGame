using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TankWeapon), true)]
public class TankWeaponEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TankWeapon weapon = (TankWeapon)target;
        float coefficent = weapon.CalculateWeaponCoefficent();

        if (!weapon.useDuribility)
        {
            EditorGUILayout.HelpBox("This weapon does not use durability, and as such has an inaccurate coefficent!", MessageType.Warning);
        }

        string coeffientMessage = "This weapon's coefficent is " + coefficent;
        
        EditorGUILayout.HelpBox(coeffientMessage, MessageType.Info);
    }

}
