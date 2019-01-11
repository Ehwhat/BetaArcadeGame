using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerSelector : VisualMenuSelector<float> {

    [SerializeField]
    int minimumTime = 2;
    [SerializeField]
    int defaultTime = 5;

    int currentTime;

    public override float GetResult()
    {
        return currentTime;
    }

    public override void NextOption()
    {
        currentTime++;
        DisplayTime();
    }

    public override void PreviousOption()
    {
        currentTime--;
        if(currentTime < minimumTime)
        {
            currentTime = minimumTime;
        }
        DisplayTime();
    }

    private void DisplayTime()
    {
        displayText.text = string.Format("{0:00}:00", currentTime);
    }

    // Use this for initialization
    void Start () {
        currentTime = defaultTime;
        DisplayTime();

    }
	
}
