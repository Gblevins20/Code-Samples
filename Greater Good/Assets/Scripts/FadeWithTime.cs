/*
 * Gregory Blevins
 * Project 5
 * Removes extra silhouettes
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeWithTime : MonoBehaviour
{
    public float timeToFade;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimeToFade());
    }

    IEnumerator TimeToFade()
    {
        yield return new WaitForSeconds(timeToFade);

        Destroy(gameObject);
    }

}
