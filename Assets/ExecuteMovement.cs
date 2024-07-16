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
        // change the expression to 'sleep'
        gameObject.transform.Find("Body").Find("screen").gameObject.GetComponent<RobotScreenNotification>().SetScreenImage("CatSleep");

        // start to play the movement sound effect just before moving
        AudioPlayer audioPlayer = gameObject.GetComponent<AudioPlayer>();
        audioPlayer.PlayAudio("Audio/Movement", true);

        Debug.Log("MoveAlongPath_Coroutine started, initial position:" + gameObject.transform.position);
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
            while (Vector3.Distance(transform.position, target) > 0.01f)
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
        audioPlayer.StopAudio();
    }

    public bool loopInterrupted;

    private IEnumerator MoveAlongPath_Loop_Coroutine(List<Vector3> targetList, float moveSpeed, float rotateSpeed)
    {
        // change the expression to 'sleep'
        gameObject.transform.Find("Body").Find("screen").gameObject.GetComponent<RobotScreenNotification>().SetScreenImage("CatSleep");

        // start to play the movement sound effect just before moving
        AudioPlayer audioPlayer = gameObject.GetComponent<AudioPlayer>();
        audioPlayer.PlayAudio("Audio/Movement", true);

        loopInterrupted = false;
        while(true){
            foreach (Vector3 target in targetList) // for our 'next point'
            {
                Debug.Log("Next point is: " + target);
                // first rotate to face target
                while (Vector3.Angle(transform.forward, target - transform.position) > 1f)
                {
                    if (loopInterrupted){
                        audioPlayer.StopAudio();
                        yield break;
                    }
                    // ensure that the rotation degree is less than 180
                    Quaternion targetRotation = Quaternion.LookRotation((target - transform.position).normalized);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                    
                    yield return null;
                }

                // then move to target position
                while (Vector3.Distance(transform.position, target) > 0.01f)
                {
                    if (loopInterrupted){
                        audioPlayer.StopAudio();
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
}
