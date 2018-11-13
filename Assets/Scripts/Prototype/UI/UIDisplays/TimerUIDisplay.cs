using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimerUIDisplay : MonoBehaviour {

    public TMPro.TextMeshProUGUI text;

    public void SetTimer(object timeInSecondsFloat)
    {
        float time = (float)timeInSecondsFloat;
        string timeString = new DateTime(TimeSpan.FromSeconds(time).Ticks).ToString("mm:ss");
        text.text = timeString;
    }
	
}
