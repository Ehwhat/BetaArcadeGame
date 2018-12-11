using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tanks/UI/NewsFeed/New News Feed System")]
public class NewsFeedSystem : ScriptableObject {

    public NewsFillerDefinition fillerDefiniton;

    public Queue<string> newsQueue = new Queue<string>();

    private List<string> fillerCycle = new List<string>();
    private int fillerIndex = 0;

    public void CreateNewFillerCycle()
    {
        List<string> filler = new List<string>(fillerDefiniton.newFiller);
        while(filler.Count > 0)
        {
            int index = Random.Range(0, filler.Count);
            fillerCycle.Add(filler[index]);
            filler.RemoveAt(index);
        }
    }

    public void AddNewsToQueue(string news)
    {
        newsQueue.Enqueue(news);
    }

    public string GetNewsItem()
    {
        if(newsQueue.Count > 0)
        {
            return newsQueue.Dequeue();
        }
        else
        {
            return GetRandomFiller();
        }
    }

    private string GetRandomFiller()
    {
        if(fillerIndex >= fillerCycle.Count)
        {
            fillerIndex = 0;
        }
        return fillerCycle[fillerIndex++];
    }
	
}
