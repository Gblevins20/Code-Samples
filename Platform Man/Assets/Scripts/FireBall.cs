using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		PlayerController playerController = collision.GetComponentInParent<PlayerController>();
		if(playerController)
		{
			if(!playerController.isBlock)
			{
				Destroy(playerController.gameObject);
			}
			else { Destroy(gameObject); }
		}
		else { Destroy(gameObject); }
	}
}
