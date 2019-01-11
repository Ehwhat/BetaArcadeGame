using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using InControl;

public class LevelMenuManager : MonoBehaviour {

    public SimpleDeathmatchGamemodeDefinition deathmatchGamemode;

    public MainMenuManager mainMenu;
    public LevelDefinitionSet levelSet;

    private float selectionDelay = 0.3f;
    private float lastSelectionTime = 0;
    bool starting = false;

    public LevelMenuSelector levelMenuSelector;
    public TimerSelector timerSelector;
    public MenuSelector currentSelector;

    LevelDefinition activeDefinition;
    InputDevice device;

    void Start()
    {
        device = GameInput.GetPlayerDevice(0);
        currentSelector.OnSelected();
    }

    private void Update()
    {
        if (!starting)
        {
            bool pressingLeft = device.LeftStick.X < -0.5f || device.DPadLeft.IsPressed;
            bool pressingRight = device.LeftStick.X > 0.5f || device.DPadRight.IsPressed;
            bool pressingUp = device.LeftStick.Y < -0.5f || device.DPadUp.IsPressed;
            bool pressingDown = device.LeftStick.Y > 0.5f || device.DPadDown.IsPressed;
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
                }else if (pressingDown)
                {
                    currentSelector.OnDeselected();
                    currentSelector = currentSelector.nextSelector;
                    currentSelector.OnSelected();
                    lastSelectionTime = Time.time;
                }
                else if (pressingUp)
                {
                    currentSelector.OnDeselected();
                    currentSelector = currentSelector.previousSelector;
                    currentSelector.OnSelected();
                    lastSelectionTime = Time.time;
                }
            }

            if (select)
            {
                deathmatchGamemode.timerMinutes = timerSelector.GetResult();
                mainMenu.StartGame(levelMenuSelector.GetResult().id);
            }
            else if (cancel)
            {
                mainMenu.OpenMainMenuScreen();
            }
        }

    }

    public void LoadLevelImage()
    {
        levelMenuSelector.LoadLevelImage();
    }

    public void SelectLeft()
    {
        currentSelector.PreviousOption();
    }

    public void SelectRight()
    {
        currentSelector.NextOption();
    }


}
