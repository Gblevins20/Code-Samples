/*
 * Gregory Blevins
 * Project 5
 * Controls Monster Memory and Pathing
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    Vector3 lastKnownPosition;
    public Vector3 startArea;

    public bool gettingBored = false;
    public float boredTimer = 30f, boredomDelay = 3f;
    float timerHolder, boredomDelayHolder;

    bool gameStarted = false;

    public MonsterPathing startingNode;
    public List<Vector3> pathToPlayer;
    MonsterPathing[] allNodes;

    FollowPlayerMainMonster monsterTarget;

    PlayerController player;

    //public ChaserBehaviour temp;

    // Start is called before the first frame update
    void Start()
    {
        timerHolder = boredTimer;
        boredomDelayHolder = boredomDelay;

        allNodes = FindObjectsOfType<MonsterPathing>();

        lastKnownPosition = transform.position;

        monsterTarget = FindObjectOfType<FollowPlayerMainMonster>();

        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStarted && GameManager.instance.gameStarted)
        {
            gameStarted = true;

            startArea = monsterTarget.currentNode.transform.position;
        }

        if (gameStarted)
        {
            
            SensoryPriority();

            if (gettingBored)
            {
                boredTimer -= 1 * Time.deltaTime;
                //if (pathExists)
                //{
                //    FollowPathToPlayer();
                //}
                //else if (!pathExists)
                //{
                //    SearchForPath();
                //}
            }
            boredomDelay -= 1 * Time.deltaTime;
        }
    }

    //Determines if the monster can find the player
    void SensoryPriority()
    {

        if (DoISee(lastKnownPosition))
        {
            
        }
        else if (DoIHear(lastKnownPosition))
        {
            
        }
        else if (boredomDelay <= 0 && !gettingBored)
        {
            gettingBored = true;

        }

    }

    //Handles the "sight range" of the monster
    public bool DoISee(Vector3 target)
    {
        if (target!=lastKnownPosition)
        {

            lastKnownPosition = target;

            monsterTarget.UpdateTarget(lastKnownPosition);

            gettingBored = false;

            boredTimer = timerHolder;

            boredomDelay = boredomDelayHolder;

            //if (pathExists)
            //{
            //    pathToPlayer.Clear();
            //    firstInput = true;
            //}
            //pathExists = false;

            return true;
        }

        return false;
    }

    //Handles the "hearing range" of the monster
    public bool DoIHear(Vector3 target)
    {

        if (target != lastKnownPosition)
        {

            lastKnownPosition = target;

            monsterTarget.UpdateTarget(lastKnownPosition);

            gettingBored = false;

            boredTimer = timerHolder;

            boredomDelay = boredomDelayHolder;

            //if (pathExists)
            //{
            //    pathToPlayer.Clear();
            //    firstInput = true;
            //}

            return true;
        }

        return false;
    }

    public void ActivateChase()
    {
        gameStarted = true;
    }

    //public void FollowPathToPlayer()
    //{
    //    monsterTarget.UpdateTarget(pathToPlayer[0]);
    //}

    //public void UpdatePathToPlayer(List<Vector3> input)
    //{
    //    pathToPlayer = input;

    //    for (int i = 0; i < allNodes.Length; i++)
    //    {
    //        allNodes[i].Deactivate();
    //    }
    //}

    //void FindNearestNode()
    //{
    //    pathExists = true;

    //    float lenght = 9999.0f;

    //    MonsterPathing nearestNode = null;

    //    for (int i = 0; i < allNodes.Length; i++)
    //    {
    //        if (Mathf.Abs((transform.position - allNodes[i].transform.position).magnitude) < lenght)
    //        {
    //            lenght = Mathf.Abs((this.transform.position - allNodes[i].transform.position).magnitude);
    //            nearestNode = allNodes[i];
    //        }
    //    }

    //    startingNode = nearestNode;
    //}

    //public void UpdateNode()
    //{
    //    pathToPlayer.RemoveAt(0);
    //    if (pathToPlayer.Count <= 0)
    //    {
    //        firstInput = true;
    //    }
    //}

    //void SearchForPath()
    //{
    //    FindNearestNode();
    //    player.FindNearestNode();

    //    temp.InitialValues(startingNode, player.nearestNode, 0);

    //    Instantiate(temp, startingNode.transform, startingNode.transform);
    //}
}
