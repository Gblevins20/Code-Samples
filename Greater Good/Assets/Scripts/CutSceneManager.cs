using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CutSceneSlide
{
    [SerializeField] Sprite sprite;
    [SerializeField] int slideTime;

	public Sprite Sprite { get => sprite;  }
	public int SlideTime { get => slideTime;  }
}

[System.Serializable]
public class CutScene
{
    [SerializeField] CutSceneSlide[] slides;
    [SerializeField] string cutSceneName;
    int index = 0;

	public string CutSceneName { get => cutSceneName; }
    public int Length() { return slides.Length; }

	public CutSceneSlide GetNextSlide()
	{
        if (index < slides.Length)
        {
            CutSceneSlide slide = slides[index];
            index++;
            return slide;
        }
        return null;
	}
}

public class CutSceneManager : MonoBehaviour
{
    public static bool cutScenePlaying = false;
    public static CutSceneManager instance;

    [SerializeField] CutScene[] cutScenes;
    [SerializeField] Image currentSlide;

	private void Awake()
	{
		instance = this;  
        currentSlide.gameObject.SetActive(false);
	}

	private void Start()
	{
        PlayCutScene("Opening");
        StartCoroutine(StartGameAfterOpening());
	}

	public void PlayCutScene(string cutSceneName)
	{
        bool sceneFound = false;
        foreach(CutScene cutScene in cutScenes)
		{
            if(cutScene.CutSceneName == cutSceneName) 
            {
                StartCoroutine(PlaySlideShow(cutScene));
                sceneFound = true;
                break; 
            }
		}
		if (!sceneFound) { Debug.LogError("[CutSceneManager]" +  cutSceneName + " not found in cutScenes"); }
	}

    IEnumerator PlaySlideShow(CutScene cutScene)
	{
        currentSlide.gameObject.SetActive(true);
        cutScenePlaying = true;
        for(int i = 0; i < cutScene.Length(); i++)
		{
            CutSceneSlide slide = cutScene.GetNextSlide();
            currentSlide.sprite = slide.Sprite;
            float colorFade = 0;
            currentSlide.color = new Color(colorFade, colorFade, colorFade, 1);
            while(colorFade < 1)
			{
                yield return new WaitForSeconds(0.1f);
                colorFade += 0.2f;
                currentSlide.color = new Color(colorFade, colorFade, colorFade, 1);
            }
            float elapsedTime = 0;
            while(elapsedTime < slide.SlideTime && !Input.GetKeyDown(KeyCode.Space))
			{
                yield return new WaitForSeconds(0.01f);
                elapsedTime += 0.01f;
            }
            while (colorFade > 0)
            {
                yield return new WaitForSeconds(0.1f);
                colorFade -= 0.2f;
                currentSlide.color = new Color(colorFade, colorFade, colorFade, 1);
            }
        }
        currentSlide.gameObject.SetActive(false);
        cutScenePlaying = false;
    }

    IEnumerator StartGameAfterOpening()
	{
        yield return new WaitWhile(() => cutScenePlaying);
        GameManager.instance.ResetPopulation();
        GameManager.instance.gameStarted = true;
	}
}
