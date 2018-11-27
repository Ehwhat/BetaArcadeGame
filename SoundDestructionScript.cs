using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDestructionScript : MonoBehaviour
{
    AudioSource audiosrc;
    private bool SoundStart = false;
    public AudioClip audioclip_;
	// Use this for initialization
	void Start () {
        audiosrc = GetComponent<AudioSource>();
        audiosrc.clip = audioclip_;
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
}
