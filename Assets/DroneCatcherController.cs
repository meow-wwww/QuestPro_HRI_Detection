using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCatcherController : MonoBehaviour
{
    GameObject leftCatcher;
    GameObject rightCatcher;

    public float rotationSpeed = 6f;
    public float moveSpeed = 0.04f;

    // Start is called before the first frame update
    void Start()
    {
        leftCatcher = transform.Find("Catcher1").gameObject;
        rightCatcher = transform.Find("Catcher2").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator OpenCatcher(){
        Coroutine left = StartCoroutine(Rotate_Catcher_Coroutine(leftCatcher.transform.localRotation * Quaternion.Euler(-20f, 0f, 0f), leftCatcher, rotationSpeed));
        Coroutine right = StartCoroutine(Rotate_Catcher_Coroutine(rightCatcher.transform.localRotation * Quaternion.Euler(-20f, 0f, 0f), rightCatcher, rotationSpeed));
        yield return left;
        yield return right;
    }

    public IEnumerator CloseCatcher(){
        Coroutine left = StartCoroutine(Rotate_Catcher_Coroutine(leftCatcher.transform.localRotation * Quaternion.Euler(20f, 0f, 0f), leftCatcher, rotationSpeed, direction: -1));
        Coroutine right = StartCoroutine(Rotate_Catcher_Coroutine(rightCatcher.transform.localRotation * Quaternion.Euler(20f, 0f, 0f), rightCatcher, rotationSpeed, direction: -1));
        yield return left;
        yield return right;
    }

    public void LiftCatcher(float height){
        StartCoroutine(Vertically_Move_Catcher_Coroutine(leftCatcher, moveSpeed, height, direction: 1));
        StartCoroutine(Vertically_Move_Catcher_Coroutine(rightCatcher, moveSpeed, height, direction: 1));
    }

    public void LowerCatcher(float height){
        StartCoroutine(Vertically_Move_Catcher_Coroutine(leftCatcher, moveSpeed, height, direction: -1));
        StartCoroutine(Vertically_Move_Catcher_Coroutine(rightCatcher, moveSpeed, height, direction: -1));
    }

    public void ForwardCatcher(float distance){
        StartCoroutine(Horizontally_Move_Catcher_Coroutine(leftCatcher, moveSpeed, distance, direction: 1));
        StartCoroutine(Horizontally_Move_Catcher_Coroutine(rightCatcher, moveSpeed, distance, direction: 1));
    }

    public void BackwardCatcher(float distance){
        StartCoroutine(Horizontally_Move_Catcher_Coroutine(leftCatcher, moveSpeed, distance, direction: -1));
        StartCoroutine(Horizontally_Move_Catcher_Coroutine(rightCatcher, moveSpeed, distance, direction: -1));
    }

    private IEnumerator Horizontally_Move_Catcher_Coroutine(GameObject catcher, float moveSpeed, float length, int direction=1)
    {
        Vector3 initialPosition = catcher.transform.Find("CupCatcher").gameObject.transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(0,0, length*direction);
        while (Vector3.Distance(catcher.transform.Find("CupCatcher").gameObject.transform.position, targetPosition) > 0.01f)
        {
            catcher.transform.Find("CupCatcher").gameObject.transform.position = Vector3.MoveTowards(catcher.transform.Find("CupCatcher").gameObject.transform.position, targetPosition, moveSpeed*Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator Vertically_Move_Catcher_Coroutine(GameObject catcher, float moveSpeed, float length, int direction=1)
    {
        Vector3 initialPosition = catcher.transform.Find("CupCatcher").gameObject.transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(0, length*direction, 0);
        float startScaleY = catcher.transform.Find("CupCatcher").Find("HigherArmParent").localScale.y;
       
        while (Vector3.Distance(catcher.transform.Find("CupCatcher").gameObject.transform.position, targetPosition) > 0.01f)
        {
            catcher.transform.Find("CupCatcher").gameObject.transform.position = Vector3.MoveTowards(catcher.transform.Find("CupCatcher").gameObject.transform.position, targetPosition, moveSpeed*Time.deltaTime);
            // rescale the HigherArmParent (y axis).
            float movedDistance = Mathf.Abs(initialPosition.y - catcher.transform.Find("CupCatcher").gameObject.transform.position.y);
            catcher.transform.Find("CupCatcher").Find("HigherArmParent").localScale = new Vector3(1, startScaleY - movedDistance*direction*24f, 1);
            yield return null;
        }
    }

    private IEnumerator Rotate_Catcher_Coroutine(Quaternion targetRotation, GameObject catcher, float rotateSpeed, int direction=1)
    {
        while (Quaternion.Angle(catcher.transform.localRotation, targetRotation) > 0.5f)
        {
            float realRotateSpeed = rotateSpeed;
            if (Quaternion.Angle(catcher.transform.localRotation, targetRotation) < 2f)
                realRotateSpeed = rotateSpeed/2f;
            catcher.transform.localRotation = catcher.transform.localRotation * Quaternion.Euler(realRotateSpeed*Time.deltaTime*direction*-1, 0, 0);
            yield return null;
        }
    }
}
