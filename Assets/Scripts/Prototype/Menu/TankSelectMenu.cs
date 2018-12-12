using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class TankSelectMenu : MonoBehaviour {

    public enum SelectStages
    {
        NotSelected,
        Selected
    }

    public SelectStages currentStage = SelectStages.NotSelected;

    public CharacterDefinitionSet characterDefinitionSet;

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
        input = GameInput.GetPlayerDevice(gamepadIndex);
        ChangeCharacter(0);
        lastInput = -0.5f;
        tankRepresentation.SetColour(playerData.playerColour);
    }
	
	// Update is called once per frame
	void Update () {
		if(currentStage != SelectStages.Selected)
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
            playerData.playerColour = Color.HSVToRGB(currentHue + colourAdjust * Time.deltaTime * 0.3f, 1, 1);
            tankRepresentation.SetColour(playerData.playerColour);


        }
	}


    private void ChangeCharacter(int index)
    {
        currentCharacterIndex = index;
        currentCharacter = characterDefinitionSet.GetDefinition(currentCharacterIndex);
        characterSpriteRenderer.sprite = currentCharacter.portrait;
        currentTank = currentCharacter.defaultTankDefinition;
        tankRepresentation.LoadTankDefinition(currentTank);
    }
}
