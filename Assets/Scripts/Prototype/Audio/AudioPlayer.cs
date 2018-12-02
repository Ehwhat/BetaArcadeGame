using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Audio/AudioPlayer")]
public class AudioPlayer : ScriptableObject {

    const int INIT_AMOUNT = 20;

    static GameObject sourceHolder;
    static Queue<AudioSource> avaliableSources = new Queue<AudioSource>();
    static List<AudioSource> takenSources = new List<AudioSource>();

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnInit()
    {
        AllocateSources();
    }

    private static void AllocateSources()
    {
        sourceHolder = new GameObject("Audio Player");
        DontDestroyOnLoad(sourceHolder);
        for (int i = 0; i < INIT_AMOUNT; i++)
        {
            avaliableSources.Enqueue(sourceHolder.AddComponent<AudioSource>());
        }
    }
	
    private static AudioSource GetSource()
    {
        AudioSource source;
        if (avaliableSources.Count > 0)
        {
            source = avaliableSources.Dequeue();
        }
        else
        {
            source = sourceHolder.AddComponent<AudioSource>();
        }
        return source;
    }

    public static void PlayOneOff(AudioObject audio)
    {
        AudioSource source = GetSource();
        source.pitch = audio.Pitch;
        source.volume = audio.Volume;
        source.PlayOneShot(audio.clip);
        CoroutineServer.StartCoroutine(StopAfterClip(audio.clip, source));
    }

    private static IEnumerator StopAfterClip(AudioClip clip, AudioSource source)
    {
        yield return new WaitForSeconds(clip.length);
        avaliableSources.Enqueue(source);
    }

}
