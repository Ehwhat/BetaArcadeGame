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
    private int Current_Track_ID;
    private int Current_Track_Time = 0;
    //Maybe for fade ins or something?
    private int Alt_Track_ID;
    private int Alt_Track_Time = 0;

    private bool Switched = false;
    private bool Switchable = false;
    private bool Playing = false;

    public AudioSource Playing_Music;

    public AudioClip[] Music;
    public AudioClip[] Sounds;

    public GameObject soundprefab;

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Start()
    {
         Play_New_Track(0);
        // Play_Sound(0);
        //Play_Dynamic_Tracks(0, 0);
    }

    public void Play_Sound(int Sound_ID)
    {
        GameObject x = Instantiate(soundprefab);
        x.GetComponent<SoundDestructionScript>().audioclip_ = Sounds[Sound_ID];
    }

    public void Stop_Music()
    {
        Playing_Music.Stop();
    }

    public void Play_New_Track(int Track_ID)
    {
        if (Music[Track_ID] != null)
        {
            Playing_Music.clip = Music[Track_ID];
            Playing_Music.Play();
            Playing = true;
            Current_Track_ID = Track_ID;
        }
        else
        {
            Debug.Log("No music found");
            Playing = false;
            //Stop Music
        }
    }

    
    public void Pause_Track()
    {
        Playing_Music.Pause();
    }
    
    public void Unpause_Track()
    {
        Playing_Music.UnPause();
    }
    
    public void Play_Dynamic_Tracks(int Track_ID, int Alt_ID)
    {
        Play_New_Track(Track_ID);
        if (Music[Alt_ID] != null && Playing == true)
        {
            Alt_Track_ID = Alt_ID;
            Switchable = true;
        }
        else
        {
            Debug.Log("No Alternate music found");
            Switchable = false;
        }
        Switched = false;
    }

    //I think this theoretically works?
    //????????????
    //Need to use "Play_Dynamic_Tracks" before this one
    public void Switch_Dynamic_Tracks()
    {
        if (Switchable == true)
        {
            float time = Playing_Music.time;
            if (Switched == true)
            {
                Playing_Music.clip = Music[Current_Track_ID];
                Playing_Music.time = time;
                Playing_Music.Play();
            }
            else
            {
                Playing_Music.clip = Music[Alt_Track_ID];
                Playing_Music.time = time;
                Playing_Music.Play();
            }
            Switched = !Switched;
        }
    }
}