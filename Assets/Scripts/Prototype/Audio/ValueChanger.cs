using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueChanger : MonoBehaviour {
    public Slider musicslider;
    public Slider soundslider;
    public SoundSystem soundsystem;
	// Use this for initialization
	void Start () {
        musicslider.value = soundsystem.GetMusicVolume();
        soundslider.value = soundsystem.GetSoundVolume();

	}
	
	// Update is called once per frame
	void Update () {

	}
}
