using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDestructionScript : MonoBehaviour
{
    AudioSource audiosrc;
    private bool SoundStart = false;
    private AudioClip audioclip_;
    private float pitch_ = 1.0f;
	// Use this for initialization
	void Start () {
        audiosrc = GetComponent<AudioSource>();
        audiosrc.clip = audioclip_;
        audiosrc.pitch = pitch_;
        audiosrc.Play();
    }
	
	// Update is called once per frame
	void Update () {
        if (audiosrc.isPlaying && SoundStart == false)
        {
            Debug.Log("Sound Started");
            SoundStart = true;
        }
        if (!audiosrc.isPlaying && SoundStart == true)
        {
            Debug.Log("Sound destroyed");
            Destroy(gameObject);
        }
	}

    public void SetAudioClip(AudioClip audioclip)
    {
        audioclip_ = audioclip;
    }

    public void SetPitch(float pitch)
    {
        pitch_ = pitch;
    }
}
