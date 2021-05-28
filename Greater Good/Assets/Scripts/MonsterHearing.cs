using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHearing : MonoBehaviour
{
    RaycastHit hit;

    ChasePlayer monsterHear;

    Vector3 playerPosition;

    PlayerController playerHolder;

    void Start()
    {
        monsterHear = FindObjectOfType<ChasePlayer>();
        playerHolder = FindObjectOfType<PlayerController>();
    }

    

    private void OnTriggerStay(Collider other)
    {

        playerPosition = playerHolder.gameObject.transform.position;

        if (other.CompareTag("Player") && playerHolder.IsItSprinting() )
        {

            if (Physics.Raycast(transform.position, transform.TransformDirection(other.transform.position), out hit, Vector3.Distance(transform.position, other.transform.position)) && hit.transform.tag != "Obstacle")
            {
                monsterHear.DoIHear(playerPosition);
            }
        }
    }
}
