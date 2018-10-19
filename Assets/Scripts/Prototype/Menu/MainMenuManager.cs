﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public Animator animator;

    public GameObject baseMenuScreen;
    public GameObject characterSelectScreen;

    private GameObject openScreen;

    public void StartCharacterSelectScreen()
    {
        animator.SetTrigger("OpenCharacterMenu");
    }

    public void OpenMainMenuScreen()
    {
        animator.SetTrigger("CloseMenu");
    }

    public void StartLevelSelectScreen()
    {
        animator.SetTrigger("OpenLevelSelectMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void StartGame(int level)
    {
        SceneManager.LoadScene(level);
    }

    public void Start()
    {
    }


}