using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPMoveToTable : MonoBehaviour
{
    public GameObject table;
    public float moveSpeed = 2f;
    public float rotateSpeed = 80f;
    public ObjectPlacementInitialization globalPositionInfo;


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
        Vector3 targetPositon = (gameObject.transform.position + table.transform.position) / 2f;
        gameObject.GetComponent<ExecuteMovement>().PlanAndMoveTo(targetPositon, moveSpeed, rotateSpeed);
    }

    public void MoveToTableUser1()
    {
        Vector3 targetPositon = table.transform.position + 0.75f * globalPositionInfo.userRight - 0.17f * globalPositionInfo.userForward;
        Vector3 tablePosition2d = new Vector3(table.transform.position.x, 0, table.transform.position.z);
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        executor.PlanAndMoveTo(targetPositon, moveSpeed, rotateSpeed, true, tablePosition2d - 0.17f * globalPositionInfo.userForward);
    }
    public void MoveToTableUser1Dangerous()
    {
        Vector3 targetPositon = table.transform.position + 0.8f * globalPositionInfo.userRight - 0.5f * globalPositionInfo.userForward;
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        executor.PlanAndMoveTo(targetPositon, moveSpeed, rotateSpeed, true, globalPositionInfo.userPosition);
    }
    // public void AdjustToTableUser1FromDangerous()
    // {
    //     Vector3 targetPositon = table.transform.position + 0.8f * globalPositionInfo.userRight - 0.3f * globalPositionInfo.userForward;
    //     // GameObject.Find("TargetDebug").transform.position = targetPositon;
    //     ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
    //     executor.PlanAndMoveTo(targetPositon, moveSpeed, rotateSpeed, true);
    // }

    public void MoveToTableUser2()
    {
        Vector3 targetPositon = table.transform.position + 0.75f * globalPositionInfo.userRight + 0.17f * globalPositionInfo.userForward;
        Vector3 tablePosition2d = new Vector3(table.transform.position.x, 0, table.transform.position.z);
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        executor.PlanAndMoveTo(targetPositon, moveSpeed, rotateSpeed, true, tablePosition2d + 0.17f * globalPositionInfo.userForward);
    }

    public void GoAway(){
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        executor.MoveAlongPath(new List<Vector3>{new Vector3(globalPositionInfo.robotInitialPosition.x, 0, globalPositionInfo.robotInitialPosition.z)}, moveSpeed, rotateSpeed);
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
        executor.MoveAlongPath(wanderPath, moveSpeed, rotateSpeed, false, new Vector3(0,0,0), true);
    }

    public void StopWandering(){
        gameObject.GetComponent<ExecuteMovement>().loopInterrupted = true;
    }
}
