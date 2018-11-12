using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

[CreateAssetMenu(menuName = "Tanks/Singletons/New Game Input Manager")]
public class GameInput : ScriptableObject {

    private static InputDevice[] gameDevices;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        InputManager.OnDeviceAttached += OnDeviceAttached;
        InputManager.OnDeviceDetached += OnDeviceDetached;
        gameDevices = new InputDevice[4];
    }

    private static void OnDeviceDetached(InputDevice obj)
    {
        Debug.Log("Controller " + obj.Name + " Removed");
        for (int i = 0; i < 4; i++)
        {
            if(obj == gameDevices[i])
            {
                gameDevices[i].active = false;
                return;
            }
        }
    }

    private static void OnDeviceAttached(InputDevice obj)
    {
        Debug.Log("Controller " + obj.Name + " Detected");
        for (int i = 0; i < 4; i++)
        {
            InputDevice currentDevice = gameDevices[i];
            if(currentDevice == null || (!currentDevice.active && obj.Name == currentDevice.Name && obj.Meta == currentDevice.Meta))
            {
                gameDevices[i] = obj;
                gameDevices[i].active = true;
                return;
            }
        }
    }

    public static bool IsPlayerEnabled(int i)
    {
        return gameDevices[i] == null || gameDevices[i].active;
    }

    public static InputDevice GetPlayerDevice(int i)
    {
        return gameDevices[i];
    }
	
}
