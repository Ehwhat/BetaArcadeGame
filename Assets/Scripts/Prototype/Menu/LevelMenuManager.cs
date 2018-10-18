using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;
using TMPro;

public class LevelMenuManager : MonoBehaviour {

    public TextMeshProUGUI levelName;

    public MainMenuManager mainMenu;
    public LevelDefinitionSet levelSet;
    public Animator animationController;

    private GamePadState lastState;

    private float selectionDelay = 0.3f;
    private float lastSelectionTime = 0;
    int currentLevelIndex = 0;

    LevelDefinition activeDefinition;

    void Start()
    {
        LoadDefinition(currentLevelIndex);
    }

    private void Update()
    {
        GamePadState state = GamePad.GetState(PlayerIndex.One);

        bool pressingLeft = (state.ThumbSticks.Left.X < -0.5f) || state.DPad.Left == ButtonState.Pressed;
        bool pressingRight = (state.ThumbSticks.Left.X > 0.5f) || state.DPad.Right == ButtonState.Pressed;
        bool select = state.Buttons.A == ButtonState.Pressed && lastState.Buttons.A == ButtonState.Released;
        bool cancel = state.Buttons.B == ButtonState.Pressed && lastState.Buttons.B == ButtonState.Released;

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
        }else if (cancel)
        {
            mainMenu.OpenMainMenuScreen();
        }


        lastState = state;
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


}
