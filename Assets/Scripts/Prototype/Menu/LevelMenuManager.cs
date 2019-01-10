using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using InControl;

public class LevelMenuManager : MonoBehaviour {

    public TextMeshProUGUI levelName;
    public Image levelImage;

    public Animation cameraAnimation;

    public MainMenuManager mainMenu;
    public LevelDefinitionSet levelSet;
    public Animator animationController;


    private float selectionDelay = 0.3f;
    private float lastSelectionTime = 0;
    int currentLevelIndex = 0;
    bool starting = false;

    LevelDefinition activeDefinition;
    InputDevice device;

    void Start()
    {
        LoadDefinition(currentLevelIndex);
        device = GameInput.GetPlayerDevice(0);
        LoadLevelImage();


    }

    private void Update()
    {
        if (!starting)
        {
            bool pressingLeft = device.LeftStick.Left.IsPressed || device.DPadLeft.IsPressed;
            bool pressingRight = device.LeftStick.Right.IsPressed || device.DPadRight.IsPressed;
            bool select = device.Action1.WasPressed;
            bool cancel = device.Action2.WasPressed;

            if (lastSelectionTime + selectionDelay < Time.time)
            {
                if (pressingLeft)
                {
                    SelectLeft();
                    lastSelectionTime = Time.time;
                }
                else if (pressingRight)
                {
                    SelectRight();
                    lastSelectionTime = Time.time;
                }
            }

            if (select)
            {
                mainMenu.StartGame(activeDefinition.id);
            }
            else if (cancel)
            {
                mainMenu.OpenMainMenuScreen();
            }
        }

    }

    public void SelectLeft()
    {
        currentLevelIndex--;
        if (currentLevelIndex < 0)
        {
            currentLevelIndex = levelSet.Count - 1;
        }
        LoadDefinition(currentLevelIndex);
        animationController.SetTrigger("OnLeft");
    }

    public void SelectRight()
    {
        currentLevelIndex++;
        if (currentLevelIndex > levelSet.Count - 1)
        {
            currentLevelIndex = 0;
        }
        LoadDefinition(currentLevelIndex);
        animationController.SetTrigger("OnRight");
    }

    private void LoadDefinition(int levelIndex)
    {
        activeDefinition = levelSet.GetDefinition(levelIndex);
        levelName.text = activeDefinition.levelName;
        
    }

    public void LoadLevelImage()
    {
        if (activeDefinition.levelImage)
        {
            levelImage.enabled = true;
            levelImage.sprite = activeDefinition.levelImage;
            levelImage.preserveAspect = true;
        }
        else
        {
            levelImage.enabled = false;
        }
    }

    private IEnumerator StartGame(int levelIndex)
    {
        cameraAnimation.Play();
        yield return new WaitForSeconds(0.4f);
        mainMenu.StartGame(levelIndex);
    }


}
