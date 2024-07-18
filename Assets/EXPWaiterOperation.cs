using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPWaiterOperation : MonoBehaviour
{
    public GameObject table;
    public float moveSpeed = 0.5f;
    public float rotateSpeed = 30f;
    public ObjectPlacementInitialization globalPositionInfo; // assigned in Unity Inspector


    public GameObject currentDrink = null;
    public GameObject cupCatcher; // assigned in Unity Inspector

    WaiterCatcherController controller;

    // Start is called before the first frame update
    void Start()
    {
        System.Diagnostics.Debug.Assert(cupCatcher != null, "CupCatcher is not assigned in Unity Inspector");
        controller = gameObject.transform.Find("CupCatcher").gameObject.GetComponent<WaiterCatcherController>();
    }

    void Update(){
        if (table == null){
            table = GameObject.Find("TABLE");
        }
    }

    public void SetCurrentDrink(GameObject drink){
        // FollowPlate script can be deprecated;
        if (currentDrink != null) {
            // plate = currentDrink.GetComponent<FollowPlate>().plate;
            // currentDrink.GetComponent<FollowPlate>().SetFollowPlate(false);
            currentDrink.transform.SetParent(null, worldPositionStays: true);
        }
        // if currentDrink's name == Coffee_wrong, disable it
        if (currentDrink != null && currentDrink.name == "Coffee_wrong"){
            currentDrink.SetActive(false);
        }
        currentDrink = drink;
        if (currentDrink != null){
            currentDrink.transform.position = cupCatcher.transform.position - new Vector3(0f, 0.17f, 0f);
            currentDrink.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            currentDrink.transform.SetParent(cupCatcher.transform.Find("Catcher1").Find("CupCatcher").Find("FrontEndpoint"), worldPositionStays: true);
        }
    }

    public IEnumerator CurrentDrinkDetach(){
        if (currentDrink != null){
            currentDrink.transform.SetParent(null, worldPositionStays: true);
            currentDrink = null;
        }
        yield return null;
    }

    public IEnumerator CurrentDrinkAttach(){
        if (currentDrink != null){
            currentDrink.transform.SetParent(cupCatcher.transform.Find("Catcher1").Find("CupCatcher").Find("FrontEndpoint"), worldPositionStays: true);
        }
        yield return null;
    }

    public void SendOutDrink(){
        StartCoroutine(
            controller.WaitForCoroutinesToEnd(new List<IEnumerator>(){
                controller.LiftCatcher(0.03f),
                controller.ForwardCatcher(0.3f),
                controller.LowerCatcher(0.03f),
                CurrentDrinkDetach(),
                controller.OpenCatcher(),
                controller.LiftCatcher(0.03f),
                controller.CloseCatcher(),
                controller.BackwardCatcher(0.3f),
                controller.LowerCatcher(0.03f)
            })
        );
    }

    public void MoveToTableHalf(){
        Vector3 targetPosition = (gameObject.transform.position + table.transform.position) / 2f;
        targetPosition = new Vector3(targetPosition.x, globalPositionInfo.floorHeight, targetPosition.z);
        StartCoroutine(gameObject.GetComponent<ExecuteMovement>().PlanAndMoveTo(targetPosition, moveSpeed, rotateSpeed));
    }

    public void MoveToTableUser1(bool rigid=false)
    {
        Vector3 tablePosition2d = new Vector3(table.transform.position.x, 0, table.transform.position.z);
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        if (!rigid){
            Vector3 targetPosition = table.transform.position + 0.8f * globalPositionInfo.userRight - 0.7f * globalPositionInfo.userForward;
            executor.PlanAndMoveTo(targetPosition, moveSpeed, rotateSpeed, true, tablePosition2d - 0.1f * globalPositionInfo.userForward);
        }
        else{
            Vector3 targetPosition = table.transform.position + 1.2f * globalPositionInfo.userRight - 1.0f * globalPositionInfo.userForward;
            StartCoroutine(
                controller.WaitForCoroutinesToEnd(
                    new List<IEnumerator>(){
                        executor.PlanAndMoveTo(targetPosition, moveSpeed, rotateSpeed, true, tablePosition2d), // - 0.1f * globalPositionInfo.userForward),
                        executor.MoveAlongPath(new List<Vector3>{tablePosition2d + 0.8f * globalPositionInfo.userRight - 0.8f * globalPositionInfo.userForward}, moveSpeed, rotateSpeed, true, tablePosition2d) // - 0.1f * globalPositionInfo.userForward)
                    }
                )
            );
        }
    }

    public void MoveToTableUser1Collision()
    {
        Vector3 targetPosition = globalPositionInfo.userPosition + 1f * globalPositionInfo.userRight;
        // table.transform.position + 0.8f * globalPositionInfo.userRight - 0.5f * globalPositionInfo.userForward;
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        StartCoroutine(executor.PlanAndMoveTo(targetPosition, moveSpeed*2, rotateSpeed, true, globalPositionInfo.userPosition));
    }
    public void MoveToTableUser1Dangerous(bool rigid=false)
    {
        Vector3 tablePosition2d = new Vector3(table.transform.position.x, 0, table.transform.position.z);
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        if (!rigid){
            Vector3 targetPosition = table.transform.position + 0.8f * globalPositionInfo.userRight - 0.7f * globalPositionInfo.userForward;
            executor.PlanAndMoveTo(targetPosition, moveSpeed, rotateSpeed, true, tablePosition2d - 0.1f * globalPositionInfo.userForward);
        }
        else{
            Vector3 targetPosition = table.transform.position + 1.2f * globalPositionInfo.userRight - 1.0f * globalPositionInfo.userForward;
            StartCoroutine(
                controller.WaitForCoroutinesToEnd(
                    new List<IEnumerator>(){
                        executor.PlanAndMoveTo(targetPosition, moveSpeed, rotateSpeed, true, tablePosition2d), // - 0.1f * globalPositionInfo.userForward),
                        executor.MoveAlongPath(new List<Vector3>{tablePosition2d + 0.8f * globalPositionInfo.userRight - 0.8f * globalPositionInfo.userForward}, moveSpeed, rotateSpeed, true, tablePosition2d) // - 0.1f * globalPositionInfo.userForward)
                    }
                )
            );
        }
        currentDrink.GetComponent<DrinkAction>().Dangerous();
    }

    public void MoveToTableUser2(bool rigid=false)
    {
        
        Vector3 tablePosition2d = new Vector3(table.transform.position.x, 0, table.transform.position.z);
        // GameObject.Find("TargetDebug").transform.position = targetPosition;
        
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        if (!rigid) {
            Vector3 targetPosition = table.transform.position + 0.9f * globalPositionInfo.userRight + 0.9f * globalPositionInfo.userForward;
            executor.PlanAndMoveTo(targetPosition, moveSpeed, rotateSpeed, true, tablePosition2d + 0.1f * globalPositionInfo.userForward);
        }
        else{
            Vector3 targetPosition = table.transform.position + 1.2f * globalPositionInfo.userRight + 1.2f * globalPositionInfo.userForward;
            StartCoroutine(
                controller.WaitForCoroutinesToEnd(
                    new List<IEnumerator>(){
                        // start position is user1
                        executor.MoveAlongPath(new List<Vector3>{tablePosition2d + 1.2f * globalPositionInfo.userRight - 0.8f * globalPositionInfo.userForward}, moveSpeed, rotateSpeed),
                        executor.PlanAndMoveTo(targetPosition, moveSpeed, rotateSpeed, true, tablePosition2d),
                        executor.MoveAlongPath(new List<Vector3>{tablePosition2d + 0.75f * globalPositionInfo.userRight + 0.75f * globalPositionInfo.userForward}, moveSpeed, rotateSpeed, true, tablePosition2d)
                    }
                )
            );
        }
    }

    public void GoAway(){
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        StartCoroutine(executor.MoveAlongPath(new List<Vector3>{new Vector3(globalPositionInfo.robotInitialPosition.x, 0, globalPositionInfo.robotInitialPosition.z)}, moveSpeed, rotateSpeed));
    }

    public void WanderAround(){
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        List<Vector3> wanderPath = new List<Vector3>{
            globalPositionInfo.userPosition + globalPositionInfo.userRight * 2.0f + globalPositionInfo.userForward * 1.9f,
            globalPositionInfo.userPosition + globalPositionInfo.userRight * 2.0f + globalPositionInfo.userForward * 0.5f,
        };
        StartCoroutine(executor.MoveAlongPath(wanderPath, moveSpeed, rotateSpeed, false, new Vector3(0,0,0), loop: true));
    }

    public void StopWandering(){
        gameObject.GetComponent<ExecuteMovement>().loopInterrupted = true;
    }
}