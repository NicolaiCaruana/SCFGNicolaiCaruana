using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class customAIMoveScriptGrid : MonoBehaviour
{
    Seeker seeker;
    Path pathToFollow;
    public Transform target;
    GameObject graphParent;
    GameObject targetNode;
    public List<Transform> obstacleNodes;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(this.name);
        seeker = GetComponent<Seeker>();
        target = GameObject.Find("Target").transform;
        targetNode = GameObject.Find("TargetNode");
        graphParent = GameObject.Find("AStarGrid");
        graphParent.GetComponent<AstarPath>().Scan();
        pathToFollow = seeker.StartPath(transform.position, target.position);
        StartCoroutine(updateGraph());
        StartCoroutine(moveTowardsEnemy(this.transform));
    }

    IEnumerator updateGraph()
    {
        while (true)
        {
            graphParent.GetComponent<AstarPath>().Scan();
            yield return null;
        }
    }

    IEnumerator moveTowardsEnemy(Transform t)
    {
        while (true)
        {
            List<Vector3> posns = pathToFollow.vectorPath;
            Debug.Log("Positions Count: " + posns.Count);

            for (int counter = 0; counter < posns.Count; counter++)
            {
                if (posns[counter] != null)
                {
                    while (Vector3.Distance(t.position, posns[counter]) >= 0.5f)
                    {
                        t.position = Vector3.MoveTowards(t.position, posns[counter], 1f);
                        pathToFollow = seeker.StartPath(t.position, target.position);
                        yield return seeker.IsDone();
                        posns = pathToFollow.vectorPath;
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                pathToFollow = seeker.StartPath(t.position, target.position);
                yield return seeker.IsDone();
                posns = pathToFollow.vectorPath;
            }
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

//KU1 
//Local Avoidance - Makes use of ORCA and RV02 libraries + provides tools for prevention of collision between agents.

//KU2
//The script scans the grid and generates paths from the AI to the target,
//then coroutines are used to move the target along a set path and scans
//the graph again for the seeker to adjust the path. The coroutine moves
//the AI towards the target. The path is recreated when the AI touches the
//Target, preventing the AI from stopping the routine