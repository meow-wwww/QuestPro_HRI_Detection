using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidCatcherController : MonoBehaviour
{
    GameObject leftCatcher, rightCatcher;

    public float rotationSpeed = 6f;
    public float moveSpeed = 0.15f;
    public float openWidth = 0.05f;

    public float Arm1Length, Arm2Length;
    public float Arm1LocalScaleY, Arm2LocalScaleY;

    public IEnumerator WaitForCoroutinesToEnd(List<IEnumerator> coroutines)
    {
        foreach (IEnumerator coroutine in coroutines)
        {
            yield return StartCoroutine(coroutine);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        leftCatcher = transform.Find("Catcher1").gameObject;
        rightCatcher = transform.Find("Catcher2").gameObject;

        Arm1Length = Vector3.Distance(
            leftCatcher.transform.Find("Arm1Move").Find("Arm1Scale").Find("Arm1").Find("InnerPoint").position,
            leftCatcher.transform.Find("Arm1Move").Find("Arm1Scale").Find("Arm1").Find("OuterPoint").position
        );
        Arm2Length = Vector3.Distance(
            leftCatcher.transform.Find("Arm1Move").Find("Corner1").Find("Arm2Move").Find("Arm2Scale").Find("Arm2").Find("InnerPoint").position,
            leftCatcher.transform.Find("Arm1Move").Find("Corner1").Find("Arm2Move").Find("Arm2Scale").Find("Arm2").Find("OuterPoint").position
        );
        Arm1LocalScaleY = leftCatcher.transform.Find("Arm1Move").Find("Arm1Scale").localScale.y;
        Arm2LocalScaleY = leftCatcher.transform.Find("Arm1Move").Find("Corner1").Find("Arm2Move").Find("Arm2Scale").localScale.y;
    }

    public IEnumerator OpenCatcher(){
        Coroutine leftCoroutine = StartCoroutine(Horizontally_Move_Catcher_Coroutine(leftCatcher, moveSpeed, openWidth, 1));
        Coroutine rightCoroutine = StartCoroutine(Horizontally_Move_Catcher_Coroutine(rightCatcher, moveSpeed, openWidth, 1));
        yield return leftCoroutine;
        yield return rightCoroutine;
    }

    public IEnumerator CloseCatcher(){
        Coroutine leftCoroutine = StartCoroutine(Horizontally_Move_Catcher_Coroutine(leftCatcher, moveSpeed, openWidth, -1));
        Coroutine rightCoroutine = StartCoroutine(Horizontally_Move_Catcher_Coroutine(rightCatcher, moveSpeed, openWidth, -1));
        yield return leftCoroutine;
        yield return rightCoroutine;
    }

    public IEnumerator Arm1LengthChange(float length, int direction){
        Coroutine leftCoroutine = StartCoroutine(Arm1_LengthChange_Coroutine(leftCatcher, moveSpeed, length, direction));
        Coroutine rightCoroutine = StartCoroutine(Arm1_LengthChange_Coroutine(rightCatcher, moveSpeed, length, direction));
        yield return leftCoroutine;
        yield return rightCoroutine;
    }

    public IEnumerator Arm2LengthChange(float length, int direction){
        Coroutine leftCoroutine = StartCoroutine(Arm2_LengthChange_Coroutine(leftCatcher, moveSpeed, length, direction));
        Coroutine rightCoroutine = StartCoroutine(Arm2_LengthChange_Coroutine(rightCatcher, moveSpeed, length, direction));
        yield return leftCoroutine;
        yield return rightCoroutine;
    }



    private IEnumerator Horizontally_Move_Catcher_Coroutine(GameObject catcher, float moveSpeed, float length, int direction){
        Vector3 targetPosition = catcher.transform.position + catcher.transform.forward * direction * length;
        while(Vector3.Distance(catcher.transform.position, targetPosition) > 0.01f){
            catcher.transform.position = Vector3.MoveTowards(catcher.transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator Arm1_LengthChange_Coroutine(GameObject catcher, float moveSpeed, float length, int direction){
        // Arm1Move move forward = length
        GameObject arm1Move = catcher.transform.Find("Arm1Move").gameObject;
        GameObject arm1Scale = arm1Move.transform.Find("Arm1Scale").gameObject;
        GameObject arm1Fix_InnerPoint = catcher.transform.Find("Arm1Fix_InnerPoint").gameObject;
        
        Vector3 targetPosition = arm1Move.transform.position + arm1Move.transform.forward * direction * length;

        while (Vector3.Distance(arm1Move.transform.position, targetPosition) > 0.01f){
            arm1Move.transform.position = Vector3.MoveTowards(arm1Move.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // rescale Arm1Scale Y
            Vector3 arm1Fix_InnerPoint_Position = arm1Fix_InnerPoint.transform.position;
            float expectedLength = Vector3.Distance(arm1Scale.transform.Find("Arm1").Find("OuterPoint").position, arm1Fix_InnerPoint_Position);
            arm1Scale.transform.localScale = new Vector3(arm1Scale.transform.localScale.x, expectedLength / Arm1Length * Arm1LocalScaleY, arm1Scale.transform.localScale.z);

            yield return null;
        }
    }

    private IEnumerator Arm2_LengthChange_Coroutine(GameObject catcher, float moveSpeed, float length, int direction){
        GameObject arm2Move = catcher.transform.Find("Arm1Move").Find("Corner1").Find("Arm2Move").gameObject;
        GameObject arm2Scale = arm2Move.transform.Find("Arm2Scale").gameObject;
        GameObject arm2Fix_InnerPoint = catcher.transform.Find("Arm1Move").Find("Corner1").Find("Arm2Fix_InnerPoint").gameObject;

        Vector3 targetPosition = arm2Move.transform.position + arm2Move.transform.forward * direction * length;

        while (Vector3.Distance(arm2Move.transform.position, targetPosition) > 0.01f){
            arm2Move.transform.position = Vector3.MoveTowards(arm2Move.transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // rescale Arm2Scale Y
            Vector3 arm2Fix_InnerPoint_Position = arm2Fix_InnerPoint.transform.position;
            float expectedLength = Vector3.Distance(arm2Scale.transform.Find("Arm2").Find("OuterPoint").position, arm2Fix_InnerPoint_Position);
            arm2Scale.transform.localScale = new Vector3(arm2Scale.transform.localScale.x, expectedLength / Arm2Length * Arm2LocalScaleY, arm2Scale.transform.localScale.z);

            yield return null;
        }
    }
}
