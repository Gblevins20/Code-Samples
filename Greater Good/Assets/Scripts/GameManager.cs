using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	[SerializeField] int startingPopulation;
	[SerializeField] int minPopulationDetlaPerTick;
	[SerializeField] int maxPopulationDetlaPerTick;
	[SerializeField] int tickTime = 5;
	[SerializeField] TMP_Text populationText;
	[SerializeField] GameObject tweetDisplay;

	public GameObject pauseMenu;
	public GameObject GameOverPanel;

    ChasePlayer quickFix;

	public int currentPopulation;
	string curentLevelName = string.Empty;
	public bool gameStarted = false;

	private void Awake()
	{
		if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
		else { Destroy(gameObject); }
	}

	private void Start()
	{
		currentPopulation = startingPopulation;
	}

	private void Update()
	{
		if (gameStarted) 
		{
			gameStarted = false; 
			quickFix = FindObjectOfType<ChasePlayer>(); 
			quickFix.ActivateChase(); 
			StartCoroutine(ReducePopulation()); 
			FindObjectOfType<TweetManager>().StartSpawnTweets(); 
		}
		populationText.text = "People that need to evacuate: " + currentPopulation;
		if (Input.GetKeyDown(KeyCode.P ) ) { pauseMenu.SetActive(true); if (Time.timeScale == 1) { Pause(); } else { Unpause(); } }
	}

	public void ResetPopulation() 
	{
		currentPopulation = startingPopulation;
		TweetManager tweetManager = FindObjectOfType<TweetManager>();
		if(tweetManager)
		{
			Debug.Log("Clear Tweets");
			tweetManager.ClearTweets();
		}
	}

	public GameObject GetTweetDisplay() { return tweetDisplay; }

	public void LoadLevel(string levelName)
	{
		Unpause();
		AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
		if (ao == null) { Debug.LogError("[GameManager] Unable to load level "); }
		curentLevelName = levelName;
	}

	public void UnloadLevel(string levelName)
	{
		AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
		if (ao == null) { Debug.LogError("[GameManager] Unable to unload level "); }
	}

	public void UnloadCurrentLevel()
	{
		AsyncOperation ao = SceneManager.UnloadSceneAsync(curentLevelName);
		if (ao == null) { Debug.LogError("[GameManager] Unable to unload level "); }
	}

	public void Pause()
	{
		Time.timeScale = 0;
		pauseMenu.SetActive(true);
	}

	public void Unpause()
	{
		Time.timeScale = 1;
		pauseMenu.SetActive(false);
	}

	IEnumerator ReducePopulation()
	{
		while(currentPopulation > 0 && !CutSceneManager.cutScenePlaying)
		{
			currentPopulation -= Random.Range(minPopulationDetlaPerTick, maxPopulationDetlaPerTick+1);
			if(currentPopulation <= 0) { currentPopulation = 0; }
			yield return new WaitForSeconds(tickTime);
		}
		if (!CutSceneManager.cutScenePlaying)
		{
			CutSceneManager.instance.PlayCutScene("Conclusion");
			yield return new WaitWhile(() => CutSceneManager.cutScenePlaying);
			UnloadLevel("Main Level");
			GameOverPanel.SetActive(true);
		}
    }

	public void GameOver() { StartCoroutine(LoseGame()); }

	IEnumerator LoseGame()
	{
		CutSceneManager.instance.PlayCutScene("Loss");
		yield return new WaitWhile(() => CutSceneManager.cutScenePlaying);
        UnloadLevel("Main Level");
		GameOverPanel.SetActive(true);
	}
}
