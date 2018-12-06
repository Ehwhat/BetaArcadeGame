using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

    private static float shakeAmount;
    public float shakeModifer = 1;
    public Vector2 shakeVectorModifers = new Vector2(1, 1);
    public float shakeDrain;
    public static Vector2 newShakeAmount;
    public bool changeToNew;

	void Update () {
        if (changeToNew)
        {
            if (newShakeAmount.x != 0 || newShakeAmount.y != 0)
            {
                newShake();
            }
        }
        else
        {
            oldShake();
        }
	}

    public static void AddShake(float amount, Quaternion rotation)
    {
        shakeAmount += amount;
        amount *= 0.1f; //shake modifier 
        float rot = rotation.eulerAngles.z;
        if (rot > 337.5 || rot <= 22.5)
        {
            newShakeAmount.y -= amount;
            //print("up");
        }
        else if (rot <= 67.5)
        {
            newShakeAmount.x += (amount * 0.71f);
            newShakeAmount.y -= (amount * 0.71f);
            //print("up left");
        }
        else if (rot <= 112.5)
        {
            newShakeAmount.x -= amount;
            //print("left");
        }
        else if (rot <= 157.5)
        {
            newShakeAmount.x += (amount * 0.71f);
            newShakeAmount.y += (amount * 0.71f);
            //print("down left");
        }
        else if (rot <= 202.5)
        {
            newShakeAmount.y += amount;
            //print("down");
        }
        else if (rot <= 247.5)
        {
            newShakeAmount.x -= (amount * 0.71f);
            newShakeAmount.y += (amount * 0.71f);
            //print("down right");
        }
        else if (rot <= 292.5)
        {
            newShakeAmount.x += amount;
            //print("right");
        }
        else
        {
            newShakeAmount.x -= (amount * 0.71f);
            newShakeAmount.y -= (amount * 0.71f);
            //print("up right");
        }
    }

    private void oldShake()
    {
        float actualShakeAmount = ((shakeAmount * shakeModifer) * (shakeAmount * shakeModifer) * (shakeAmount * shakeModifer));
        float shakeX = shakeVectorModifers.x * actualShakeAmount * (Mathf.PerlinNoise(Time.time, 0) * 2) - 1;
        float shakeY = shakeVectorModifers.y * actualShakeAmount * (Mathf.PerlinNoise(0, Time.time) * 2) - 1;

        transform.localPosition = new Vector2(shakeX, shakeY);

        shakeAmount -= shakeDrain * Time.deltaTime;
        shakeAmount = Mathf.Max(0, shakeAmount);
    }

    private void newShake()
    {
        if (newShakeAmount.x <= 0.1 && newShakeAmount.x >= -0.1)
        {
            newShakeAmount.x = 0;
        }
        else
        {
            if (newShakeAmount.x > 0)
            {
                newShakeAmount.x *= 0.9f;
            }
            else
            {
                newShakeAmount.x *= 0.9f;
            }
        }
        if (newShakeAmount.y <= 0.1 && newShakeAmount.y >= -0.1)
        {
            newShakeAmount.y = 0;
        }
        else
        {
            if (newShakeAmount.y > 0)
            {
                newShakeAmount.y *= 0.9f;
            }
            else
            {
                newShakeAmount.y *= 0.9f;
            }
        }
        /*float shakeX = newShakeAmount.x + transform.localPosition.x;
        float shakeY = newShakeAmount.y + transform.localPosition.y;
        transform.localPosition = new Vector2(shakeX, shakeY);*/
        transform.localPosition = newShakeAmount;

        /*shakeAmount -= shakeDrain * Time.deltaTime;
        shakeAmount = Mathf.Max(0, shakeAmount);*/
    }
}
