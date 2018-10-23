using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using InControl;

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
    public int player;
    public CharacterDefinitionSet characterSet;
    public GameDataDefinition gameDataDefinition;

    private int currentCharacterIndex = 0;
    private CharacterDefinition activeDefinition;
    private GameObject currentTankPrefab;

    public CharacterSelectStates isSelected = CharacterSelectStates.NotJoined;

    private float selectionDelay = 0.3f;
    private float lastSelectionTime = 0;

    private InputDevice device;

    // Use this for initialization
    void Start () {
        LoadDefinition(currentCharacterIndex);
        LoadActiveDefinitionPrefab();
        device = GameInput.GetPlayerDevice(player);
    }

    private void OnEnable()
    {

        for (int i = 0; i < 4; i++)
        {
            gameDataDefinition.SetPlayerJoined(i, false);
        }
        
        if (player != 0)
        {
            joinScreen.SetActive(true);
        }else
        {
            isSelected = CharacterSelectStates.Selecting;
        }
    }

    // Update is called once per frame
    void Update () {
        if (device == null || !device.active)
        {
            return;
        }
        
        bool pressingLeft = device.LeftStick.Left.IsPressed || device.DPadLeft.IsPressed;
        bool pressingRight = device.LeftStick.Right.IsPressed || device.DPadRight.IsPressed;
        bool select = device.Action1.WasPressed;
        bool cancel = device.Action2.WasPressed;

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

            if (cancel && isSelected == CharacterSelectStates.Selecting && player == 0)
            {
                mainMenu.OpenMainMenuScreen();
            }else if(cancel && isSelected == CharacterSelectStates.Selecting)
            {
                isSelected = CharacterSelectStates.NotJoined;
                joinScreen.SetActive(true);
            }

            if(select && isSelected == CharacterSelectStates.Selected && player == 0)
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
