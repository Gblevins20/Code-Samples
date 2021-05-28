using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TweetManager : MonoBehaviour
{
    [SerializeField] GameObject[] tweets;
	[SerializeField] int deleteTweetsAfter = 5;
	[SerializeField] float spawnTime = 5;
	[SerializeField] string[] tweetMessages = new string[10];
    public bool spawnTweets = false;

	public void StartSpawnTweets() { spawnTweets = true; StartCoroutine(SpawnTweets()); }

	public void ClearTweets() 
	{
		for(int i = 0; i < GameManager.instance.GetTweetDisplay().transform.childCount; i++)
		{
			Destroy(GameManager.instance.GetTweetDisplay().transform.GetChild(i).gameObject);
		}
	}

	public IEnumerator SpawnTweets()
	{
		while(spawnTweets)
		{
			yield return new WaitForSeconds(spawnTime);
			GameObject tweet = Instantiate(tweets[Random.Range(0, tweets.Length)], GameManager.instance.GetTweetDisplay().transform);
			tweet.GetComponentInChildren<TMP_Text>().text = tweetMessages[Random.Range(0, tweetMessages.Length)];
			if(GameManager.instance.GetTweetDisplay().transform.childCount > deleteTweetsAfter) { Destroy(GameManager.instance.GetTweetDisplay().transform.GetChild(0).gameObject); }
		}
	}
}
