using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundSystem : MonoBehaviour
{

    /*  - Be able to play sounds from anywhere (Maybe using static functions / singleton pattern?)
        - Needs to be able to reference what sound to play (either using an array with IDs or just passing them in whatever the play function is)
        - Sounds should be able to have their volume and pitch set, if possible with min-max ranges (Maybe make a sound scriptable object for storing those along with the clip?)
        - Oh! and a system for soundtracks as well as one off sounds*/
    //Random pitch modulation

    //might just be a music thingy rather than an int, but as an idea
    private int Current_Track_ID = 0;
    private int Current_Track_Time = 0;
    //Maybe for fade ins or something?
    private int Alt_Track_ID;
    private int Alt_Track_Time = 0;

    private bool Current_Exists = false;
    private bool Alt_Exists = false;
    private bool Paused;
    private bool Switched;

    public AudioSource Playing_Music;

    public AudioClip[] Music;
    public AudioClip[] Sounds;

    public GameObject soundprefab;

    public void Start()
    {
        Play_Sound(0);
    }

    public void Play_Sound(int Sound_ID)
    {
        GameObject x = Instantiate(soundprefab);
        x.GetComponent<SoundDestructionScript>().audioclip_ = Sounds[Sound_ID];
        //create Sound thing
        //access Sound and apply it
        //play Sound
    }

   /* public void Stop_Music()
    {
        Current_Exists = false;
        Alt_Exists = false;
    }*/

    public void Play_New_Track(int Track_ID)
    {
        if (Music[Track_ID] != null)
        {
            Playing_Music.clip = Music[Track_ID];
            Current_Exists = true;
            Alt_Exists = false;
            Playing_Music.Play();
        }
        else
        {
            Debug.Log("No music found");
        }
    }
    /*
    public void Pause_Track()
    {
        Paused = true;
    }

    public void Unpause_Track()
    {
        Paused = false;
    }

    public void Play_Dynamic_Tracks(int Track_ID, int Alt_Track_ID)
    {
        Current_Exists = true;
        Alt_Exists = true;
    }

    public void Switch_Dynamic_Tracks()
    {
        Switched = !Switched;
    }*/
}