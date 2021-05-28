/*
 * Gregory Blevins
 * Project 5 (Greater Good)
 * Controls Monster Sight
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSight : MonoBehaviour
{
    RaycastHit hit;

    ChasePlayer monsterSight;
    FollowPlayerMainMonster monsterPosition;

    Vector3 playerPosition;

    MonsterPathing hitNode;

    void Start()
    {
        monsterSight = FindObjectOfType<ChasePlayer>();
        monsterPosition = FindObjectOfType<FollowPlayerMainMonster>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MovementNode"))
        {
            hitNode = other.GetComponent<MonsterPathing>();
            monsterPosition.currentNode = hitNode;
        }

        playerPosition = FindObjectOfType<PlayerController>().gameObject.transform.position;

        if (other.CompareTag("Player") && !other.CompareTag("Obstacles"))
        {
            
            if (Physics.Raycast(monsterPosition.gameObject.transform.position,playerPosition,out hit))
            {
                monsterSight.DoISee(playerPosition);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !other.CompareTag("Obstacles"))
        {
            playerPosition = FindObjectOfType<PlayerController>().gameObject.transform.position;

            if (Physics.Raycast(monsterPosition.gameObject.transform.position, playerPosition, out hit))
            {
                monsterSight.DoISee(playerPosition);
            }
        }
    }
}
