using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteMovement : MonoBehaviour
{
    RoutePlanning routePlanner;
    Rigidbody rigidbody;
    
    void Start()
    {
        routePlanner = GameObject.Find("MRUK").GetComponent<RoutePlanning>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlanAndMoveTo(Vector3 destination, float moveSpeed, float rotateSpeed, bool finalRotate=false, Vector3 finalFaceTowards=default(Vector3))
    {
        StartCoroutine(PlanAndMoveTo_Coroutine(destination, moveSpeed, rotateSpeed, finalRotate, finalFaceTowards));
    }

    private IEnumerator PlanAndMoveTo_Coroutine(Vector3 destination, float moveSpeed, float rotateSpeed, bool finalRotate, Vector3 finalFaceTowards)
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
        
        yield return MoveAlongPath_Coroutine(foundPath, moveSpeed, rotateSpeed, finalRotate, finalFaceTowards); 
        // it will wait for the coroutine to finish
    }

    public void MoveAlongPath(List<Vector3> targetList, float moveSpeed, float rotateSpeed, bool finalRotate=false, Vector3 finalFaceTowards=default(Vector3), bool loop=false)
    {
        // move gameObject to target position
        // moveSpeed: length per second
        // rotateSpeed: degrees per second
        if (loop)
            StartCoroutine(MoveAlongPath_Loop_Coroutine(targetList, moveSpeed, rotateSpeed));
        else
            StartCoroutine(MoveAlongPath_Coroutine(targetList, moveSpeed, rotateSpeed, finalRotate, finalFaceTowards));
        Debug.Log("Coroutine started");
    }

    private IEnumerator MoveAlongPath_Coroutine(List<Vector3> targetList, float moveSpeed, float rotateSpeed, bool finalRotate, Vector3 finalFaceTowards)
    {
        // start to play the movement sound effect just before moving
        AudioSource movementAudioSource = gameObject.GetComponent<AudioSource>();
        movementAudioSource.loop = true;
        movementAudioSource.Play();

        foreach (Vector3 target in targetList)
        {
            Debug.Log("Next point is: " + target);
            // first rotate to face target
            while (Vector3.Angle(transform.forward, target - transform.position) > 1f)
            {
                // ensure that the rotation degree is less than 180
                Quaternion targetRotation = Quaternion.LookRotation((target - transform.position).normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                
                yield return null;
            }

            // then move to target position
            while (Vector3.Distance(transform.position, target) > 0.02f)
            {
                Vector3 direction = (target - transform.position).normalized;
                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, target, step);

                // rigid body 写法
                // rigidbody.MovePosition(transform.position + direction * step);
                yield return null;
            }
        }
        if (finalRotate){
            // Vector3 tablePosition2d = new Vector3(table.transform.position.x, 0, table.transform.position.z);
            while (Vector3.Angle(transform.forward, finalFaceTowards - transform.position) > 1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(finalFaceTowards - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                yield return null;
            }
        }
        movementAudioSource.Stop();
    }

    public bool loopInterrupted;

    private IEnumerator MoveAlongPath_Loop_Coroutine(List<Vector3> targetList, float moveSpeed, float rotateSpeed)
    {
        // start to play the movement sound effect just before moving
        AudioSource movementAudioSource = gameObject.GetComponent<AudioSource>();
        movementAudioSource.loop = true;
        movementAudioSource.Play();

        loopInterrupted = false;
        while(true){
            foreach (Vector3 target in targetList) // for our 'next point'
            {
                Debug.Log("Next point is: " + target);
                // first rotate to face target
                while (Vector3.Angle(transform.forward, target - transform.position) > 1f)
                {
                    if (loopInterrupted){
                        movementAudioSource.Stop();
                        yield break;
                    }
                    // ensure that the rotation degree is less than 180
                    Quaternion targetRotation = Quaternion.LookRotation((target - transform.position).normalized);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                    
                    yield return null;
                }

                // then move to target position
                while (Vector3.Distance(transform.position, target) > 0.02f)
                {
                    if (loopInterrupted){
                        movementAudioSource.Stop();
                        yield break;
                    }
                    Vector3 direction = (target - transform.position).normalized;
                    float step = moveSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, target, step);

                    // rigid body 写法
                    // rigidbody.MovePosition(transform.position + direction * step);
                    yield return null;
                }
            }
        }
    }

    public void FlyAlongPath(List<Vector3> targetList, float moveSpeed, float rotateSpeed, bool finalRotate=false, Vector3 finalFaceTowards=default(Vector3))
    {
        StartCoroutine(FlyAlongPath_Coroutine(targetList, moveSpeed, rotateSpeed, finalRotate, finalFaceTowards));
    }

    public float flightHeight = 2f;

    private IEnumerator FlyAlongPath_Coroutine(List<Vector3> targetList, float moveSpeed, float rotateSpeed, bool finalRotate, Vector3 finalFaceTowards)
    {
        // start to play the movement sound effect just before moving

        foreach (Vector3 target in targetList)
        {
            // first lift up to the flight height
            Vector3 robotInFlightHeight = new Vector3(transform.position.x, flightHeight, transform.position.z);
            while (Vector3.Distance(transform.position, robotInFlightHeight) > 0.05f)
            {
                Debug.Log("+++++++" + Vector3.Distance(transform.position, robotInFlightHeight));
                Vector3 direction = new Vector3(0, 1, 0);
                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, moveSpeed * Time.deltaTime);
                yield return null;
            }
            Vector3 targetInFlightHeight = new Vector3(target.x, flightHeight, target.z);
            // then rotate to face target
            while (Vector3.Angle(transform.forward, targetInFlightHeight - transform.position) > 1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation((targetInFlightHeight - transform.position).normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                yield return null;
            }
            // then fly to target position (keep the flight height)
            while (Vector3.Distance(transform.position, targetInFlightHeight) > 0.02f)
            {
                Vector3 direction = (targetInFlightHeight - transform.position).normalized;
                transform.position = Vector3.MoveTowards(transform.position, targetInFlightHeight, moveSpeed * Time.deltaTime);
                yield return null;
            }
            // rotate to final face towards (but its y is changed to flightHeight)
            Vector3 finalFaceTowardsInFlightHeight = new Vector3(finalFaceTowards.x, flightHeight, finalFaceTowards.z);
            if (finalRotate){
                while (Vector3.Angle(transform.forward, finalFaceTowardsInFlightHeight - transform.position) > 1f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(finalFaceTowardsInFlightHeight - transform.position);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                    yield return null;
                }
            }
            // finally lower down to the target position
            while ((transform.position.y - target.y) > 0.02f)
            {
                Vector3 direction = new Vector3(0, -1, 0);
                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
