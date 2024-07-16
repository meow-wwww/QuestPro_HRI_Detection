using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPDroneOperation : MonoBehaviour
{
    public GameObject table;
    public float moveSpeed = 2f;
    public float rotateSpeed = 30f;
    public ObjectPlacementInitialization globalPositionInfo; // assigned in Unity Inspector


    public GameObject currentDrink = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update(){
        if (table == null){
            table = GameObject.Find("TABLE");
        }
    }

    public void SetCurrentDrink(GameObject drink){
        GameObject plate = null;
        if (currentDrink != null) {
            plate = currentDrink.GetComponent<FollowPlate>().plate;
            currentDrink.GetComponent<FollowPlate>().SetFollowPlate(false);
        }
        else {
            plate = GameObject.Find("plate_middle");
        }
        currentDrink = drink;
        currentDrink.transform.position = plate.transform.position - plate.transform.right * 0.16f;
        currentDrink.GetComponent<FollowPlate>().SetFollowPlate(true);
    }

    public void MoveToTableHalf(){
        Vector3 targetPosition = (gameObject.transform.position + table.transform.position) / 2f;
        targetPosition = new Vector3(targetPosition.x, gameObject.GetComponent<ExecuteMovement>().flightHeight, targetPosition.z);
        gameObject.GetComponent<ExecuteMovement>().FlyAlongPath(new List<Vector3>{targetPosition}, moveSpeed, rotateSpeed);
    }

    public void MoveToTableUser1()
    {
        Vector3 targetPosition = table.transform.position + 0f * globalPositionInfo.userRight - 0.1f * globalPositionInfo.userForward;
        // Vector3 targetLookAtPoint = new Vector3(targetPosition.)
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        executor.FlyAlongPath(new List<Vector3>{targetPosition}, moveSpeed, rotateSpeed, true, targetPosition - 0.1f * globalPositionInfo.userForward);
    }
    public void MoveToTableUser1Dangerous()
    {
        Vector3 targetPosition = table.transform.position + 0.8f * globalPositionInfo.userRight - 0.5f * globalPositionInfo.userForward;
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        executor.FlyAlongPath(new List<Vector3>{targetPosition}, moveSpeed, rotateSpeed, true, new Vector3(globalPositionInfo.userPosition.x, gameObject.GetComponent<ExecuteMovement>().flightHeight, globalPositionInfo.userPosition.z));
    }
    // public void AdjustToTableUser1FromDangerous()
    // {
    //     Vector3 targetPosition = table.transform.position + 0.8f * globalPositionInfo.userRight - 0.3f * globalPositionInfo.userForward;
    //     // GameObject.Find("TargetDebug").transform.position = targetPosition;
    //     ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
    //     executor.PlanAndMoveTo(targetPosition, moveSpeed, rotateSpeed, true);
    // }

    public void MoveToTableUser2()
    {
        Vector3 targetPosition = table.transform.position + 0f * globalPositionInfo.userRight + 0.1f * globalPositionInfo.userForward;
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        executor.FlyAlongPath(new List<Vector3>{targetPosition}, moveSpeed, rotateSpeed, true, targetPosition + 0.1f * globalPositionInfo.userForward);
    }

    public void GoAway(){
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        executor.FlyAlongPath(new List<Vector3>{new Vector3(globalPositionInfo.robotInitialPosition.x, 0, globalPositionInfo.robotInitialPosition.z)}, moveSpeed, rotateSpeed);
    }

    public void WanderAround(){
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        List<Vector3> wanderPath = new List<Vector3>{
            globalPositionInfo.userPosition + globalPositionInfo.userRight * 1.0f + globalPositionInfo.userForward * 1.9f,
            globalPositionInfo.userPosition + globalPositionInfo.userRight * 1.0f + globalPositionInfo.userForward * 0.5f,
            globalPositionInfo.userPosition + globalPositionInfo.userRight * 1.0f + globalPositionInfo.userForward * 1.9f,
            globalPositionInfo.userPosition + globalPositionInfo.userRight * 1.1f + globalPositionInfo.userForward * 0.5f,
            globalPositionInfo.userPosition + globalPositionInfo.userRight * 1.0f + globalPositionInfo.userForward * 1.9f,
        };
        for (int i = 0; i < wanderPath.Count; i++)
            wanderPath[i] = new Vector3(wanderPath[i].x, gameObject.GetComponent<ExecuteMovement>().flightHeight, wanderPath[i].z);
        executor.FlyAlongPath(wanderPath, moveSpeed, rotateSpeed, false, new Vector3(0,0,0));
    }

    public void StopWandering(){
        // gameObject.GetComponent<ExecuteMovement>().loopInterrupted = true;
    }
}
