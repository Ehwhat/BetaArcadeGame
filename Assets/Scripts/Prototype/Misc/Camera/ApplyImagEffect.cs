using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class ApplyImagEffect : MonoBehaviour {

    public Material material;
    [Range(0,1)]
    public float amount = 0;

    void Start()
    {
        if (!SystemInfo.supportsImageEffects || null == material ||
           null == material.shader || !material.shader.isSupported)
        {
            enabled = false;
            return;
        }
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat("_Amount", amount);
        Graphics.Blit(source, destination, material);
    }
}
