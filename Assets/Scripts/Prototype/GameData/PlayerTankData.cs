using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/Players/New PlayerTankData")]
public class PlayerTankData : ScriptableObject {



    public System.Action onChangedEvent = () => { };

    public Color defaultplayerColour;
    public QuipSystemDefinition quipSystem;

    public bool isInGame { get; private set; }
    public float currentHealthPercentage { get; private set; }
    public List<float> currentTurretConditions { get; private set; }

    public void SetHealthPercentage(float healthPercentage)
    {
        currentHealthPercentage = Mathf.Clamp01(healthPercentage);
        onChangedEvent();
    }

    public void SetIsInGame(bool isInGame)
    {
        this.isInGame = isInGame;
        onChangedEvent();
    }

}
