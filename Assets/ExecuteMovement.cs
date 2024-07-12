using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteMovement : MonoBehaviour
{
    RoutePlanning routePlanner;
    Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        routePlanner = GameObject.Find("MRUK").GetComponent<RoutePlanning>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlanAndMoveTo(Vector3 destination, float moveSpeed, float rotateSpeed, bool turnLeftAtLast=false)
    {
        StartCoroutine(PlanAndMoveTo_Coroutine(destination, moveSpeed, rotateSpeed, turnLeftAtLast));
    }

    private IEnumerator PlanAndMoveTo_Coroutine(Vector3 destination, float moveSpeed, float rotateSpeed, bool turnLeftAtLast)
    {
        // first plan the route
        // then move there
        float robotWidth = gameObject.GetComponent<BoxCollider>().bounds.size.x;

        List<Vector3> foundPath = routePlanner.FindPath(transform.position, destination, robotWidth, 0.05f);
        if (foundPath.Count == 0)
        {
            Debug.Log("No Path Found");
            yield return null;
        }
        LineRenderer lr = gameObject.GetComponent<LineRenderer>();
        if (lr != null)
        {
            routePlanner.RenderPath(lr, foundPath);
        }
        if (turnLeftAtLast){
            Vector3 lastPoint = foundPath[foundPath.Count - 1];
            Vector3 direction = foundPath[foundPath.Count - 1] - foundPath[foundPath.Count - 2];
            Vector3 direction_turn_left = new Vector3(-direction.z, direction.y, direction.x);
            Vector3 newPoint = lastPoint + direction_turn_left.normalized * 0.01f;
            foundPath.Add(newPoint);
        }
        MoveAlongPath(foundPath, moveSpeed, rotateSpeed);
        yield return null;
    }

    public void MoveAlongPath(List<Vector3> targetList, float moveSpeed, float rotateSpeed)
    {
        // move gameObject to target position
        // moveSpeed: length per second
        // rotateSpeed: degrees per second
        StartCoroutine(MoveAlongPath_Coroutine(targetList, moveSpeed, rotateSpeed));
        Debug.Log("Coroutine started");
    }

    private IEnumerator MoveAlongPath_Coroutine(List<Vector3> targetList, float moveSpeed, float rotateSpeed)
    {
        foreach (Vector3 target in targetList)
        {
            // first rotate to face target
            while (Vector3.Angle(transform.forward, target - transform.position) > 1f)
            {
                // Debug.Log("Rotating ... ...");
                // Vector3 direction = (target - transform.position).normalized;
                // Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, rotateSpeed * Mathf.Deg2Rad * Time.deltaTime, 0.0f);
                // transform.rotation = Quaternion.LookRotation(newDirection);

                // ensure that the rotation degree is less than 180
                Quaternion targetRotation = Quaternion.LookRotation((target - transform.position).normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                
                yield return null;
            }

            // then move to target position
            while (Vector3.Distance(transform.position, target) > 0.01f)
            {
                // Debug.Log("Moving ... ...");
                Vector3 direction = (target - transform.position).normalized;
                float step = moveSpeed * Time.deltaTime;
                // transform.position = Vector3.MoveTowards(transform.position, target, step);
                rigidbody.MovePosition(transform.position + direction * step);
                yield return null;
            }
        }
    }

}
