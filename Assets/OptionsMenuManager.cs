using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class OptionsMenuManager : MonoBehaviour {

    public SoundSystem soundSystem;

    public Slider musicSlider;
    public Slider sfxSlider;

    public Selectable resolutionSelectable;
    public TextMeshProUGUI resolutionText;

    public Button backButton;
    public Button startButton;

    public Toggle fullScreenToggle;

    private List<Resolution> avaliableResolutions = new List<Resolution>();
    private int resolutionSelection = 0;
    private Resolution selectedResolution;

    private bool wasInput = false;

	public void Start()
    {
        FindAllResolutions();
        SetupOptions();
        backButton.Select();
    }

    public void OnClose()
    {

    }

    public void OnApply()
    {
        soundSystem.SetMusicVolume(musicSlider.value);
        soundSystem.SetSoundVolume(sfxSlider.value);
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullScreenToggle.isOn);
    }

    private void Update()
    {
        InControl.InputDevice input = GameInput.GetPlayerDevice(0);

        if (EventSystem.current.currentSelectedGameObject == resolutionSelectable.gameObject)
        {
            if (input.LeftStickX.Value > 0.3f || input.DPadRight.WasPressed)
            {
                if (!wasInput)
                {
                    SelectNextResolution();
                    wasInput = true;
                }
            }else if(input.LeftStickX.Value < -0.3f || input.DPadLeft.WasPressed)
            {
                if (!wasInput)
                {
                    SelectPrevResolution();
                    wasInput = true;
                }
            }
            else if (wasInput)
            {
                wasInput = false;
            }
        }
    }

    private void FindAllResolutions()
    {
        avaliableResolutions = new List<Resolution>(Screen.resolutions);
    }

    private void SetupOptions()
    {
        musicSlider.value = soundSystem.GetMusicVolume();
        sfxSlider.value = soundSystem.GetSoundVolume();
        UpdateResolutionSelection(FindClosestResolution(avaliableResolutions, out resolutionSelection));
        fullScreenToggle.isOn = Screen.fullScreen;
    }

    public void SelectNextResolution()
    {
        UpdateResolutionSelection(avaliableResolutions[(int)Mathf.Repeat(++resolutionSelection, avaliableResolutions.Count)]);
    }

    public void SelectPrevResolution()
    {
        UpdateResolutionSelection(avaliableResolutions[(int)Mathf.Repeat(++resolutionSelection, avaliableResolutions.Count)]);
    }

    private void UpdateResolutionSelection(Resolution resolution)
    {
        selectedResolution = resolution;
        resolutionText.text = selectedResolution.ToString();
    }

    private Resolution FindClosestResolution(List<Resolution> resolutions, out int index)
    {
        index = 0;
        Resolution closest = resolutions[0];
        float difference = ScoreResolution(closest);
        for (int i = 1; i < resolutions.Count; i++)
        {
            float score = ScoreResolution(resolutions[i]);
            if (score < difference)
            {
                closest = resolutions[i];
                difference = score;
                index = i;
            }
        }
        return closest;
    }

    private float ScoreResolution(Resolution resolution)
    {
        return Mathf.Abs(Screen.width - resolution.width) + Mathf.Abs(Screen.height - resolution.height);
    }
}
