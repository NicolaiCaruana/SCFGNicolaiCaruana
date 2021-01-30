using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = this.transform;
        StartCoroutine(moveTarget());
    }

    public IEnumerator moveTarget()
    {
        List<Vector3> positions = new List<Vector3>();
        positions.Add(target.position);
        positions.Add(new Vector3(target.position.x, -target.position.y));
        StartCoroutine(moveTarget(target.transform, positions, true));
        yield return null;
    }

    public IEnumerator moveTarget(Transform t, List<Vector3> points, bool loop)
    {
        if (loop)
        {
            while (true)
            {
                List<Vector3> forwardpoints = points;
                foreach (Vector3 position in forwardpoints)
                {
                    while (Vector3.Distance(t.position, position) > 0.5f)
                    {
                        t.position = Vector3.MoveTowards(t.position, position, 1f);
                        Debug.Log(position);/**/
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                forwardpoints.Reverse();
                yield return null;
            }
        }

        else
        {
            foreach (Vector3 position in points)
            {
                while (Vector3.Distance(t.position, position) > 0.5f)
                {
                    t.position = Vector3.MoveTowards(t.position, position, 1f);
                    yield return new WaitForSeconds(0.2f);
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