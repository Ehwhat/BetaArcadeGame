using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuipDisplayer : MonoBehaviour {

    public RectTransform quipHolder;
    public Image quipTextBox;
    public Animator animator;
    public TMPro.TextMeshProUGUI text;
    public float lettersPerSecond = 10f;
    public float quipExitDelay = 0.6f;
    public int id;
    public QuipSystemDefinition quipSystem;


    public void Awake()
    {
        if (quipSystem)
        {
            quipSystem.RegisterQuipDisplayer(this);
        }
    }

    private void Update()
    {
    }

    [ContextMenu("Test Quip")]
    private void SayTestQuip()
    {
        Activate();
        SayQuip("How dare you!",true);
        
    }

    public void ChangeColour(Color c)
    {
        quipTextBox.color = c;
    }

    public void Activate()
    {
        animator.SetBool("IsLowered", true);
    }

    public void Deactivate()
    {
        animator.SetBool("IsLowered", false);
    }

    public void SayQuip(string quip, bool deactivateWhenDone = false)
    {
        Activate();
        StopAllCoroutines();
        StartCoroutine(QuipEnumerator(quip, deactivateWhenDone));
    }

    private IEnumerator QuipEnumerator(string quip, bool deactivateWhenDone = false)
    {
        text.text = "";

        quipHolder.sizeDelta = new Vector2(Mathf.Max(text.preferredWidth, 115), quipHolder.sizeDelta.y);
        int i = 0;
        while(text.text.Length < quip.Length)
        {
            text.text += quip[i];
            i++;
            quipHolder.sizeDelta = new Vector2(Mathf.Max(text.preferredWidth,115), quipHolder.sizeDelta.y);
            yield return new WaitForSeconds(1 / lettersPerSecond);
        }
        yield return new WaitForSeconds(quipExitDelay);
        if(deactivateWhenDone)
            Deactivate();
    }

}
