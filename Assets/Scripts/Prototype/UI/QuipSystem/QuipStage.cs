using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuipStage {

    public enum StartType
    {
        StartWithPrevious,
        StartAfterPrevious
    }

    public enum QuipSpeaker
    {
        First,
        Second
    }

    public QuipSpeaker speaker;
    public StartType startType;
    public float startAfterPreviousDelay;

    [TextArea]
    public string quipText;
	
}
