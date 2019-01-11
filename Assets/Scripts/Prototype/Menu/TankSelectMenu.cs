﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using InControl;

public class TankSelectMenu : MonoBehaviour {

    public enum SelectStages
    {
        NotJoined,
        NotSelected,
        Selected
    }

    public SelectStages currentStage = SelectStages.NotJoined;

    public MainMenuManager mainMenuManager;
    public CharacterDefinitionSet characterDefinitionSet;
    public GameDataDefinition gameData;
    public HueBarManager huebar;
    public Animator animator;

    public TextMeshPro characterNameText;

    public PlayerTankData playerData;
    public int gamepadIndex = 0;

    public SpriteRenderer characterSpriteRenderer;
    public TankRepresentation tankRepresentation;

    public TankDefinition currentTank;
    public CharacterDefinition currentCharacter;

    private int currentCharacterIndex = 0;
    private float lastInput = 0;
    InputDevice input;

	// Use this for initialization
	void Start () {
        
        ChangeCharacter(0);
        playerData.playerColour = playerData.defaultplayerColour;
        lastInput = -0.5f;
        tankRepresentation.SetColour(playerData.playerColour);
    }

    private void OnEnable()
    {
        Reset();
    }

    // Update is called once per frame
    void Update () {
        input = GameInput.GetPlayerDevice(gamepadIndex);
        if (input == null)
            return;


        if (currentStage == SelectStages.NotJoined){
            if (input.Action1.WasPressed)
            {
                currentStage = SelectStages.NotSelected;
                playerData.SetIsInGame(true);
                gameData.SetPlayerJoined(gamepadIndex, true);
                animator.SetBool("IsJoined", true);

            }
        }
		else if(currentStage == SelectStages.Selected)
        {
            if (gamepadIndex == 0 && input.Action1.WasPressed)
            {
                Debug.Log("test");
                mainMenuManager.StartLevelSelectScreen();
            }else if (input.Action2.WasPressed)
            {
                currentStage = SelectStages.NotSelected;
                animator.SetBool("IsDone", false);
               
            }

        }
        else
        {

            bool left = input.LeftStick.Left.IsPressed;
            bool right = input.LeftStick.Right.IsPressed;
            float colourAdjust = input.RightTrigger.Value - input.LeftTrigger.Value;

            if (Time.time - lastInput >= 0.5)
            {
                if (left)
                {
                    ChangeCharacter((int)Mathf.Repeat(currentCharacterIndex - 1, characterDefinitionSet.Count));
                    lastInput = Time.time;
                }
                else if (right)
                {
                    ChangeCharacter((int)Mathf.Repeat(currentCharacterIndex + 1, characterDefinitionSet.Count));
                    lastInput = Time.time;
                }
                else
                {
                    lastInput = -0.5f;
                }
            }

            float currentHue = 0;
            float currentS, currentV = 0;
            Color.RGBToHSV(playerData.playerColour, out currentHue, out currentS, out currentV);
            currentHue = currentHue + colourAdjust * Time.deltaTime * 0.3f;
            playerData.playerColour = Color.HSVToRGB(currentHue, 1, 1);
            huebar.SetSelector(currentHue%1, Color.HSVToRGB(currentHue, 1, 1));

            tankRepresentation.SetColour(playerData.playerColour);

            if (input.Action1.WasPressed)
            {
                currentStage = SelectStages.Selected;
                playerData.selectedCharacter = currentCharacter;
                playerData.selectedTank = currentTank;
                animator.SetBool("IsDone", true);
                Debug.Log("yes");
            }else if (input.Action2.WasPressed)
            {
                currentStage = SelectStages.NotJoined;
                playerData.SetIsInGame(false);
                gameData.SetPlayerJoined(gamepadIndex, false);
                animator.SetBool("IsJoined", false);
            }

        }
        
	}

    public void Reset()
    {
        currentStage = SelectStages.NotJoined;
        animator.SetBool("IsDone", false);
        playerData.SetIsInGame(false);
        gameData.SetPlayerJoined(gamepadIndex, false);
        animator.SetBool("IsJoined", false);
    }


    private void ChangeCharacter(int index)
    {
        currentCharacterIndex = index;
        currentCharacter = characterDefinitionSet.GetDefinition(currentCharacterIndex);
        characterSpriteRenderer.sprite = currentCharacter.portrait;
        currentTank = currentCharacter.defaultTankDefinition;
        tankRepresentation.LoadTankDefinition(currentTank);
        characterNameText.text = currentCharacter.name;
    }
}
