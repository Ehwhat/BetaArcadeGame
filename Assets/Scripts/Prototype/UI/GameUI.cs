using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour {

    public Transform gamemodeUIHolder;

    public T SpawnGamemodeUI<T>(T gamemodeUI) where T : Object
    {
        return Instantiate(gamemodeUI, gamemodeUIHolder);
    }
	
}
