using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrailRenderer : MonoBehaviour {

    public AudioSource source;
    new TrailRenderer renderer;


    private void Start()
    {
        renderer = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        int sampleNumber = 64;
        float[] samples = new float[sampleNumber];
        source.GetOutputData(samples, 0);

        Gradient gradient = new Gradient();
        AnimationCurve curve = new AnimationCurve();

        List<GradientColorKey> colours = new List<GradientColorKey>();

        for (float i = 0; i < sampleNumber; i++)
        {

            float val = ((samples[(int)i] + 1) * 0.5f);
            curve.AddKey((1.0f / sampleNumber) * i, val);
        }

        gradient.colorKeys = colours.ToArray();

        renderer.widthCurve = curve;
        

    }

}
