using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour {

    public PlayerTankData data;
    public Image healthBar;

    public void Start()
    {
        data.onChangedEvent += OnDataChanged;
        OnDataChanged();
    }

    private void OnDataChanged()
    {
        gameObject.SetActive(data.isInGame);
        if (data.isInGame)
        {
            healthBar.fillAmount = data.currentHealthPercentage;
        }
    }

}
