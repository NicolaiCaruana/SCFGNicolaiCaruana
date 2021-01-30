using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class KU4Patrol : MonoBehaviour
{
    Seeker patrolSeeker;
    Path patrolPath;
    GameObject graphParent;
    public Transform firstWaypoint;
    public List<Transform> Waypoints = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        patrolSeeker = GetComponent<Seeker>();
        firstWaypoint = GameObject.Find("Waypoint 1").transform;
        graphParent = GameObject.Find("AStarGrid");
        graphParent.GetComponent<AstarPath>().Scan();
        patrolPath = patrolSeeker.StartPath(transform.position, firstWaypoint.position);
        StartCoroutine(updateGraph());

        StartCoroutine(moveToWaypoints(this.transform));
    }

    IEnumerator updateGraph()
    {
        while (true)
        {
            graphParent.GetComponent<AstarPath>().Scan();
            yield return null;
        }
    }

    IEnumerator moveToWaypoints(Transform t)
    {
        while (true)
        {
            foreach (Transform w in Waypoints)
            {
                List<Vector3> posns = patrolPath.vectorPath;
                Debug.Log("Positions Count: " + posns.Count);

                for (int counter = 0; counter < posns.Count; counter++)
                {
                    if (posns[counter] != null)
                    {
                        while (Vector3.Distance(t.position, posns[counter]) >= 0.5f)
                        {
                            t.position = Vector3.MoveTowards(t.position, posns[counter], 1f);
                            patrolPath = patrolSeeker.StartPath(t.position, w.position);
                            yield return patrolSeeker.IsDone();
                            posns = patrolPath.vectorPath;
                            yield return new WaitForSeconds(0.2f);
                        }
                        patrolPath = patrolSeeker.StartPath(t.position, w.position);
                        yield return patrolSeeker.IsDone();
                        posns = patrolPath.vectorPath;
                    }
                }
            }
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
