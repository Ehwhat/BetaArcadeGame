using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour {

    public PlayerTankData data;
    public Image healthBar;
    public Image potrait;
    public Image frame;
    public int playerIndex = 0;

    public void Start()
    {
        potrait.sprite = GameDataMonobehaviour.instance.selectedCharacter[playerIndex].headPortrait;
        frame.color = GameDataMonobehaviour.instance.playerColour[playerIndex];
        data.onChangedEvent += OnDataChanged;
        OnDataChanged();
    }

    public void OnDisable()
    {
        data.onChangedEvent -= OnDataChanged;
    }

    private void OnDataChanged()
    {
        gameObject.SetActive(GameDataMonobehaviour.instance.playersJoined[playerIndex]);
        if (GameDataMonobehaviour.instance.playersJoined[playerIndex])
        {
            healthBar.fillAmount = data.currentHealthPercentage;
        }
    }

}
