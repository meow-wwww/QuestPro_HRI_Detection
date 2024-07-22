using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPWaiterOperation : MonoBehaviour
{
    [Header("Object references")]
    public GameObject table = null;
    public GameObject currentDrink = null;
    public GameObject cupCatcher; // assigned in Unity Inspector

    [Header("Robot Movement Parameters")]
    public float moveSpeed = 0.5f;
    public float rotateSpeed = 30f;

    [Header("Fixed Positions In Routes")]
    public ObjectPlacementInitialization globalPositionInfo; // assigned in Unity Inspector

    public Vector3 middlePoint; // 1.7 right
    public Vector3 user1Peripheral;
    public Vector3 user2Peripheral;
    public Vector3 user1Near;
    public Vector3 user2Near;
    public Vector3 user1Collision;

    WaiterCatcherController controller;

    // Start is called before the first frame update
    void Start()
    {
        System.Diagnostics.Debug.Assert(globalPositionInfo != null, "GlobalPositionInfo is not assigned in Unity Inspector");
        System.Diagnostics.Debug.Assert(cupCatcher != null, "CupCatcher is not assigned in Unity Inspector");

        controller = gameObject.transform.Find("CupCatcher").gameObject.GetComponent<WaiterCatcherController>();
    }

    void Update(){
        if (table == null && globalPositionInfo.GlobalPositionSet){
            table = globalPositionInfo.experimentTable;
            if (table != null){
                // set key points in robot movement
                if (globalPositionInfo.sceneName == "Sitting"){
                    middlePoint = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 1.7f * globalPositionInfo.userRight;
                    user1Peripheral = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 1.2f * globalPositionInfo.userRight - 1.0f * globalPositionInfo.userForward;
                    user2Peripheral = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 1.2f * globalPositionInfo.userRight + 1.2f * globalPositionInfo.userForward;
                    user1Near = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 0.8f * globalPositionInfo.userRight - 0.7f * globalPositionInfo.userForward;
                    user2Near = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 0.7f * globalPositionInfo.userRight + 0.7f * globalPositionInfo.userForward;
                    user1Collision = globalPositionInfo.userPosition + 1f * globalPositionInfo.userRight;
                }
                else if (globalPositionInfo.sceneName == "Standing"){
                    middlePoint = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 1.7f * globalPositionInfo.userRight;
                    user1Peripheral = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 1.0f * globalPositionInfo.userRight - 0.8f * globalPositionInfo.userForward;
                    user2Peripheral = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 1.0f * globalPositionInfo.userRight + 0.8f * globalPositionInfo.userForward;
                    user1Near = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 0.65f * globalPositionInfo.userRight - 0.6f * globalPositionInfo.userForward;
                    user2Near = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 0.65f * globalPositionInfo.userRight + 0.6f * globalPositionInfo.userForward;
                    user1Collision = globalPositionInfo.userPosition + 1f * globalPositionInfo.userRight;
                }
            }
        }
    }

    public IEnumerator SetCurrentDrink_Coroutine(GameObject drink){
        if (currentDrink != null) {
            currentDrink.transform.SetParent(null, worldPositionStays: true);
        }
        // if currentDrink's name == Coffee_wrong, disable it
        if (currentDrink != null && currentDrink.name == "Coffee_wrong"){
            currentDrink.SetActive(false);
        }
        currentDrink = drink;
        if (currentDrink != null){
            currentDrink.transform.position = cupCatcher.transform.Find("Catcher1").Find("CupCatcher").Find("FrontEndpoint").position - 0.04f * cupCatcher.transform.parent.right + cupCatcher.transform.parent.forward * 0.003f - new Vector3(0f, 0.02f, 0f);
            currentDrink.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            currentDrink.transform.SetParent(cupCatcher.transform.Find("Catcher1").Find("CupCatcher").Find("FrontEndpoint"), worldPositionStays: true);
        }
        yield return null;
    }

    public void SetCurrentDrink(GameObject drink){
        // FollowPlate script can be deprecated;
        if (currentDrink != null) {
            currentDrink.transform.SetParent(null, worldPositionStays: true);
        }
        // if currentDrink's name == Coffee_wrong, disable it
        if (currentDrink != null && currentDrink.name == "Coffee_wrong"){
            currentDrink.SetActive(false);
        }
        currentDrink = drink;
        if (currentDrink != null){
            currentDrink.transform.position = cupCatcher.transform.Find("Catcher1").Find("CupCatcher").Find("FrontEndpoint").position - 0.04f * cupCatcher.transform.parent.right + cupCatcher.transform.parent.forward * 0.003f - new Vector3(0f, 0.02f, 0f);
            currentDrink.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            currentDrink.transform.SetParent(cupCatcher.transform.Find("Catcher1").Find("CupCatcher").Find("FrontEndpoint"), worldPositionStays: true);
        }
    }

    private IEnumerator CurrentDrinkDetach(){
        if (currentDrink != null){
            currentDrink.transform.SetParent(null, worldPositionStays: true);
            currentDrink = null;
        }
        yield return null;
    }

    private IEnumerator CurrentDrinkAttach(){
        if (currentDrink != null){
            currentDrink.transform.SetParent(cupCatcher.transform.Find("Catcher1").Find("CupCatcher").Find("FrontEndpoint"), worldPositionStays: true);
        }
        yield return null;
    }

    public void SendOutDrink(bool dangerous=false){
        gameObject.transform.Find("Body").Find("screen").GetComponent<RobotScreenNotification>().SetScreenImage("CatAwake");
        float sendOutDrinkDistance = 0.5f;
        if (globalPositionInfo.sceneName == "Sitting")
            sendOutDrinkDistance = 0.5f;
        else if (globalPositionInfo.sceneName == "Standing")
            sendOutDrinkDistance = 0.57f;
        else 
            System.Diagnostics.Debug.Assert(false, "Invalid scene name.");
        if (!dangerous){
            StartCoroutine(
                controller.WaitForCoroutinesToEnd(new List<IEnumerator>(){
                    controller.LiftCatcher(0.1f),
                    controller.ForwardCatcher(sendOutDrinkDistance),
                    controller.LowerCatcher(0.1f),
                    CurrentDrinkDetach(),
                    controller.OpenCatcher(),
                    controller.LiftCatcher(0.1f),
                    controller.CloseCatcher(),
                    controller.BackwardCatcher(sendOutDrinkDistance),
                    controller.LowerCatcher(0.1f)
                })
            );
        }
        else{
            StartCoroutine(
                controller.WaitForCoroutinesToEnd(new List<IEnumerator>(){
                    controller.LiftCatcher(0.03f),
                    controller.ForwardCatcher(sendOutDrinkDistance),
                    currentDrink.GetComponent<DrinkAction>().Dangerous_Coroutine(),
                    controller.LowerCatcher(0.03f),
                    CurrentDrinkDetach(),
                    controller.OpenCatcher(),
                    controller.LiftCatcher(0.03f),
                    controller.CloseCatcher(),
                    controller.BackwardCatcher(sendOutDrinkDistance),
                    controller.LowerCatcher(0.03f)
                })
            );
        }
        gameObject.transform.Find("Body").Find("screen").GetComponent<RobotScreenNotification>().DrinkReady();
    }

    public void CollectDrink(){
        StartCoroutine(
            controller.WaitForCoroutinesToEnd(new List<IEnumerator>(){
                controller.LiftCatcher(0.03f),
                controller.OpenCatcher(),
                controller.ForwardCatcher(0.5f),
                controller.CloseCatcher(),
                SetCurrentDrink_Coroutine(GameObject.Find("Coffee_user2")),
                controller.BackwardCatcher(0.5f),
                controller.LowerCatcher(0.03f)
            })
        );
    }

    public void MoveToTableHalf_Fixed(){
        Vector3 targetPosition = (gameObject.transform.position + table.transform.position) / 2f;
        targetPosition = new Vector3(targetPosition.x, globalPositionInfo.floorHeight, targetPosition.z);
        StartCoroutine(gameObject.GetComponent<ExecuteMovement>().MoveAlongPath(
            new List<Vector3>{
                targetPosition
            }, 
            moveSpeed, rotateSpeed
        ));
    }

    // public void MoveToTableHalf(){
    //     Vector3 targetPosition = (gameObject.transform.position + table.transform.position) / 2f;
    //     targetPosition = new Vector3(targetPosition.x, globalPositionInfo.floorHeight, targetPosition.z);
    //     StartCoroutine(gameObject.GetComponent<ExecuteMovement>().PlanAndMoveTo(targetPosition, moveSpeed, rotateSpeed));
    // }

    public void MoveToTableUser1_Fixed(){
        StartCoroutine(gameObject.GetComponent<ExecuteMovement>().MoveAlongPath(
            new List<Vector3>{
                middlePoint,
                user1Peripheral,
                user1Near
            }, 
            moveSpeed, rotateSpeed
        ));
    }

    // public void MoveToTableUser1(bool rigid=false)
    // {
    //     Vector3 tablePosition2d = new Vector3(table.transform.position.x, 0, table.transform.position.z);
    //     ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
    //     if (!rigid){
    //         Vector3 targetPosition = table.transform.position + 0.8f * globalPositionInfo.userRight - 0.7f * globalPositionInfo.userForward;
    //         executor.PlanAndMoveTo(targetPosition, moveSpeed, rotateSpeed, true, tablePosition2d - 0.1f * globalPositionInfo.userForward);
    //     }
    //     else{
    //         Vector3 targetPosition = table.transform.position + 1.2f * globalPositionInfo.userRight - 1.0f * globalPositionInfo.userForward;
    //         StartCoroutine(
    //             controller.WaitForCoroutinesToEnd(
    //                 new List<IEnumerator>(){
    //                     executor.PlanAndMoveTo(targetPosition, moveSpeed, rotateSpeed, true, tablePosition2d),
    //                     executor.MoveAlongPath(new List<Vector3>{tablePosition2d + 0.8f * globalPositionInfo.userRight - 0.8f * globalPositionInfo.userForward}, moveSpeed, rotateSpeed, true, tablePosition2d) // - 0.1f * globalPositionInfo.userForward)
    //                 }
    //             )
    //         );
    //     }
    // }

    public void MoveToTableUser1Collision_Fixed()
    {
        StartCoroutine(gameObject.GetComponent<ExecuteMovement>().MoveAlongPath(
            new List<Vector3>{
                user1Collision
            }, 
            moveSpeed*2, rotateSpeed*2,
            true, globalPositionInfo.userPosition
        ));
    }

    // public void MoveToTableUser1Collision()
    // {
    //     Vector3 targetPosition = globalPositionInfo.userPosition + 1f * globalPositionInfo.userRight;
    //     ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
    //     StartCoroutine(executor.PlanAndMoveTo(targetPosition, moveSpeed*2, rotateSpeed, true, globalPositionInfo.userPosition));
    // }

    public void MoveToTableUser1FromCollision_Fixed(){
        StartCoroutine(gameObject.GetComponent<ExecuteMovement>().MoveAlongPath(
            new List<Vector3>{
                user1Peripheral,
                user1Near
            }, 
            moveSpeed, rotateSpeed
        ));
    }

    public void MoveToTableUser1Dangerous_Fixed()
    {
        StartCoroutine(gameObject.GetComponent<ExecuteMovement>().MoveAlongPath(
            new List<Vector3>{
                user1Peripheral,
                user1Near
            }, 
            moveSpeed, rotateSpeed
        ));
        currentDrink.GetComponent<DrinkAction>().Dangerous();
    }

    // public void MoveToTableUser1Dangerous(bool rigid=false)
    // {
    //     Vector3 tablePosition2d = new Vector3(table.transform.position.x, 0, table.transform.position.z);
    //     ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
    //     if (!rigid){
    //         Vector3 targetPosition = table.transform.position + 0.8f * globalPositionInfo.userRight - 0.7f * globalPositionInfo.userForward;
    //         executor.PlanAndMoveTo(targetPosition, moveSpeed, rotateSpeed, true, tablePosition2d - 0.1f * globalPositionInfo.userForward);
    //     }
    //     else{
    //         Vector3 targetPosition = table.transform.position + 1.2f * globalPositionInfo.userRight - 1.0f * globalPositionInfo.userForward;
    //         StartCoroutine(
    //             controller.WaitForCoroutinesToEnd(
    //                 new List<IEnumerator>(){
    //                     executor.PlanAndMoveTo(targetPosition, moveSpeed, rotateSpeed, true, tablePosition2d), // - 0.1f * globalPositionInfo.userForward),
    //                     executor.MoveAlongPath(new List<Vector3>{tablePosition2d + 0.8f * globalPositionInfo.userRight - 0.8f * globalPositionInfo.userForward}, moveSpeed, rotateSpeed, true, tablePosition2d) // - 0.1f * globalPositionInfo.userForward)
    //                 }
    //             )
    //         );
    //     }
    //     currentDrink.GetComponent<DrinkAction>().Dangerous();
    // }

    public void MoveToTableUser2_Fixed(){
        StartCoroutine(gameObject.GetComponent<ExecuteMovement>().MoveAlongPath(
            new List<Vector3>{
                user1Peripheral,
                middlePoint,
                user2Peripheral,
                user2Near
            }, 
            moveSpeed, rotateSpeed
        ));
    }

    // public void MoveToTableUser2(bool rigid=false)
    // {
        
    //     Vector3 tablePosition2d = new Vector3(table.transform.position.x, 0, table.transform.position.z);
        
    //     ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
    //     if (!rigid) {
    //         Vector3 targetPosition = table.transform.position + 0.9f * globalPositionInfo.userRight + 0.9f * globalPositionInfo.userForward;
    //         executor.PlanAndMoveTo(targetPosition, moveSpeed, rotateSpeed, true, tablePosition2d + 0.1f * globalPositionInfo.userForward);
    //     }
    //     else{
    //         Vector3 targetPosition = table.transform.position + 1.2f * globalPositionInfo.userRight + 1.2f * globalPositionInfo.userForward;
    //         StartCoroutine(
    //             controller.WaitForCoroutinesToEnd(
    //                 new List<IEnumerator>(){
    //                     // start position is user1
    //                     executor.MoveAlongPath(new List<Vector3>{tablePosition2d + 1.2f * globalPositionInfo.userRight - 0.8f * globalPositionInfo.userForward}, moveSpeed, rotateSpeed),
    //                     executor.PlanAndMoveTo(targetPosition, moveSpeed, rotateSpeed, true, tablePosition2d),
    //                     executor.MoveAlongPath(new List<Vector3>{tablePosition2d + 0.75f * globalPositionInfo.userRight + 0.75f * globalPositionInfo.userForward}, moveSpeed, rotateSpeed, true, tablePosition2d)
    //                 }
    //             )
    //         );
    //     }
    // }

    public void GoAway(){
        StartCoroutine(gameObject.GetComponent<ExecuteMovement>().MoveAlongPath(
            new List<Vector3>{
                new Vector3(globalPositionInfo.robotInitialPosition.x, 0, globalPositionInfo.robotInitialPosition.z)
            }, 
            moveSpeed, rotateSpeed
        ));
    }

    public void WanderAround(){
        ExecuteMovement executor = gameObject.GetComponent<ExecuteMovement>();
        StartCoroutine(
            controller.WaitForCoroutinesToEnd(
                new List<IEnumerator>(){
                    executor.MoveAlongPath(new List<Vector3>{gameObject.transform.position - gameObject.transform.forward}, moveSpeed, rotateSpeed),
                    executor.MoveAlongPath(
                        new List<Vector3>{
                            globalPositionInfo.userPosition + globalPositionInfo.userRight * 2.0f + globalPositionInfo.userForward * 1.9f,
                            globalPositionInfo.userPosition + globalPositionInfo.userRight * 2.0f + globalPositionInfo.userForward * 0.5f
                        }, 
                        moveSpeed, rotateSpeed, false, new Vector3(0,0,0), loop: true
                    )
                }
            )
        );
    }

    public void StopWandering(){
        gameObject.GetComponent<ExecuteMovement>().loopInterrupted = true;
    }
}
