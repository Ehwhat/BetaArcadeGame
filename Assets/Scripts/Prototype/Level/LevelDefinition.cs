using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Levels/New Level Definition")]
public class LevelDefinition : ScriptableObject {

    public string levelName;
    public Sprite levelImage;
    public int id;

}
