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

    public float volumeMin = 1;
    public float volumeMax = 1;

    public float Pitch
    {
        get { return Random.Range(pitchMin, pitchMax); }
    }

    public float Volume
    {
        get { return Random.Range(volumeMin, volumeMax); }
    }

    public AudioClip clip;

}
