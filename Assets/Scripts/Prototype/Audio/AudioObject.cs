using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    
}

[CreateAssetMenu(menuName = "Tanks/Audio/AudioObject")]
public class AudioObject : ScriptableObject {

    public static string PITCH_MIN_PROPERTY = "pitchMin";
    public float pitchMin = 1;

    public static string PITCH_MAX_PROPERTY = "pitchMax";
    public float pitchMax = 1;

}
