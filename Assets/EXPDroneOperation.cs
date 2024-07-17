using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPDroneOperation : MonoBehaviour
{
    public GameObject table;
    public float moveSpeed = 2f;
    public float rotateSpeed = 30f;
    public ObjectPlacementInitialization globalPositionInfo; // assigned in Unity Inspector

    [Header("Object references")]
    public GameObject currentDrink = null;
    GameObject plate; // assigned in Unity Inspector
    public GameObject cupCatcher; // assigned in Unity Inspector

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update(){
        if (table == null){
            table = GameObject.Find("TABLE");
        }
    }

    // Drink and plate/cupCatcher related functions

    public void SetCurrentDrink(GameObject drink){
        // here the FollowPlate script is deprecated
        if (currentDrink != null) {
            currentDrink.transform.SetParent(null, worldPositionStays: true);
        }
        else{
            // if currentDrink's name == Coffee_wrong, disable it
            if (currentDrink != null && currentDrink.name == "Coffee_wrong"){
                currentDrink.SetActive(false);
            }
        }
        currentDrink = drink;
        if (drink != null){
            currentDrink.transform.position = cupCatcher.transform.position - new Vector3(0f, 0.17f, 0f);
            currentDrink.transform.SetParent(cupCatcher.transform, worldPositionStays: true);
        }
    }

    public void OpenCupCatcher(){
        cupCatcher.GetComponent<DroneCatcherController>().OpenCatcher();
        TryReleaseDrink();
    }

    public void CloseCupCatcher(bool collectDrink){
        cupCatcher.GetComponent<DroneCatcherController>().CloseCatcher();
        if (collectDrink){
            CollectDrink();
        }
    }

    public void TryReleaseDrink(){
        if (currentDrink != null && currentDrink.transform.IsChildOf(cupCatcher.transform)){
            currentDrink.transform.SetParent(null, worldPositionStays: true);
        }
    }

    public void CollectDrink(){
        currentDrink.transform.SetParent(cupCatcher.transform, worldPositionStays: true);
    }
    

    // public void OpenLifter(){
    //     gameObject.transform.Find("Lifters").GetComponent<DroneLifterController>().OpenLifter();
    //     TryReleasePlate();
    // }

    // public void CloseLifter(bool collectPlate){
    //     gameObject.transform.Find("Lifters").GetComponent<DroneLifterController>().CloseLifter();
    //     if (collectPlate){
    //         CollectPlate();
    //     }
    // }

    // void TryReleasePlate(){
    //     if (plate.transform.IsChildOf(gameObject.transform)){
    //         plate.transform.SetParent(null, worldPositionStays: true);
    //     }
    //     if (currentDrink != null && currentDrink.transform.IsChildOf(plate.transform)){
    //         transform.Find("DroneBody").GetComponent<AudioPlayer>().PlayAudio("Audio/Ding");
    //     }
    // }

    // void CollectPlate(){
    //     plate.transform.SetParent(gameObject.transform, worldPositionStays: true);
    // }


    // movement

    public void MoveToTableHalf(){
        Vector3 targetPosition = (gameObject.transform.position + table.transform.position) / 2f;
        targetPosition = new Vector3(targetPosition.x, gameObject.GetComponent<ExecuteMovement>().flightHeight, targetPosition.z);
        gameObject.GetComponent<ExecuteMovement>().FlyAlongPath(new List<Vector3>{targetPosition}, moveSpeed, rotateSpeed);
    }

    public void MoveToTableUser1(bool above=false)
    {
        Vector3 targetPosition = table.transform.position + 0f * globalPositionInfo.userRight - 0.1f * globalPositionInfo.userForward;
        if (above)
            targetPosition += new Vector3(0f,0.4f,0f);
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        executor.FlyAlongPath(new List<Vector3>{targetPosition}, moveSpeed, rotateSpeed, true, targetPosition - 0.1f * globalPositionInfo.userForward);
    }

    public void MoveToTableUser1Collision()
    {
        Vector3 targetPosition = table.transform.position + 0.8f * globalPositionInfo.userRight - 0.5f * globalPositionInfo.userForward;
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        executor.FlyAlongPath(new List<Vector3>{targetPosition}, moveSpeed, rotateSpeed, true, new Vector3(globalPositionInfo.userPosition.x, gameObject.GetComponent<ExecuteMovement>().flightHeight, globalPositionInfo.userPosition.z));
    }

    public void MoveToTableUser1Dangerous(){
        Vector3 targetPosition = table.transform.position + 0.3f * globalPositionInfo.userRight - 0.1f * globalPositionInfo.userForward;
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        executor.FlyAlongPath(new List<Vector3>{targetPosition}, moveSpeed, rotateSpeed, true, targetPosition - 0.1f * globalPositionInfo.userForward);
    }

    public void MoveUp(float height){
        Vector3 targetPosition = gameObject.transform.position + new Vector3(0, height, 0);
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        executor.FlyAlongPath(new List<Vector3>{targetPosition}, moveSpeed, rotateSpeed);
    }

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
            // globalPositionInfo.userPosition + globalPositionInfo.userRight * 1.0f + globalPositionInfo.userForward * 1.9f,
            // globalPositionInfo.userPosition + globalPositionInfo.userRight * 1.1f + globalPositionInfo.userForward * 0.5f,
            // globalPositionInfo.userPosition + globalPositionInfo.userRight * 1.0f + globalPositionInfo.userForward * 1.9f,
        };
        for (int i = 0; i < wanderPath.Count; i++)
            wanderPath[i] = new Vector3(wanderPath[i].x, gameObject.GetComponent<ExecuteMovement>().flightHeight, wanderPath[i].z);
        executor.FlyAlongPath(wanderPath, moveSpeed, rotateSpeed, false, new Vector3(0,0,0), loop: true);
    }

    public void StopWandering(){
        gameObject.GetComponent<ExecuteMovement>().loopInterrupted = true;
    }
}
