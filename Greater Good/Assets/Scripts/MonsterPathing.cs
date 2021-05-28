/*
 * Gregory Blevins
 * Project 5
 * Handles nodes and node target management
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPathing : MonoBehaviour
{
    public MonsterPathing SouthNode = null, WestNode = null, NorthNode = null, EastNode = null;

    PlayerController targetPlayer;
    ChasePlayer startingPosition;
    FollowPlayerMainMonster targetingData;

    Vector3 currentTarget;

    public bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        targetPlayer = FindObjectOfType<PlayerController>();
        startingPosition = FindObjectOfType<ChasePlayer>();
        targetingData = FindObjectOfType<FollowPlayerMainMonster>();
    }


    public void Deactivate()
    {
        activated = false;
    }

    public void Activate()
    {
        activated = true;
    }
}