using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsFeedDisplayer : MonoBehaviour {

    public static string spaceCharacter = " | ";

    public float slideMoveSpeed = 10f;
    public float defaultSlideWidth = 1600f;

    public RectTransform slideHolder;
    public NewsFeedSystem system;
    public int slideCharacterLimit = 90;
    public TMPro.TextMeshProUGUI slidePrefab;

    private List<TMPro.TextMeshProUGUI> slides = new List<TMPro.TextMeshProUGUI>();
    private TMPro.TextMeshProUGUI lastSlide;


    private void Start()
    {
        Debug.Log("start");
        system.CreateNewFillerCycle();
        InitaliseNewSlide();
    }

    TMPro.TextMeshProUGUI InitaliseNewSlide(bool prewarm = false)
    {
        float width = defaultSlideWidth;
        TMPro.TextMeshProUGUI slide = Instantiate(slidePrefab, slideHolder);
        slide.rectTransform.anchoredPosition += Vector2.right * width;
        PopulateSlide(slide);
        lastSlide = slide;
        slides.Add(slide);
        return slide;
    }

    void Update()
    {

        if (Input.GetKeyDown("k"))
        {
            system.AddNewsToQueue("Test News!");
        }

        for (int i = 0; i < slides.Count; i++)
        {
            if(slides[i].rectTransform.anchoredPosition.x < -slides[i].preferredWidth)
            {
                Destroy(slides[i].gameObject);
                slides.RemoveAt(i);
            }
            slides[i].rectTransform.anchoredPosition += Vector2.left * slideMoveSpeed * Time.deltaTime;
        }
        if(lastSlide.rectTransform.anchoredPosition.x <= defaultSlideWidth - lastSlide.preferredWidth)
        {
            InitaliseNewSlide();
        }
    }

    [ContextMenu("Test Populate")]
    private void PopulateSlide(TMPro.TextMeshProUGUI slide)
    {
        string newsFeed = " | " + system.GetNewsItem() ;

        slide.text = newsFeed;

    }

}
