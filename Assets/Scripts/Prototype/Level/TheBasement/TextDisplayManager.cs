using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDisplayManager : MonoBehaviour {

    public Queue<string> queuedMessages = new Queue<string>();
    public TextMeshProUGUI text;
    public float boxHalfWidth = 15;
    public float speed = 5;
    public float padding = 0.2f;
    string currentString;

    public void AddMessage(object message) {
        string actualMessage = (string)message;
        queuedMessages.Enqueue(actualMessage);
    }

    public void TestMessage(object message)
    {
        Debug.Log("Test");
        queuedMessages.Enqueue("PLAYER ONE DEAD, KILLED BY PLAYER TWO");
    }

    private void Update()
    {
        if(currentString == null)
        {
            StartCoroutine(PlayNewString());
        }
        else
        {
            if (text.gameObject.activeSelf)
            {
                RectTransform rectTransform = text.GetComponent<RectTransform>();

                rectTransform.anchoredPosition -= new Vector2( speed * Time.deltaTime, 0);
                if(rectTransform.anchoredPosition.x < -(rectTransform.rect.width/2) - boxHalfWidth - padding)
                {
                    text.gameObject.SetActive(false);
                }
            }
            else
            {
                StartCoroutine(PlayNewString());
            }
        }
    }

    private IEnumerator PlayNewString()
    {
        if (queuedMessages.Count > 0)
        {
            RectTransform rectTransform = text.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(100000, 100000);
            currentString = queuedMessages.Dequeue();
            text.text = currentString;
            text.gameObject.SetActive(true);
            yield return new WaitForEndOfFrame();
            
            rectTransform.anchoredPosition = new Vector2((rectTransform.rect.width / 2) + padding + boxHalfWidth, 0);
        }
        else
        {
            currentString = null;
        }
    }



}
