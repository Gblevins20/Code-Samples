using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecieveShadows : MonoBehaviour
{
	private void Start()
	{
		GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		GetComponent<SpriteRenderer>().receiveShadows = true;
	}
}
