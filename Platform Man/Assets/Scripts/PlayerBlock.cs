using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlock : MonoBehaviour
{
	PlayerController controller;
	

	private void Start()
	{
		controller = GetComponentInParent<PlayerController>();
	}

	
	private void OnMouseDown()
	{
		controller.isPlayable = !controller.isPlayable;
	}
}
