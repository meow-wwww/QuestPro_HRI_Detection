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
    public float moveSpeed;
    public float rotateSpeed;

    [Header("Fixed Positions In Routes")]
    public ObjectPlacementInitialization globalPositionInfo; // assigned in Unity Inspector

    public Vector3 middlePoint; // 1.7 right
    public Vector3 user1Peripheral;
    public Vector3 user2Peripheral;
    public Vector3 user1Near;
    public Vector3 user2Near;
    public Vector3 user1Collision;

    WaiterCatcherController controller;
    InstructionManager instructionManager;

    // Start is called before the first frame update
    void Start()
    {
        System.Diagnostics.Debug.Assert(globalPositionInfo != null, "GlobalPositionInfo is not assigned in Unity Inspector");
        System.Diagnostics.Debug.Assert(cupCatcher != null, "CupCatcher is not assigned in Unity Inspector");

        controller = gameObject.transform.Find("CupCatcher").gameObject.GetComponent<WaiterCatcherController>();
        instructionManager = GameObject.Find("InstructionManager").GetComponent<InstructionManager>();
    }

    void Update(){
        if (table == null && globalPositionInfo.GlobalPositionSet){
            table = globalPositionInfo.experimentTable;
            if (table != null){
                // set key points in robot movement
                if (globalPositionInfo.sceneName == "Sitting"){
                    middlePoint = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 1.7f * globalPositionInfo.userRight;
                    user1Peripheral = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 1.2f * globalPositionInfo.userRight - 0.8f * globalPositionInfo.userForward;
                    user2Peripheral = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 1.2f * globalPositionInfo.userRight + 1.2f * globalPositionInfo.userForward;
                    user1Near = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 0.7f * globalPositionInfo.userRight - 0.7f * globalPositionInfo.userForward;
                    user2Near = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 0.6f * globalPositionInfo.userRight + 0.7f * globalPositionInfo.userForward;
                    user1Collision = globalPositionInfo.userPosition + 0.55f * globalPositionInfo.userRight + 0.1f * globalPositionInfo.userForward;
                }
                else if (globalPositionInfo.sceneName == "Standing"){
                    middlePoint = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 1.3f * globalPositionInfo.userRight;
                    user1Peripheral = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 1.5f * globalPositionInfo.userRight - 0.2f * globalPositionInfo.userForward;
                    user2Peripheral = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 1.1f * globalPositionInfo.userRight + 1.1f * globalPositionInfo.userForward;
                    user1Near = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 0.7f * globalPositionInfo.userRight - 0.4f * globalPositionInfo.userForward;
                    user2Near = new Vector3(table.transform.position.x, globalPositionInfo.floorHeight, table.transform.position.z) + 0f * globalPositionInfo.userRight + 1.1f * globalPositionInfo.userForward;
                    user1Collision = globalPositionInfo.userPosition + 0.85f * globalPositionInfo.userRight;
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
            currentDrink.transform.position = cupCatcher.transform.Find("Catcher1").Find("CupCatcher").Find("FrontEndpoint").position - 0.04f * cupCatcher.transform.parent.right + cupCatcher.transform.parent.forward * 0.003f - new Vector3(0f, 0.04f, 0f);
            // currentDrink.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            currentDrink.transform.SetParent(cupCatcher.transform.Find("Catcher1").Find("CupCatcher").Find("FrontEndpoint"), worldPositionStays: true);
        }
        yield return null;
    }

    public void SetCurrentDrink(GameObject drink){
        StartCoroutine(controller.WaitForCoroutinesToEnd(new List<IEnumerator>(){
            SetCurrentDrink_Coroutine(drink)
        }));
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
        float sendOutDrinkDistance = 0f;
        float cupToTableHeight = 0f;
        float additionalHeight = 0f;
        if (globalPositionInfo.sceneName == "Sitting"){
            sendOutDrinkDistance = 0.5f;
            cupToTableHeight = 0f;
            additionalHeight = 0.1f;
        }
        else if (globalPositionInfo.sceneName == "Standing"){
            sendOutDrinkDistance = 0.55f;
            cupToTableHeight = globalPositionInfo.tableHeight - gameObject.transform.Find("CupCatcher").Find("CupHeight").position.y;
            additionalHeight = 0.07f;
        }
        else{
            System.Diagnostics.Debug.Assert(false, "Invalid scene name.");
        }
        if (!dangerous){
            StartCoroutine(
                controller.WaitForCoroutinesToEnd(new List<IEnumerator>(){
                    gameObject.GetComponent<ExecuteMovement>().MoveAlongPath_Coroutine(
                        new List<Vector3>{},
                        moveSpeed, rotateSpeed,
                        finalRotate: true, 
                        finalFaceTowards: table.transform.position
                    ),
                    controller.LiftCatcher(cupToTableHeight + additionalHeight),
                    controller.ForwardCatcher(sendOutDrinkDistance),
                    controller.LowerCatcher(additionalHeight),
                    CurrentDrinkDetach(),
                    controller.OpenCatcher(),
                    controller.LiftCatcher(additionalHeight),
                    controller.CloseCatcher(),
                    controller.BackwardCatcher(sendOutDrinkDistance),
                    controller.LowerCatcher(cupToTableHeight + additionalHeight)
                })
            );
        }
        else if (dangerous){
            StartCoroutine(
                controller.WaitForCoroutinesToEnd(new List<IEnumerator>(){
                    gameObject.GetComponent<ExecuteMovement>().MoveAlongPath_Coroutine(
                        new List<Vector3>{},
                        moveSpeed, rotateSpeed,
                        finalRotate: true, 
                        finalFaceTowards: table.transform.position
                    ),
                    instructionManager.SetText_Coroutine("The drink's spilling! Correct the robot"),
                    controller.LiftCatcher(cupToTableHeight + additionalHeight),
                    controller.ForwardCatcher(sendOutDrinkDistance),
                    currentDrink.GetComponent<DrinkAction>().Dangerous_Coroutine(),
                    controller.LowerCatcher(additionalHeight),
                    CurrentDrinkDetach(),
                    controller.OpenCatcher(),
                    controller.LiftCatcher(additionalHeight),
                    controller.CloseCatcher(),
                    controller.BackwardCatcher(sendOutDrinkDistance),
                    controller.LowerCatcher(cupToTableHeight + additionalHeight)
                })
            );
        }
        gameObject.transform.Find("Body").Find("screen").GetComponent<RobotScreenNotification>().DrinkReady();
    }

    public void CollectDrink(){
        float cupToTableHeight = globalPositionInfo.tableHeight - gameObject.transform.Find("CupCatcher").Find("CupHeight").position.y;
        float additionalHeight = 0.03f;
        StartCoroutine(
            controller.WaitForCoroutinesToEnd(new List<IEnumerator>(){
                controller.LiftCatcher(cupToTableHeight + additionalHeight),
                controller.OpenCatcher(),
                controller.ForwardCatcher(0.5f),
                controller.CloseCatcher(),
                SetCurrentDrink_Coroutine(GameObject.Find("Coffee_user2")),
                controller.BackwardCatcher(0.5f),
                controller.LowerCatcher(cupToTableHeight + additionalHeight)
            })
        );
    }

    public void MoveToTableHalf_Fixed(){
        Vector3 targetPosition = (gameObject.transform.position + table.transform.position) / 2f - 0.5f * globalPositionInfo.userForward;
        targetPosition = new Vector3(targetPosition.x, globalPositionInfo.floorHeight, targetPosition.z);
        if (globalPositionInfo.sceneName == "Standing"){
            targetPosition += 0.5f * globalPositionInfo.userRight;
        }
        StartCoroutine(
            controller.WaitForCoroutinesToEnd(new List<IEnumerator>(){
                gameObject.GetComponent<ExecuteMovement>().MoveAlongPath_Coroutine(
                    new List<Vector3>{
                        targetPosition
                    }, 
                    moveSpeed, rotateSpeed
                ),
                instructionManager.SetText_Coroutine("Signal awareness")
            })
        );
    }

    public void MoveToTableUser1_Fixed(string instructionText){
        List<IEnumerator> coroutineList = new List<IEnumerator>(){
            gameObject.GetComponent<ExecuteMovement>().MoveAlongPath_Coroutine(
                new List<Vector3>{
                    // middlePoint,
                    user1Peripheral,
                    user1Near
                }, 
            moveSpeed, rotateSpeed
            )
        };
        if (instructionText != "") {
            coroutineList.Add(instructionManager.SetText_Coroutine(instructionText));
        }

        StartCoroutine(
            controller.WaitForCoroutinesToEnd(
                coroutineList
            )
        );
    }

    public void MoveToTableUser1Collision_Fixed()
    {
        List<Vector3> collisionPath;
        if (globalPositionInfo.sceneName == "Sitting"){
            collisionPath = new List<Vector3>{
                globalPositionInfo.robotInitialPosition + globalPositionInfo.userForward * 0.5f,
                user1Collision
            };
        }
        else if (globalPositionInfo.sceneName == "Standing"){
            collisionPath = new List<Vector3>{
                user1Collision + globalPositionInfo.userRight * 1.3f + globalPositionInfo.userForward * 1.5f,
                user1Collision
            };
        }
        else {
            System.Diagnostics.Debug.Assert(false, "Invalid scene name.");
            collisionPath = new List<Vector3>();
        }
        StartCoroutine(controller.WaitForCoroutinesToEnd(new List<IEnumerator>(){
            gameObject.GetComponent<ExecuteMovement>().CollisionMode(),
            gameObject.GetComponent<ExecuteMovement>().MoveAlongPath_Coroutine(
                collisionPath,
                moveSpeed*2, rotateSpeed*2,
                accelerate: true
            ),
            gameObject.GetComponent<ExecuteMovement>().NormalMode()
        }));
    }

    public void MoveToTableUser1FromCollision_Fixed(){
        if (globalPositionInfo.sceneName == "Sitting"){
            StartCoroutine(gameObject.GetComponent<ExecuteMovement>().MoveAlongPath_Coroutine(
                new List<Vector3>{
                    user1Peripheral,
                    user1Near
                }, 
                moveSpeed, rotateSpeed,
                true, table.transform.position
            ));
        }
        else{ // Standing
            StartCoroutine(gameObject.GetComponent<ExecuteMovement>().MoveAlongPath_Coroutine(
                new List<Vector3>{
                    user1Peripheral,
                    user1Near
                }, 
                moveSpeed, rotateSpeed,
                true, user1Near - globalPositionInfo.userRight
            ));
        }
    }

    public void MoveToTableUser1Dangerous_Fixed()
    {
        StartCoroutine(gameObject.GetComponent<ExecuteMovement>().MoveAlongPath_Coroutine(
            new List<Vector3>{
                user1Peripheral,
                user1Near
            }, 
            moveSpeed, rotateSpeed
        ));
        currentDrink.GetComponent<DrinkAction>().Dangerous();
    }

    public void MoveToTableUser2_Fixed(){
        StartCoroutine(gameObject.GetComponent<ExecuteMovement>().MoveAlongPath_Coroutine(
            new List<Vector3>{
                user1Peripheral,
                // middlePoint,
                user2Peripheral,
                user2Near
            }, 
            moveSpeed, rotateSpeed,
            true, table.transform.position
        ));
    }

    public void GoAway(){
        StartCoroutine(gameObject.GetComponent<ExecuteMovement>().MoveAlongPath_Coroutine(
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
                    executor.MoveAlongPath_Coroutine(
                        new List<Vector3>{
                            gameObject.transform.position - gameObject.transform.forward
                        }, 
                        moveSpeed, rotateSpeed
                    ),
                    executor.MoveAlongPath_Loop_Coroutine(
                        new List<Vector3>{
                            globalPositionInfo.userPosition + globalPositionInfo.userRight * 2.0f + globalPositionInfo.userForward * 0.7f,
                            globalPositionInfo.userPosition + globalPositionInfo.userRight * 2.0f + globalPositionInfo.userForward * 0.0f
                        }, 
                        moveSpeed, rotateSpeed //, false, new Vector3(0,0,0)
                    )
                }
            )
        );
    }

    public void StopWandering(){
        gameObject.GetComponent<ExecuteMovement>().loopInterrupted = true;
    }

    // public void PlayAudio(string audioClipName_Instruction){
    //     string audioClipName, instructionText;
    //     if (audioClipName_Instruction.Contains("#")){
    //         audioClipName = audioClipName_Instruction.Split('#')[0];
    //         instructionText = audioClipName_Instruction.Split('#')[1];
    //         // MainThreadDispatcher.Enqueue(() => {
    //         //     GameObject.Find("WaiterRobot").transform.Find("Body").Find("screen").GetComponent<RobotScreenNotification>().SendVoiceRequestWithInstruction(audioClipName, instructionText);
    //         //     if (audioClipName == "WhereShouldIPlace")
    //         //         GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>().SetDrinkPositionIndicator(true);
    //         //     Send("Received");
    //         // });
    //     }
    //     else{
    //         audioClipName = audioClipName_Instruction;
    //         // MainThreadDispatcher.Enqueue(() => {
    //         //     GameObject.Find("WaiterRobot").transform.Find("Body").Find("screen").GetComponent<RobotScreenNotification>().SendVoiceRequest(audioClipName_Instruction);
    //         //     if (audioClipName_Instruction == "WhereShouldIPlace")
    //         //         GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>().SetDrinkPositionIndicator(true);
    //         //     Send("Received");
    //         // });
    //     }
        
    // }
}
