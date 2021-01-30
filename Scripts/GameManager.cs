using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Random random = new Random();
    GameObject graphParent;
    public GameObject[] AIObjects;

    // Start is called before the first frame update
    void Start()
    {
        GameObject AI = Resources.Load<GameObject>("Prefabs/AI");
        AI.GetComponent<customAIMoveScriptGrid>().enabled = false;
        PopulatePatrollingAI();
    }

    public void SpawnObstacles()
    {
        GameObject Obstacle = Resources.Load<GameObject>("Prefabs/Obstacle");
        Obstacle.transform.localScale = new Vector3(Random.Range(1, 10), Random.Range(1, 10), 1);
        Instantiate(Obstacle, new Vector3(Random.Range(-50, 50), Random.Range(-30, 50), 0), Quaternion.identity);
    }

    public void ReScan()
    {
        graphParent = GameObject.Find("AStarGrid");
        graphParent.GetComponent<AstarPath>().Scan();
        Debug.Log("RE-SCANNING PATH");
    }

    public void SpawnAI()
    {
        GameObject AI = Resources.Load<GameObject>("Prefabs/AI");
        Instantiate(AI, new Vector3(Random.Range(-50, 50), Random.Range(-30, 50), 0), Quaternion.identity);
    }

    public void startAI()
    {
        GameObject AI = Resources.Load<GameObject>("Prefabs/AI");
        AI.GetComponent<customAIMoveScriptGrid>().enabled = true;
        AIObjects = GameObject.FindGameObjectsWithTag("AI");
        foreach (GameObject obj in AIObjects)
        {
            obj.GetComponent<customAIMoveScriptGrid>().enabled = true;

        }
    }

    public void PopulatePatrollingAI()
    {
        if (SceneManager.GetActiveScene().name == "Scene2")
        {
            GameObject PatrollingAIPrefab = Resources.Load<GameObject>("Prefabs/PatrollingAI");
            GameObject PatrollingAI = Instantiate(PatrollingAIPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            KU4Patrol patrolScript = PatrollingAI.GetComponent<KU4Patrol>();
            for (int i = 0; i < 10; i++)
            {
                string waypointItem = "Waypoint " + (i + 1);
                Debug.Log(waypointItem);
                patrolScript.Waypoints.Add(GameObject.Find(waypointItem).transform);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
