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

    private float Music_Volume = 1.0f;
    private float Sound_Volume = 1.0f;

    private bool Switched = false;
    private bool Switchable = false;
    private bool Playing = false;

    public AudioSource Playing_Music;

    public AudioClip[] Music;
    //public AudioClip[] Sounds;

    public GameObject soundprefab;

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void Start()
    {
         Play_New_Track(0);
    }

    public void Play_Sound(AudioObject audio)
    {
        GameObject x = Instantiate(soundprefab);
        x.GetComponent<SoundDestructionScript>().SetAudioClip(audio.clip);
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
            Playing_Music.Stop();
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

    //Volume is between 0.0f and 1.0f. Only new sounds have their sound adjusted.
    public void SetSoundVolume(float NewVolume)
    {
        if (NewVolume < 0.0f)
        {
            NewVolume = 0.0f;
        }
        if (NewVolume > 1.0f)
        {
            NewVolume = 1.0f;
        }
        Sound_Volume = NewVolume;
        soundprefab.GetComponent<AudioSource>().volume = NewVolume;
    }

    //Volume is between 0.0f and 1.0f
    public void SetMusicVolume(float NewVolume)
    {
        if (NewVolume < 0.0f)
        {
            NewVolume = 0.0f;
        }
        if (NewVolume > 1.0f)
        {
            NewVolume = 1.0f;
        }
        Music_Volume = NewVolume;
        Playing_Music.volume = NewVolume;
    }

    public float GetMusicVolume()
    {
        return Music_Volume;
    }

    public float GetSoundVolume()
    {
        return Sound_Volume;
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

    //The sound played has a random pitch
    public void Play_Random_Pitch_Sound(AudioObject audio, float range)
    {
        //
        if (range < 3.0f)
        {
            range = 3.0f;
        }
        if (range > 6.0f)
        {
            range = 6.0f;
        }
        GameObject x = Instantiate(soundprefab);
        x.GetComponent<SoundDestructionScript>().SetAudioClip(audio.clip);
        float randx = Random.Range(0.1f, range - 3.0f);
        x.GetComponent<SoundDestructionScript>().SetPitch(randx);
    }
}