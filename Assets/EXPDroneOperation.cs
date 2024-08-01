using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXPDroneOperation : MonoBehaviour
{
    public GameObject table;
    GameObject tableTop;
    public float moveSpeed;
    public float rotateSpeed;
    public ObjectPlacementInitialization globalPositionInfo; // assigned in Unity Inspector

    [Header("Object references")]
    public GameObject currentDrink = null;
    GameObject plate; // assigned in Unity Inspector
    public GameObject cupCatcher; // assigned in Unity Inspector
    InstructionManager instructionManager;

    private IEnumerator WaitForCoroutinesToEnd(List<IEnumerator> coroutines)
    {
        foreach (IEnumerator coroutine in coroutines)
        {
            yield return StartCoroutine(coroutine);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        System.Diagnostics.Debug.Assert(cupCatcher != null, "CupCatcher is not assigned in Unity Inspector");
        instructionManager = GameObject.Find("InstructionManager").GetComponent<InstructionManager>();
    }

    void Update(){
        if (table == null && globalPositionInfo.GlobalPositionSet){
            table = globalPositionInfo.experimentTable; // this is the 'bottom' position of the table
            tableTop = table.transform.Find("TableTop").gameObject;
        }
    }

    // Drink and plate/cupCatcher related functions

    public void SetCurrentDrink(GameObject drink){
        // here the FollowPlate script is deprecated
        if (currentDrink != null) {
            currentDrink.transform.SetParent(null, worldPositionStays: true);
        }
        // if currentDrink's name == Coffee_wrong, disable it
        if (currentDrink != null && currentDrink.name == "Coffee_wrong"){
            currentDrink.SetActive(false);
        }
        currentDrink = drink;
        if (currentDrink != null){
            currentDrink.transform.position = cupCatcher.transform.position - new Vector3(0f, 0.19f, 0f);
            currentDrink.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
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
        targetPosition = new Vector3(targetPosition.x, gameObject.GetComponent<ExecuteMovement>().flightHeight + globalPositionInfo.floorHeight, targetPosition.z);
        StartCoroutine(WaitForCoroutinesToEnd(new List<IEnumerator>{
            // gameObject.GetComponent<ExecuteMovement>().FlyAlongPath(
            //     new List<Vector3>{targetPosition}, 
            //     moveSpeed, rotateSpeed
            // ),
            gameObject.GetComponent<ExecuteMovement>().FlyAlongPath_Coroutine(
                new List<Vector3>{targetPosition}, 
                moveSpeed, rotateSpeed,
                finalRotate: false,
                finalFaceTowards: default(Vector3),
                flyInStableHeight: false,
                stableHeight: 0f
            ),
            instructionManager.SetText_Coroutine("Signal awareness")
        }));
    }

    public void MoveToTableUser1(bool above=false, string instructionText="")
    {
        Vector3 targetPosition = tableTop.transform.position + 0.1f * globalPositionInfo.userRight - 0.2f * globalPositionInfo.userForward;
        if (above)
            targetPosition += new Vector3(0f,0.4f,0f);
        
        List<IEnumerator> coroutineList = new List<IEnumerator>{
            gameObject.GetComponent<ExecuteMovement>().FlyAlongPath_Coroutine(
                new List<Vector3>{targetPosition}, 
                moveSpeed, rotateSpeed, true, targetPosition - globalPositionInfo.userForward
            )
        };
        if (instructionText != "" && instructionText != null){
            coroutineList.Add(instructionManager.SetText_Coroutine(instructionText));
        }
        
        StartCoroutine(WaitForCoroutinesToEnd(
            coroutineList
        ));
    }

    public void MoveToTableUser1Collision()
    {
        Vector3 targetPosition = tableTop.transform.position + 0.8f * globalPositionInfo.userRight - 0.5f * globalPositionInfo.userForward;
        targetPosition.y = globalPositionInfo.floorHeight + 1.2f;
        gameObject.GetComponent<ExecuteMovement>().FlyAlongPath(
            new List<Vector3>{
                targetPosition + 1f * globalPositionInfo.userForward + 1.5f * globalPositionInfo.userRight,
                targetPosition
            }, 
            moveSpeed*2, rotateSpeed,
            // , true, new Vector3(globalPositionInfo.userPosition.x, gameObject.GetComponent<ExecuteMovement>().flightHeight, globalPositionInfo.userPosition.z)
            false, new Vector3(0,0,0),
            flyInStableHeight: true, stableHeight: globalPositionInfo.floorHeight + 1.2f
        );
    }

    public void MoveToTableUser1Dangerous(){
        Vector3 targetPosition = tableTop.transform.position + 0.25f * globalPositionInfo.userRight - 0.2f * globalPositionInfo.userForward;
        gameObject.GetComponent<ExecuteMovement>().FlyAlongPath(
            new List<Vector3>{
                targetPosition
            },
            moveSpeed, rotateSpeed, 
            true, targetPosition - globalPositionInfo.userForward
        );
        System.Diagnostics.Debug.Assert(currentDrink.name == "Coffee_user1", "Error: now the coffee is not Coffee_user1");
        currentDrink.GetComponent<DrinkAction>().Dangerous();
    }

    public void MoveUp(float height){
        Vector3 targetPosition = gameObject.transform.position + new Vector3(0, height, 0);
        gameObject.GetComponent<ExecuteMovement>().FlyAlongPath(
            new List<Vector3>{targetPosition}, 
            moveSpeed, rotateSpeed
        );
    }

    public void MoveToTableUser2()
    {
        Vector3 targetPosition = tableTop.transform.position + 0.1f * globalPositionInfo.userRight + 0.2f * globalPositionInfo.userForward;
        gameObject.GetComponent<ExecuteMovement>().FlyAlongPath(
            new List<Vector3>{targetPosition}, 
            moveSpeed, rotateSpeed, 
            true, targetPosition + globalPositionInfo.userForward
        );
    }

    public void GoAway(){
        gameObject.GetComponent<ExecuteMovement>().FlyAlongPath(
            new List<Vector3>{globalPositionInfo.robotInitialPosition}, 
            moveSpeed, rotateSpeed
        );
    }

    public void WanderAround(){
        List<Vector3> wanderPath = new List<Vector3>{
            globalPositionInfo.userPosition + globalPositionInfo.userRight * 1.5f + globalPositionInfo.userForward * 1.9f,
            globalPositionInfo.userPosition + globalPositionInfo.userRight * 1.5f + globalPositionInfo.userForward * 0.1f,
        };
        for (int i = 0; i < wanderPath.Count; i++)
            wanderPath[i] = new Vector3(wanderPath[i].x, gameObject.GetComponent<ExecuteMovement>().flightHeight, wanderPath[i].z);
        gameObject.GetComponent<ExecuteMovement>().FlyAlongPath(
            wanderPath, 
            moveSpeed, rotateSpeed, 
            false, new Vector3(0,0,0), 
            loop: true
        );
    }

    public void StopWandering(){
        gameObject.GetComponent<ExecuteMovement>().loopInterrupted = true;
    }
}
