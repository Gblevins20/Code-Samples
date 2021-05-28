/*
* (Greg Brandt, Sam Carpenter)
* (Project 2)
* Keeps track of if the mouse is over this object and tells the playercontroller it is being clicked.
* This should be attached to a child object that is able to be detect mouse over.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDetector : MonoBehaviour
{
	PlayerController player;
    GameOverController gameOverHandle;
	private void Start()
	{
		player = GetComponentInParent<PlayerController>();
        gameOverHandle = FindObjectOfType<GameOverController>();
	}

	//Mouse over is one condition for transforming a block
	public bool isMousedOver = false;
	private void OnMouseExit() { isMousedOver = false; }

	private void OnMouseEnter() { isMousedOver = true;  }

	private void OnMouseDown()
	{
		if (player.isPlayable && player.isBlock && gameOverHandle.activePlayers > 1) { StartCoroutine(MakeUnplayable()); }
	}

	//Ensures that a block is not made playable immediantly after becoming unplayable
	IEnumerator MakeUnplayable()
	{
		yield return new WaitForEndOfFrame();
		player.isPlayable = false;
        gameOverHandle.ReducePlayer();
	}
}
