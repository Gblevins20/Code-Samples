/*
 * Gregory Blevins
 * Project 5
 * Controls targetting and monster movement
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerMainMonster : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Vector3 targetPosition, nextNodePosition;

    PlayerController teleportLocation;

    public MonsterPathing currentNode;

    ChasePlayer isBored;

    public GameObject currentTarget;
    GameObject tempTarget;
    List<GameObject> silhouetteInstances;
    public float timeBetweenSilhouettes = 5f, timeHolder = 5f, teleportCooldown = 5f;

    int teleportIncrement = 1;

    void Start()
    {
        isBored = GetComponent<ChasePlayer>();
        teleportLocation = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(targetPosition);

        if ((transform.position - targetPosition).magnitude >= 1) {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            if (isBored.gettingBored && isBored.boredTimer < 15f && teleportCooldown <= 0)
            {
                TeleportToPlayer();
                teleportCooldown = timeBetweenSilhouettes + 5;
            }
        }

        timeHolder -= 1 * Time.deltaTime;
        teleportCooldown -= 1 * Time.deltaTime;
    }

    public void UpdateTarget(Vector3 updatedPosition)
    {
        targetPosition = updatedPosition;

        if (timeHolder <= 0)
        {
            RevealTarget();

            timeHolder = timeBetweenSilhouettes;
        }
    }

    public void RevealTarget()
    {
        Instantiate(currentTarget, targetPosition, currentTarget.transform.rotation);
    }

    void TeleportToPlayer()
    {
        teleportLocation.FindNearestNode();



        if (teleportLocation.nearestNode.SouthNode != teleportLocation.nearestNode && teleportIncrement <= 2)
        {
            transform.position = teleportLocation.nearestNode.SouthNode.transform.position;
            UpdateTarget(teleportLocation.nearestNode.transform.position);
            teleportIncrement++;


        }
        else if(teleportLocation.nearestNode.EastNode != teleportLocation.nearestNode && teleportIncrement <= 4){
            transform.position = teleportLocation.nearestNode.EastNode.transform.position;
            UpdateTarget(teleportLocation.nearestNode.transform.position);
            teleportIncrement++;

        }
        else if (teleportLocation.nearestNode.NorthNode != teleportLocation.nearestNode && teleportIncrement <= 6)
        {
            transform.position = teleportLocation.nearestNode.NorthNode.transform.position;
            UpdateTarget(teleportLocation.nearestNode.transform.position);
            teleportIncrement++;

        }
        else if (teleportLocation.nearestNode.WestNode != teleportLocation.nearestNode && teleportIncrement >= 7)
        {
            transform.position = teleportLocation.nearestNode.WestNode.transform.position;
            UpdateTarget(teleportLocation.nearestNode.transform.position);
            teleportIncrement = 1;

        }
        teleportIncrement++;
    }
}
