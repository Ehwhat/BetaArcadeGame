using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour {

    public GameObject baseMenuScreen;
    public GameObject characterSelectScreen;

    public void StartCharacterSelectScreen()
    {
        baseMenuScreen.SetActive(false);
        characterSelectScreen.SetActive(true);
    }

    public void CloseCharacterSelectScreen()
    {
        baseMenuScreen.SetActive(true);
        characterSelectScreen.SetActive(false);
    }

    public void StartGame()
    {

    }

    public void Start()
    {
    }


}
