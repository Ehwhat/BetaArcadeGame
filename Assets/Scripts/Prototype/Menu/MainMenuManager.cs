using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

    public Animator animator;

    public GameObject baseMenuScreen;
    public GameObject characterSelectScreen;
    public GameObject optionsScreen;
    public GameObject Menutest;

    public Button Back;
    public Button GameStart;

    private GameObject openScreen;

    private void Update()
    {
       // Debug.Log(baseMenuScreen.activeSelf);
    }

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

    public void LoadOptionsScreen()
    {
        Menutest.SetActive(false);
        optionsScreen.SetActive(true);
        Back.Select();
    }

    public void CloseOptionsScreen()
    {
        optionsScreen.SetActive(false);
        Menutest.SetActive(true);
        GameStart.Select();
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
