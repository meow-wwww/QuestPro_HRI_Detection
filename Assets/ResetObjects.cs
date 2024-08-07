using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResetObjects : MonoBehaviour
{
    public ObjectPlacementInitialization globalPositionInfo; // assigned in Unity Inspector

    public List<GameObject> objectList;

    public List<LogData> savedLogData; // corresponding to the 'context' for one trial
    public Dictionary<int, List<LogData>> globalSavedLogData = new Dictionary<int, List<LogData>>(); // corresponding to the 'context' for all trials

    public GameObject currentDrinkSaved;
    public Dictionary<int, GameObject> globalCurrentDrinkSaved = new Dictionary<int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Serializable]
    public class LogData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Transform parent;
        public bool active;
        public bool onlySaveLocalScale;
        public Vector3 localScale;

        public LogData(Vector3 pos, Quaternion rot, Transform p, bool a, bool saveLocalScale=false, Vector3 locScale=default(Vector3))
        {
            position = pos;
            rotation = rot;
            parent = p;
            active = a;
            if (saveLocalScale){
                onlySaveLocalScale = true;
                localScale = locScale;
            }
            else{
                onlySaveLocalScale = false;
                localScale = Vector3.zero;
            }
        }
    }

    public int SaveContext(int trial_id){
        try{
            // if trial_id already exists, return 1
            if (globalSavedLogData.ContainsKey(trial_id)){
                return 1;
            }

            savedLogData.Clear();
            for (int i = 0; i < objectList.Count; i++){
                LogData data = null;
                if (objectList[i].name == "coffee") {
                    data = new LogData(
                        objectList[i].transform.position, 
                        objectList[i].transform.rotation,
                        objectList[i].transform.parent,
                        objectList[i].activeSelf,
                        saveLocalScale: true,
                        locScale: objectList[i].transform.localScale
                    );
                }
                else{
                    data = new LogData(
                        objectList[i].transform.position, 
                        objectList[i].transform.rotation,
                        objectList[i].transform.parent,
                        objectList[i].activeSelf,
                        saveLocalScale: false
                    );
                }
                savedLogData.Add(data);
            }
            globalSavedLogData[trial_id] = new List<LogData>(savedLogData);

            // save currentDrink
            if (globalPositionInfo.robot.name == "WaiterRobot"){
                currentDrinkSaved = globalPositionInfo.robot.GetComponent<EXPWaiterOperation>().currentDrink;
            }
            else if (globalPositionInfo.robot.name == "DroneRobot"){
                currentDrinkSaved = globalPositionInfo.robot.GetComponent<EXPDroneOperation>().currentDrink;
            }
            else if (globalPositionInfo.robot.name == "HumanoidRobot"){
                currentDrinkSaved = globalPositionInfo.robot.GetComponent<EXPHumanoidOperation>().currentDrink;
            }
            else{
                Debug.LogError("Error: Invalid robot name.");
            }
            globalCurrentDrinkSaved[trial_id] = currentDrinkSaved;

            // print the content of globalCurrentDrinkSaved
            foreach (KeyValuePair<int, GameObject> kvp in globalCurrentDrinkSaved){
                Debug.Log("Key = " + kvp.Key + ", Value = " + kvp.Value);
            }
            Debug.Log("=============================");

            return 0;
        }
        catch (Exception e){
            Debug.LogError("Error: " + e.Message);
            return -1;
        }
    }

    public int ResetContext(int trial_id){
        try{
            if (!globalSavedLogData.ContainsKey(trial_id)){
                return 1;
            }

            List<LogData> savedLogDataForThisStep = globalSavedLogData[trial_id];

            for (int i = 0; i < objectList.Count; i++){
                if (savedLogDataForThisStep[i].onlySaveLocalScale){
                    objectList[i].transform.localScale = savedLogDataForThisStep[i].localScale;
                }
                else{
                    objectList[i].transform.position = savedLogDataForThisStep[i].position;
                    objectList[i].transform.rotation = savedLogDataForThisStep[i].rotation;
                    objectList[i].transform.SetParent(savedLogDataForThisStep[i].parent);
                    objectList[i].SetActive(savedLogDataForThisStep[i].active);
                }
            }

            // recover currentDrink
            GameObject currentDrinkSavedForThisStep = globalCurrentDrinkSaved[trial_id];
            if (globalPositionInfo.robot.name == "WaiterRobot"){
                globalPositionInfo.robot.GetComponent<EXPWaiterOperation>().currentDrink = currentDrinkSavedForThisStep;
            }
            else if (globalPositionInfo.robot.name == "DroneRobot"){
                globalPositionInfo.robot.GetComponent<EXPDroneOperation>().currentDrink = currentDrinkSavedForThisStep;
            }
            else if (globalPositionInfo.robot.name == "HumanoidRobot"){
                globalPositionInfo.robot.GetComponent<EXPHumanoidOperation>().currentDrink = currentDrinkSavedForThisStep;
            }
            else{
                Debug.LogError("Error: Invalid robot name.");
            }

            return 0;
        }
        catch (Exception e){
            Debug.LogError("Error: " + e.Message);
            return -1;
        }
    }
}
