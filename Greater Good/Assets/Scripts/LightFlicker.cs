using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    float originalIntesity;
    Light light;
    // Start is called before the first frame update
    void Start()
    {
        light = GetComponent<Light>();
        originalIntesity = light.intensity;
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
	{
        while(true)
		{
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            light.intensity = 0;
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
            light.intensity = originalIntesity;
        }
	}

}
