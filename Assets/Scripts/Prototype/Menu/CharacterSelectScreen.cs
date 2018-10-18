using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using XInputDotNetPure;

public class CharacterSelectScreen : MonoBehaviour {

    public enum CharacterSelectStates
    {
        NotJoined,
        Selecting,
        Selected
    }

    [Header("UI")]
    public GameObject joinScreen;
    public MainMenuManager mainMenu;
    public TextMeshProUGUI characterName;
    public Transform tankHolder;
    public Animator animationController;

    [Space(10)]
    [Header("Setup")]
    public PlayerIndex player;
    public CharacterDefinitionSet characterSet;
    public GameDataDefinition gameDataDefinition;

    private int currentCharacterIndex = 0;
    private CharacterDefinition activeDefinition;
    private GameObject currentTankPrefab;

    public CharacterSelectStates isSelected = CharacterSelectStates.NotJoined;

    private float selectionDelay = 0.3f;
    private float lastSelectionTime = 0;
    GamePadState lastState;

    // Use this for initialization
    void Start () {
        LoadDefinition(currentCharacterIndex);
        LoadActiveDefinitionPrefab();
    }

    private void OnEnable()
    {
        lastState = GamePad.GetState(player);

        for (int i = 0; i < 4; i++)
        {
            gameDataDefinition.SetPlayerJoined(i, false);
        }
        
        if (player != PlayerIndex.One)
        {
            joinScreen.SetActive(true);
        }else
        {
            isSelected = CharacterSelectStates.Selecting;
        }
    }

    // Update is called once per frame
    void Update () {


        GamePadState state = GamePad.GetState(player);

        
        bool pressingLeft = (state.ThumbSticks.Left.X < -0.5f) || state.DPad.Left == ButtonState.Pressed;
        bool pressingRight = (state.ThumbSticks.Left.X > 0.5f) || state.DPad.Right == ButtonState.Pressed;
        bool select = state.Buttons.A == ButtonState.Pressed && lastState.Buttons.A == ButtonState.Released;
        bool cancel = state.Buttons.B == ButtonState.Pressed && lastState.Buttons.B == ButtonState.Released;

        if (isSelected != CharacterSelectStates.NotJoined)
        {

            if (lastSelectionTime + selectionDelay < Time.time && isSelected == CharacterSelectStates.Selecting)
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

            if (cancel && isSelected == CharacterSelectStates.Selecting && player == PlayerIndex.One)
            {
                mainMenu.OpenMainMenuScreen();
            }else if(cancel && isSelected == CharacterSelectStates.Selecting)
            {
                isSelected = CharacterSelectStates.NotJoined;
                joinScreen.SetActive(true);
            }

            if(select && isSelected == CharacterSelectStates.Selected && player == PlayerIndex.One)
            {
                mainMenu.StartLevelSelectScreen();
            }

            if (select && isSelected == CharacterSelectStates.Selecting)
            {
                gameDataDefinition.SetCharacter((int)player, activeDefinition);
                gameDataDefinition.SetPlayerJoined((int)player, true);
                isSelected = CharacterSelectStates.Selected;
                animationController.SetTrigger("OnAccept");
            }
            else if (cancel && isSelected == CharacterSelectStates.Selected)
            {
                gameDataDefinition.SetPlayerJoined((int)player, false);
                isSelected = CharacterSelectStates.Selecting;
                animationController.SetTrigger("OnCancel");
            }
        }
        else
        {
            if (select)
            {
                isSelected = CharacterSelectStates.Selecting;
                joinScreen.SetActive(false);
            }
        }

       

        lastState = state;

    }

    public void SelectLeft()
    {
        currentCharacterIndex--;
        if(currentCharacterIndex < 0)
        {
            currentCharacterIndex = characterSet.Count - 1;
        }
        LoadDefinition(currentCharacterIndex);
        animationController.SetTrigger("OnLeft");
    }

    public void SelectRight()
    {
        currentCharacterIndex++;
        if (currentCharacterIndex > characterSet.Count-1)
        {
            currentCharacterIndex = 0;
        }
        LoadDefinition(currentCharacterIndex);
        animationController.SetTrigger("OnRight");
    }

    public void LoadDefinition(int definitionIndex)
    {
        if (definitionIndex >= 0 && definitionIndex < characterSet.Count)
        {
            CharacterDefinition definition = characterSet.GetDefinition(definitionIndex);
            characterName.text = definition.name;
            activeDefinition = definition;
        }
    }

    public void LoadActiveDefinitionPrefab()
    {
        Debug.Log("test");
        if (currentTankPrefab)
        {
            Destroy(currentTankPrefab);
        }
        GameObject tank = Instantiate(activeDefinition.tankDisplayPrefab, tankHolder);
        currentTankPrefab = tank;
    }
}
