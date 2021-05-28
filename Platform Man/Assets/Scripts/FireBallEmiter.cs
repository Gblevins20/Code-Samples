using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallEmiter : MonoBehaviour
{
    [SerializeField]GameObject fireBallPrefab;

	private void Start()
	{
		InvokeRepeating("SpawnFireBall", 0, 1);
	}

	void SpawnFireBall()
	{
		Instantiate(fireBallPrefab, transform.position, new Quaternion());
	}
}
