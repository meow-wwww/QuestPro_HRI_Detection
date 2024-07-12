using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPMoveToTable : MonoBehaviour
{
    public GameObject table;
    public float moveSpeed = 2f;
    public float rotateSpeed = 80f;

    // public GameObject drinkUser1, drinkUser2, drinkWrong;

    public GameObject currentDrink;

    Vector3 robotInitialPosition;
    // Start is called before the first frame update
    void Start()
    {
        robotInitialPosition = gameObject.transform.position;

        currentDrink = GameObject.Find("coffee_plate_user2");
        currentDrink.GetComponent<FollowPlate>().SetFollowPlate(true);
    }

    void Update(){
        if (table == null){
            table = GameObject.Find("TABLE");
        }
    }

    public void SetCurrentDrink(GameObject drink){
        GameObject plate = currentDrink.GetComponent<FollowPlate>().plate;
        currentDrink.GetComponent<FollowPlate>().SetFollowPlate(false);
        currentDrink = drink;
        currentDrink.transform.position = plate.transform.position;
        // currentDrink.transform.position -= gameObject.transform.forward * 0.13f;
        currentDrink.GetComponent<FollowPlate>().SetFollowPlate(true);
    }

    public void MoveToTableHalf(){
        Vector3 targetPositon = (gameObject.transform.position + table.transform.position) / 2f;
        gameObject.GetComponent<ExecuteMovement>().PlanAndMoveTo(targetPositon, moveSpeed, rotateSpeed);
    }

    public void MoveToTableUser1()
    {
        Vector3 targetPositon = table.transform.position + new Vector3(0.8f,0f,-0.1f);
        // GameObject.Find("TargetDebug").transform.position = targetPositon;
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        executor.PlanAndMoveTo(targetPositon, moveSpeed, rotateSpeed);
    }
    public void MoveToTableUser1Dangerous()
    {
        Vector3 targetPositon = table.transform.position + new Vector3(0.8f, 0f,-0.4f);
        // GameObject.Find("TargetDebug").transform.position = targetPositon;
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        executor.PlanAndMoveTo(targetPositon, moveSpeed, rotateSpeed);
        // executor.MoveAlongPath(new List<Vector3>{targetPositon}, moveSpeed, rotateSpeed);
    }
    public void AdjustToTableUser1FromDangerous()
    {
        Vector3 targetPositon = table.transform.position + new Vector3(0.8f, 0f,-0.1f);
        // GameObject.Find("TargetDebug").transform.position = targetPositon;
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        executor.PlanAndMoveTo(targetPositon, moveSpeed, rotateSpeed, true);
    }

    public void MoveToTableUser2()
    {
        Vector3 targetPositon = table.transform.position + new Vector3(0.8f,0f,0.1f);
        // GameObject.Find("TargetDebug").transform.position = targetPositon;
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        executor.PlanAndMoveTo(targetPositon, moveSpeed, rotateSpeed, true);
    }

    public void GoAway(){
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        // executor.PlanAndMoveTo(robotInitialPosition, moveSpeed, rotateSpeed);
        executor.MoveAlongPath(new List<Vector3>{new Vector3(robotInitialPosition.x, 0, robotInitialPosition.z)}, moveSpeed, rotateSpeed);
    }
}
