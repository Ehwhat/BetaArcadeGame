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

    private bool isDead = false;
    private bool isFinishedBeingDead = false;
    public Material playerDeadMaterial;

    public void Start()
    {
        if (!GameDataMonobehaviour.instance.IsPlayerJoined(playerIndex))
        {
            gameObject.SetActive(false);
            return;
        }
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
            if(data.currentHealthPercentage <= 0 && !isDead)
            {
                OnPlayerDied();
                isDead = true;
                isFinishedBeingDead = false;
            }
            else if(isDead && isFinishedBeingDead)
            {
                isDead = false;
            }
        }
    }

    IEnumerator PlayerDiedCoroutine(float from, float to, bool stayDead = false, float transition = 1f, float duration = 1f)
    {
        float timeElapsed = 0;
        playerDeadMaterial.SetFloat("_Amount", from);
        while (timeElapsed < transition)
        {
            playerDeadMaterial.SetFloat("_Amount", Mathf.Lerp(from, to, timeElapsed / transition));
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        playerDeadMaterial.SetFloat("_Amount", to);
        if (!stayDead)
        {
            yield return new WaitForSeconds(duration);
            timeElapsed = 0;
            while (timeElapsed < transition)
            {
                playerDeadMaterial.SetFloat("_Amount", Mathf.Lerp(to, from, timeElapsed / transition));
                timeElapsed += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            playerDeadMaterial.SetFloat("_Amount", from);
            isFinishedBeingDead = true;
        }
    }

    [ContextMenu("test")]
    public void OnPlayerDied()
    {
        StartCoroutine(PlayerDiedCoroutine(0, 1));
    }

}
