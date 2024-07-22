using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementInitialization : MonoBehaviour
{
    GameObject user1Seat, user2Seat, user3Seat;

    [Header("Scene Setting")]
    public string sceneName;

    [Header("Global Position Info")]
    public bool GlobalPositionSet = false;

    public Vector3 userForward;
    public Vector3 userRight;
    public Vector3 userPosition;
    public Vector3 performerPosition;
    public Vector3 robotInitialPosition;
    public float floorHeight = 0f;
    public float tableHeight;

    [Header("Object References")]
    public GameObject robot;
    public GameObject table;
    public GameObject bar;
    public GameObject instruction;

    public GameObject experimentTable;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = PlayerPrefs.GetString("mode");
        System.Diagnostics.Debug.Assert(sceneName != "", "Scene name is not assigned in Unity Inspector");
        System.Diagnostics.Debug.Assert(robot != null, "Robot is not assigned in Unity Inspector");
        System.Diagnostics.Debug.Assert(table != null, "Table is not assigned in Unity Inspector");
        System.Diagnostics.Debug.Assert(bar != null, "Bar is not assigned in Unity Inspector");
        System.Diagnostics.Debug.Assert(instruction != null, "Instruction is not assigned in Unity Inspector");
    }

    // Update is called once per frame
    void Update()
    {
        if (user1Seat == null || user2Seat == null || user3Seat == null){
            user1Seat = GameObject.Find("COUCH");
            user2Seat = GameObject.Find("BED");
            user3Seat = GameObject.Find("STORAGE");
            if (user1Seat != null && user2Seat != null && user3Seat != null){
                FindGlobalPositionInfo(); // only execute once
            }
        }
    }

    void FindGlobalPositionInfo(){
        floorHeight = GameObject.Find("FLOOR").transform.position.y;

        // set up the room's directions
        userForward = user2Seat.transform.position - user1Seat.transform.position;
        userForward = new Vector3(userForward.x, 0, userForward.z).normalized;
        userRight = new Vector3(userForward.z, 0, -userForward.x);

        // set virtual objects' position (eg. table, bar, ...)
        table.transform.position = new Vector3(GameObject.Find("TABLE").transform.position.x, floorHeight, GameObject.Find("TABLE").transform.position.z) + userRight * 0.1f;
        bar.transform.position = new Vector3(GameObject.Find("SCREEN").transform.position.x, floorHeight, GameObject.Find("SCREEN").transform.position.z);
        bar.transform.rotation = GameObject.Find("SCREEN").transform.rotation;

        

        // for standing scenario, the positions are different.
        if (sceneName == "Sitting"){
            experimentTable = table;
            userPosition = new Vector3(user1Seat.transform.position.x, floorHeight, user1Seat.transform.position.z);
            performerPosition = new Vector3(user2Seat.transform.position.x, floorHeight, user2Seat.transform.position.z);
        }
        else if (sceneName == "Standing"){
            experimentTable = bar;
            userPosition = new Vector3(bar.transform.position.x, floorHeight, bar.transform.position.z) - userForward * 0.9f;
            performerPosition = new Vector3(bar.transform.position.x, floorHeight, bar.transform.position.z) + userForward * 0.9f;
        }
        else {
            System.Diagnostics.Debug.Assert(false, "Invalid scene name.");
        }

        // set robots' initial positions
        // sitting: 5 right + 3 forward
        // standing: 2 right + 4 forward
        if (sceneName == "Sitting"){
            robot.transform.position = userPosition + userRight * 5f + userForward * 3f;
        }
        else if (sceneName == "Standing"){
            robot.transform.position = userPosition + userRight * 2f + userForward * 4f;
        }
        else{
            System.Diagnostics.Debug.Assert(false, "Invalid scene name.");
        }

        if (robot.name == "DroneRobot")
            robot.transform.position += new Vector3(0, 2f, 0);
        robotInitialPosition = robot.transform.position;

        if (sceneName == "Sitting"){
            tableHeight = table.transform.Find("TableTop").transform.position.y;
            instruction.transform.position = new Vector3(experimentTable.transform.position.x, tableHeight + 0.15f, experimentTable.transform.position.z);
        }
        else if (sceneName == "Standing"){
            tableHeight = GameObject.Find("SCREEN").transform.position.y;
            instruction.transform.position = new Vector3(experimentTable.transform.position.x, tableHeight + 0.5f, experimentTable.transform.position.z) + userRight * 0.25f;
        }
        else
            System.Diagnostics.Debug.Assert(false, "Invalid scene name.");

        
        instruction.transform.rotation = Quaternion.LookRotation(userForward, Vector3.up);

        GameObject.Find("Coffee_user1").transform.position = userPosition - userRight * 2000f + userForward * 0.2f;
        GameObject.Find("Coffee_user2").transform.position = userPosition - userRight * 2000f;
        GameObject.Find("Coffee_wrong").transform.position = userPosition - userRight * 2000f - userForward * 0.2f;

        GlobalPositionSet = true;
    }

    public void SetDrinkPositionIndicator(bool state){
        experimentTable.transform.Find("DrinkPlaceIndicator").gameObject.SetActive(state);
        if (sceneName == "Sitting"){
            experimentTable.transform.Find("DrinkPlaceIndicator").position = new Vector3(experimentTable.transform.position.x, tableHeight, experimentTable.transform.position.z) + 0.15f * userRight - 0.15f * userForward;
        }
    }
}
