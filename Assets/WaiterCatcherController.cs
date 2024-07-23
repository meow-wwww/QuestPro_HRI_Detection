using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiterCatcherController : MonoBehaviour
{
    GameObject leftCatcher;
    GameObject rightCatcher;

    public float rotationSpeed = 6f;
    public float moveSpeed = 0.15f;
    public float openWidth = 0.05f;

    Vector3 catcherHorizontalLocalPositionOnStart = new Vector3(10000f, 10000f, 10000f);
    Dictionary<GameObject, Vector3> catcherForwardLocalPositionOnStart = new Dictionary<GameObject, Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        leftCatcher = transform.Find("Catcher1").gameObject;
        rightCatcher = transform.Find("Catcher2").gameObject;
        catcherForwardLocalPositionOnStart[leftCatcher] = new Vector3(10000f, 10000f, 10000f);
        catcherForwardLocalPositionOnStart[rightCatcher] = new Vector3(10000f, 10000f, 10000f);
    }

    public IEnumerator WaitForCoroutinesToEnd(List<IEnumerator> coroutines)
    {
        foreach (IEnumerator coroutine in coroutines)
        {
            yield return StartCoroutine(coroutine);
        }
    }

    public IEnumerator OpenCatcher(){
        Coroutine leftCoroutine = StartCoroutine(Horizontally_Move_Catcher_Coroutine(leftCatcher, moveSpeed, openWidth, direction: 1));
        Coroutine rightCoroutine = StartCoroutine(Horizontally_Move_Catcher_Coroutine(rightCatcher, moveSpeed, openWidth, direction: 1));
        yield return leftCoroutine;
        yield return rightCoroutine;
    }

    public IEnumerator CloseCatcher(){
        Coroutine leftCoroutine = StartCoroutine(Horizontally_Move_Catcher_Coroutine(leftCatcher, moveSpeed, openWidth, direction: -1));
        Coroutine rightCoroutine = StartCoroutine(Horizontally_Move_Catcher_Coroutine(rightCatcher, moveSpeed, openWidth, direction: -1));
        yield return leftCoroutine;
        yield return rightCoroutine;
    }

    public IEnumerator LiftCatcher(float height){
        Coroutine leftCoroutine = StartCoroutine(Vertically_Move_Catcher_Coroutine(leftCatcher, moveSpeed, height, direction: 1));
        Coroutine rightCoroutine = StartCoroutine(Vertically_Move_Catcher_Coroutine(rightCatcher, moveSpeed, height, direction: 1));
        yield return leftCoroutine;
        yield return rightCoroutine;
    }

    public IEnumerator LowerCatcher(float height){
        Coroutine leftCoroutine = StartCoroutine(Vertically_Move_Catcher_Coroutine(leftCatcher, moveSpeed, height, direction: -1));
        Coroutine rightCoroutine = StartCoroutine(Vertically_Move_Catcher_Coroutine(rightCatcher, moveSpeed, height, direction: -1));
        yield return leftCoroutine;
        yield return rightCoroutine;
    }

    public IEnumerator ForwardCatcher(float distance){
        Coroutine leftCoroutine = StartCoroutine(Forward_Move_Catcher_Coroutine(leftCatcher, moveSpeed*1.5f, distance, direction: 1));
        Coroutine rightCoroutine = StartCoroutine(Forward_Move_Catcher_Coroutine(rightCatcher, moveSpeed*1.5f, distance, direction: 1));
        yield return leftCoroutine;
        yield return rightCoroutine;
    }

    public IEnumerator BackwardCatcher(float distance){
        Coroutine leftCoroutine = StartCoroutine(Forward_Move_Catcher_Coroutine(leftCatcher, moveSpeed*1.5f, distance, direction: -1));
        Coroutine rightCoroutine = StartCoroutine(Forward_Move_Catcher_Coroutine(rightCatcher, moveSpeed*1.5f, distance, direction: -1));
        yield return leftCoroutine;
        yield return rightCoroutine;
    }


    // Core logic for moving the catcher

    private IEnumerator Horizontally_Move_Catcher_Coroutine(GameObject catcher, float moveSpeed, float length, int direction=1)
    {
        if (catcherHorizontalLocalPositionOnStart == new Vector3(10000f, 10000f, 10000f))
            catcherHorizontalLocalPositionOnStart = catcher.transform.Find("CupCatcher").gameObject.transform.localPosition;

        // direction 1: open catcher; arm shortened; 
        // direction -1: close catcher; arm extended;
        GameObject horizontalArm = catcher.transform.Find("CupCatcher").Find("HigherArmParentHorizontal").gameObject; // original length: 0.13; scale y: 3

        Vector3 initialPosition = catcher.transform.Find("CupCatcher").gameObject.transform.localPosition;
        Vector3 targetPosition = initialPosition + new Vector3(0,0, length*direction);

        while (Vector3.Distance(catcher.transform.Find("CupCatcher").gameObject.transform.localPosition, targetPosition) > 0.01f)
        {
            catcher.transform.Find("CupCatcher").gameObject.transform.localPosition = Vector3.MoveTowards(catcher.transform.Find("CupCatcher").gameObject.transform.localPosition, targetPosition, moveSpeed*Time.deltaTime);
            // rescale the horizontalArm
            float movedDistance = Mathf.Abs(catcherHorizontalLocalPositionOnStart.z - catcher.transform.Find("CupCatcher").gameObject.transform.localPosition.z);
            horizontalArm.transform.localScale = new Vector3(1, (0.13f - movedDistance)/0.13f*3f, 1);
            yield return null;
        }
    }

    private IEnumerator Forward_Move_Catcher_Coroutine(GameObject catcher, float moveSpeed, float length, int direction=1)
    {
        if (direction == 1)
            catcherForwardLocalPositionOnStart[catcher] = catcher.transform.Find("CupCatcher").Find("FrontEndpoint").gameObject.transform.position;
        // direction = 1: forward; lengthen;
        // direction = -1: backward; shorten;
        GameObject forwardArm = catcher.transform.Find("CupCatcher").Find("HigherArmParentForward").gameObject; // original length: 0.093; scale y: 2.2

        Vector3 initialPosition = catcher.transform.Find("CupCatcher").Find("FrontEndpoint").gameObject.transform.position;
        Vector3 targetPosition = initialPosition - catcher.transform.Find("CupCatcher").Find("FrontEndpoint").gameObject.transform.right * length*direction;

        while (Vector3.Distance(catcher.transform.Find("CupCatcher").Find("FrontEndpoint").gameObject.transform.position, targetPosition) > 0.01f)
        {
            catcher.transform.Find("CupCatcher").Find("FrontEndpoint").gameObject.transform.position = Vector3.MoveTowards(catcher.transform.Find("CupCatcher").Find("FrontEndpoint").gameObject.transform.position, targetPosition, moveSpeed*Time.deltaTime);
            // rescale the forwardArm
            float movedDistance = Vector3.Distance(catcherForwardLocalPositionOnStart[catcher], catcher.transform.Find("CupCatcher").Find("FrontEndpoint").gameObject.transform.position);
            forwardArm.transform.localScale = new Vector3(1, (0.093f + movedDistance)/0.093f*2.2f, 1);
            yield return null;
        }
        
    }

    private IEnumerator Vertically_Move_Catcher_Coroutine(GameObject catcher, float moveSpeed, float length, int direction=1)
    {
        Vector3 initialPosition = catcher.transform.Find("CupCatcher").gameObject.transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(0, length*direction, 0);
       
        while (Vector3.Distance(catcher.transform.Find("CupCatcher").gameObject.transform.position, targetPosition) > 0.01f)
        {
            catcher.transform.Find("CupCatcher").gameObject.transform.position = Vector3.MoveTowards(catcher.transform.Find("CupCatcher").gameObject.transform.position, targetPosition, moveSpeed*Time.deltaTime);
            yield return null;
        }
    }

    
}
