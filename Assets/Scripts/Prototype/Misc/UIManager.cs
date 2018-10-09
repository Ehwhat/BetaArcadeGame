using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public InputField speedInput;
    public InputField rotationInput;
    public InputField forwardsInput;
    public InputField sidewaysInput;
    public InputField reverseInput;

    public Slider speedSlider;
    public Slider rotationSlider;
    public Slider forwardsSlider;
    public Slider sidewaysSlider;
    public Slider reverseSlider;

    public TankMovement move;
    RectTransform rectTransform;
    Vector3 target;

	// Use this for initialization
	void Start () {
        rectTransform = GetComponent<RectTransform>();
        target = new Vector3(0, 270, 0);

    }
	
	// Update is called once per frame
	void Update () {
        rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, target, 30);


    }

    private void UpdateDisplays()
    {
        speedInput.text = move.speedModifer.ToString("0.0");
        speedSlider.value = move.speedModifer;

        rotationInput.text = move.rotationSpeed.ToString("0.0");
        rotationSlider.value = move.rotationSpeed;

        forwardsInput.text = move.frontDragFactor.ToString("0.0");
        forwardsSlider.value = move.frontDragFactor;

        sidewaysInput.text = move.sideDragFactor.ToString("0.0");
        sidewaysSlider.value = move.sideDragFactor;

        reverseInput.text = move.reverseFactor.ToString("0.0");
        reverseSlider.value = move.reverseFactor;

    }

    public void GoDown()
    {
        target = new Vector3(0,0,0);
        UpdateDisplays();
    }

    public void GoUp()
    {
        target = new Vector3(0, 270, 0);
        UpdateDisplays();
    }

    public void ChangeSpeed(float value)
    {
        move.speedModifer = value;
        UpdateDisplays();
    }

    public void ChangeRotationSpeed(float value)
    {
        move.rotationSpeed = value;
        UpdateDisplays();
    }

    public void ChangeForwards(float value)
    {
        move.frontDragFactor = value;
        UpdateDisplays();
    }

    public void ChangeSideways(float value)
    {
        move.sideDragFactor = value;
        UpdateDisplays();
    }

    public void ChangeReverse(float value)
    {
        move.reverseFactor = value;
        UpdateDisplays();
    }

    public void ChangeSpeed(string value)
    {
        move.speedModifer = float.Parse(value);
        UpdateDisplays();
    }

    public void ChangeRotationSpeed(string value)
    {
        move.rotationSpeed = float.Parse(value);
        UpdateDisplays();
    }

    public void ChangeForwards(string value)
    {
        move.frontDragFactor = float.Parse(value);
        UpdateDisplays();
    }

    public void ChangeSideways(string value)
    {
        move.sideDragFactor = float.Parse(value);
        UpdateDisplays();
    }

    public void ChangeReverse(string value)
    {
        move.reverseFactor = float.Parse(value);
        UpdateDisplays();
    }
}
