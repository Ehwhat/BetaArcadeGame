using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    private static float shakeAmount;
    public float shakeModifer = 1;
    public Vector2 shakeVectorModifers = new Vector2(1, 1);
    public float shakeDrain;

	void Update () {

        float actualShakeAmount = ((shakeAmount * shakeModifer) * (shakeAmount * shakeModifer));
        float shakeX = shakeVectorModifers.x * actualShakeAmount * (Mathf.PerlinNoise(Time.time, 0)*2)-1;
        float shakeY = shakeVectorModifers.y * actualShakeAmount * (Mathf.PerlinNoise(0, Time.time)*2)-1;

        transform.localPosition = new Vector2(shakeX, shakeY);

        shakeAmount -= shakeDrain * Time.deltaTime;
        shakeAmount = Mathf.Max(0, shakeAmount);
	}

    public static void AddShake(float amount)
    {
        shakeAmount += amount;
    }
}
