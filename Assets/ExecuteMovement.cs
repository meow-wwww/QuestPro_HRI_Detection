using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteMovement : MonoBehaviour
{
    RoutePlanning routePlanner;
    Rigidbody rigidbody;
    public ObjectPlacementInitialization globalPositionInfo; // assigned in Unity Inspector
    
    void Start()
    {
        routePlanner = GameObject.Find("MRUK").GetComponent<RoutePlanning>();
        rigidbody = gameObject.GetComponent<Rigidbody>();

        System.Diagnostics.Debug.Assert(globalPositionInfo != null, "globalPositionInfo is not assigned in Unity Inspector");
    }

    public IEnumerator PlanAndMoveTo(Vector3 destination, float moveSpeed, float rotateSpeed, bool finalRotate=false, Vector3 finalFaceTowards=default(Vector3))
    {
        yield return StartCoroutine(PlanAndMoveTo_Coroutine(destination, moveSpeed, rotateSpeed, finalRotate, finalFaceTowards));
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
        for (int i = 0; i < foundPath.Count; i++)
        {
            Vector3 target = foundPath[i];
            foundPath[i] = new Vector3(target.x, globalPositionInfo.floorHeight, target.z);
        }
        yield return MoveAlongPath_Coroutine(foundPath, moveSpeed, rotateSpeed, finalRotate, finalFaceTowards); 
        // it will wait for the coroutine to finish
    }

    public IEnumerator MoveAlongPath(List<Vector3> targetList, float moveSpeed, float rotateSpeed, bool finalRotate=false, Vector3 finalFaceTowards=default(Vector3), bool loop=false)
    {   
        for (int i = 0; i < targetList.Count; i++)
        {
            targetList[i] = new Vector3(targetList[i].x, globalPositionInfo.floorHeight, targetList[i].z);
        }
        
        // move gameObject to target position
        // moveSpeed: length per second
        // rotateSpeed: degrees per second
        if (loop)
            yield return StartCoroutine(MoveAlongPath_Loop_Coroutine(targetList, moveSpeed, rotateSpeed));
        else
            yield return StartCoroutine(MoveAlongPath_Coroutine(targetList, moveSpeed, rotateSpeed, finalRotate, finalFaceTowards));
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

                yield return null;
            }
        }
        if (finalRotate){
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
        movementAudioSource.loop = true; // TODO: can be deleted
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

    public void FlyAlongPath(List<Vector3> targetList, float moveSpeed, float rotateSpeed, bool finalRotate=false, Vector3 finalFaceTowards=default(Vector3), bool loop=false)
    {
        if (loop)
            StartCoroutine(FlyAlongPath_Loop_Coroutine(targetList, moveSpeed, rotateSpeed, finalRotate, finalFaceTowards));
        else
            StartCoroutine(FlyAlongPath_Coroutine(targetList, moveSpeed, rotateSpeed, finalRotate, finalFaceTowards));
    }

    public float flightHeight = 2f;

    private IEnumerator FlyAlongPath_Coroutine(List<Vector3> targetList, float moveSpeed, float rotateSpeed, bool finalRotate, Vector3 finalFaceTowards)
    {
        // start to play the movement sound effect just before moving
        gameObject.GetComponent<AudioSource>().Play();

        foreach (Vector3 target in targetList)
        {
            // Before start, check the distance in x-z plane between the robot and the target, and using this to decide the flight height.
            // (if the distance is too small, the robot will fly lower)
            float realFlightHeight;
            if (Vector3.Distance(transform.position, target) < 0.5f)
                realFlightHeight = globalPositionInfo.tableHeight + 0.4f;
            else
                realFlightHeight = globalPositionInfo.floorHeight + flightHeight;
            // first lift up to the flight height
            Vector3 robotInFlightHeight = new Vector3(transform.position.x, realFlightHeight, transform.position.z);
           
            while (Vector3.Distance(transform.position, robotInFlightHeight) > 0.05f)
            {   
                Debug.Log("======= first lift up to the flight height");
                Debug.Log("+++++++" + transform.position + robotInFlightHeight + "+++" + Vector3.Distance(transform.position, robotInFlightHeight));
                float realMoveSpeed = moveSpeed;
                if (Vector3.Distance(transform.position, robotInFlightHeight) < 0.15f)
                    realMoveSpeed = moveSpeed / 2;
                Vector3 direction = new Vector3(0, 1, 0);
                transform.position = Vector3.MoveTowards(transform.position, robotInFlightHeight, realMoveSpeed * Time.deltaTime);
                yield return null;
            }
            Vector3 targetInFlightHeight = new Vector3(target.x, realFlightHeight, target.z);
            // then rotate to face target
            while (Vector3.Angle(transform.forward, targetInFlightHeight - transform.position) > 1f && Vector3.Distance(transform.position, targetInFlightHeight) > 0.05f)
            {
                Debug.Log("======= then rotate to face target");
                Quaternion targetRotation = Quaternion.LookRotation((targetInFlightHeight - transform.position).normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                yield return null;
            }
            // then fly to target position (keep the flight height)
            while (Vector3.Distance(transform.position, targetInFlightHeight) > 0.02f)
            {
                Debug.Log("======= then fly to target position (keep the flight height)");
                Vector3 direction = (targetInFlightHeight - transform.position).normalized;
                transform.position = Vector3.MoveTowards(transform.position, targetInFlightHeight, moveSpeed * Time.deltaTime);
                yield return null;
            }
            // rotate to final face towards (but its y is changed to realFlightHeight)
            Vector3 finalFaceTowardsInFlightHeight = new Vector3(finalFaceTowards.x, realFlightHeight, finalFaceTowards.z);
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
        gameObject.GetComponent<AudioSource>().Stop();
    }

    private IEnumerator FlyAlongPath_Loop_Coroutine(List<Vector3> targetList, float moveSpeed, float rotateSpeed, bool finalRotate, Vector3 finalFaceTowards)
    {
        // start to play the movement sound effect just before moving
        gameObject.GetComponent<AudioSource>().Play();

        loopInterrupted = false;

        while (true){
            foreach (Vector3 target in targetList)
            {
                // Before start, check the distance in x-z plane between the robot and the target, and using this to decide the flight height.
                // (if the distance is too small, the robot will fly lower)
                float realFlightHeight;
                if (Vector3.Distance(transform.position, target) < 0.5f)
                    realFlightHeight = globalPositionInfo.tableHeight + 0.4f;
                else
                    realFlightHeight = flightHeight;

                // first lift up to the flight height
                Vector3 robotInFlightHeight = new Vector3(transform.position.x, realFlightHeight, transform.position.z);
                while (Vector3.Distance(transform.position, robotInFlightHeight) > 0.03f)
                {
                    float realMoveSpeed = moveSpeed;
                    if (Vector3.Distance(transform.position, robotInFlightHeight) < 0.1f)
                        realMoveSpeed = moveSpeed / 2;
                    if (loopInterrupted){
                        gameObject.GetComponent<AudioSource>().Stop();
                        yield break;
                    }
                    Vector3 direction = new Vector3(0, 1, 0);
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, realMoveSpeed * Time.deltaTime);
                    yield return null;
                }
                Vector3 targetInFlightHeight = new Vector3(target.x, realFlightHeight, target.z);
                // then rotate to face target
                while (Vector3.Angle(transform.forward, targetInFlightHeight - transform.position) > 1f)
                {
                    if (loopInterrupted){
                        gameObject.GetComponent<AudioSource>().Stop();
                        yield break;
                    }
                    Quaternion targetRotation = Quaternion.LookRotation((targetInFlightHeight - transform.position).normalized);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                    yield return null;
                }
                // then fly to target position (keep the flight height)
                while (Vector3.Distance(transform.position, targetInFlightHeight) > 0.02f)
                {
                    if (loopInterrupted){
                        gameObject.GetComponent<AudioSource>().Stop();
                        yield break;
                    }
                    Vector3 direction = (targetInFlightHeight - transform.position).normalized;
                    transform.position = Vector3.MoveTowards(transform.position, targetInFlightHeight, moveSpeed * Time.deltaTime);
                    yield return null;
                }
                // rotate to final face towards (but its y is changed to realFlightHeight)
                Vector3 finalFaceTowardsInFlightHeight = new Vector3(finalFaceTowards.x, realFlightHeight, finalFaceTowards.z);
                if (finalRotate){
                    while (Vector3.Angle(transform.forward, finalFaceTowardsInFlightHeight - transform.position) > 1f)
                    {
                        if (loopInterrupted){
                        gameObject.GetComponent<AudioSource>().Stop();
                        yield break;
                    }
                        Quaternion targetRotation = Quaternion.LookRotation(finalFaceTowardsInFlightHeight - transform.position);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                        yield return null;
                    }
                }
                // finally lower down to the target position
                while ((transform.position.y - target.y) > 0.02f)
                {
                    if (loopInterrupted){
                        gameObject.GetComponent<AudioSource>().Stop();
                        yield break;
                    }
                    Vector3 direction = new Vector3(0, -1, 0);
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, moveSpeed * Time.deltaTime);
                    yield return null;
                }
            }
        }
    }
}
