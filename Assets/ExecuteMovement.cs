using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteMovement : MonoBehaviour
{
    RoutePlanning routePlanner;
    Rigidbody rigidbody;
    public ObjectPlacementInitialization globalPositionInfo; // assigned in Unity Inspector

    string initialMovingSoundEffect = "";
    
    void Start()
    {
        routePlanner = GameObject.Find("MRUK").GetComponent<RoutePlanning>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        initialMovingSoundEffect = gameObject.GetComponent<AudioSource>().clip.name;

        System.Diagnostics.Debug.Assert(globalPositionInfo != null, "globalPositionInfo is not assigned in Unity Inspector");
    }

    public IEnumerator CollisionMode(){
        gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/Warning");
        gameObject.GetComponent<AudioVolumeAdjustor>().volumeMode = "distance_full";
        yield return null;
    }

    public IEnumerator NormalMode(){
        gameObject.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/" + initialMovingSoundEffect);
        gameObject.GetComponent<AudioVolumeAdjustor>().volumeMode = "distance_half";
        yield return null;
    }

    public IEnumerator MoveAlongPath_Coroutine(List<Vector3> targetList, float moveSpeed, float rotateSpeed, bool finalRotate=false, Vector3 finalFaceTowards=default(Vector3), bool accelerate=false)
    {
        // start to play the movement sound effect just before moving
        AudioSource movementAudioSource = gameObject.GetComponent<AudioSource>();
        movementAudioSource.loop = true;
        movementAudioSource.Play();

        float accelerateRate = 1.0f;

        foreach (Vector3 target in targetList)
        {
            // first rotate to face target (only when the moving distance in this step is large enough)
            if (Vector3.Distance(transform.position, target) > 0.3f)
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
                if (accelerate){
                    accelerateRate *= 1.005f;
                }
                float step = moveSpeed * Time.deltaTime * accelerateRate;
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

    public IEnumerator MoveAlongPath_Loop_Coroutine(List<Vector3> targetList, float moveSpeed, float rotateSpeed)
    {
        // start to play the movement sound effect just before moving
        AudioSource movementAudioSource = gameObject.GetComponent<AudioSource>();
        movementAudioSource.loop = true; // TODO: can be deleted
        movementAudioSource.Play();

        loopInterrupted = false;
        while(true){
            foreach (Vector3 target in targetList) // for our 'next point'
            {
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

    public float flightHeight = 2f;

    public IEnumerator FlyAlongPath_Coroutine(List<Vector3> targetList, float moveSpeed, float rotateSpeed, bool finalRotate=false, Vector3 finalFaceTowards=default(Vector3), bool flyInStableHeight=false, float stableHeight=0f, bool accelerate=false)
    {
        // start to play the movement sound effect just before moving
        gameObject.GetComponent<AudioSource>().Play();
        float accelerateRate = 1.0f;

        foreach (Vector3 target in targetList)
        {
            // Before start, check the distance in x-z plane between the robot and the target, and using this to decide the flight height.
            // (if the distance is too small, the robot will fly lower)
            float realFlightHeight;
            if (flyInStableHeight){
                realFlightHeight = stableHeight;
            }
            else{
                if (Vector3.Distance(transform.position, target) < 0.5f)
                    realFlightHeight = globalPositionInfo.tableHeight + 0.55f;
                else
                    realFlightHeight = globalPositionInfo.floorHeight + flightHeight;
            }
            
            // first lift up to the flight height
            Vector3 robotInFlightHeight = new Vector3(transform.position.x, realFlightHeight, transform.position.z);
           
            while (Vector3.Distance(transform.position, robotInFlightHeight) > 0.05f)
            {   
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
                Quaternion targetRotation = Quaternion.LookRotation((targetInFlightHeight - transform.position).normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                yield return null;
            }
            // then fly to target position (keep the flight height)
            while (Vector3.Distance(transform.position, targetInFlightHeight) > 0.02f)
            {
                Vector3 direction = (targetInFlightHeight - transform.position).normalized;
                if (accelerate)
                    accelerateRate *= 1.01f;
                transform.position = Vector3.MoveTowards(transform.position, targetInFlightHeight, moveSpeed * Time.deltaTime * accelerateRate);
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
            if (!flyInStableHeight){
                while ((transform.position.y - target.y) > 0.02f)
                {
                    Vector3 direction = new Vector3(0, -1, 0);
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, moveSpeed * Time.deltaTime);
                    yield return null;
                }
            }
        }
        gameObject.GetComponent<AudioSource>().Stop();
    }

    public IEnumerator FlyAlongPath_Loop_Coroutine(List<Vector3> targetList, float moveSpeed, float rotateSpeed, bool finalRotate, Vector3 finalFaceTowards)
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
                    realFlightHeight = globalPositionInfo.tableHeight + 0.55f;
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
