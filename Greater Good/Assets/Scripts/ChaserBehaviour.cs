/*
 * Gregory Blevins
 * Project 6, 7
 * Handles the monster's pathing to the player
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserBehaviour : MonoBehaviour
{
    ChasePlayer output;

    public List<MonsterPathing> previousNodes;

    public MonsterPathing setTarget, endNode;

    public int currentNode = 0;

    bool north, west;

    public ChaserBehaviour(MonsterPathing startingNode, MonsterPathing endingNode)
    {
        setTarget = startingNode;

        endNode = endingNode;

        previousNodes = new List<MonsterPathing>() { startingNode };
    }

    // Start is called before the first frame update
    void Start()
    {
        output = FindObjectOfType<ChasePlayer>();
    }

    private void Update()
    {
        MoveToTarget();
    }

    void MoveToTarget()
    {
        this.transform.position = setTarget.transform.position;

        Debug.Log("How long does this happen: " + transform.position);

        if (endNode.transform.position.z >= transform.position.z && !setTarget.NorthNode.activated)
        {
            north = true;
        }
        else
        {
            north = false;
        }
        if (endNode.transform.position.x <= transform.position.x && !setTarget.WestNode.activated)
        {
            west = true;
        }
        else
        {
            west = false;
        }

        if (north && setTarget.NorthNode != setTarget && !setTarget.NorthNode.activated)
        {
            Debug.Log("Going North");
            setTarget = setTarget.NorthNode;
            setTarget.Activate();
        }
        else if (west && setTarget.WestNode != setTarget && !setTarget.WestNode.activated)
        {
            Debug.Log("Going West");
            setTarget = setTarget.WestNode;
            setTarget.Activate();
        }
        else if (!north && setTarget.SouthNode != setTarget && !setTarget.SouthNode.activated)
        {
            Debug.Log("Going South");
            setTarget = setTarget.SouthNode;
            setTarget.Activate();
        }
        else if (!west && setTarget.EastNode != setTarget && !setTarget.EastNode.activated)
        {
            Debug.Log("Going East");
            setTarget = setTarget.EastNode;
            setTarget.Activate();
        }

        if (setTarget == setTarget.SouthNode)

        previousNodes.Add(setTarget);

        if (setTarget == endNode)
        {
            Debug.Log("Time to end this");
            Destroy(this.GetComponent<GameObject>());
        }
    }

    List<Vector3> PathToTravel()
    {
        List<Vector3> navigationalNodes = new List<Vector3>();

        for (int i = 0; i <= previousNodes.Count; i++)
        {
            navigationalNodes.Add(previousNodes[i].transform.position);
        }

        return navigationalNodes;
    }

    public void InitialValues(MonsterPathing startingNode, MonsterPathing endingNode, int startingValue)
    {
        setTarget = startingNode;

        endNode = endingNode;

        currentNode = startingValue;

        previousNodes = new List<MonsterPathing>() { startingNode };

        Debug.Log(previousNodes + " & " + currentNode);

        MoveToTarget();
    }
    
}
