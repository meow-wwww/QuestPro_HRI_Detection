using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResetObjects : MonoBehaviour
{
    public ObjectPlacementInitialization globalPositionInfo; // assigned in Unity Inspector

    public List<GameObject> objectList;

    public List<LogData> savedLogData;

    public GameObject currentDrinkSaved;

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

    public bool SaveContext(){
        try{
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

            // save currentDrink
            if (globalPositionInfo.robot.name == "WaiterRobot"){
                currentDrinkSaved = globalPositionInfo.robot.GetComponent<EXPWaiterOperation>().currentDrink;
            }
            else if (globalPositionInfo.robot.name == "DroneRobot"){
                currentDrinkSaved = globalPositionInfo.robot.GetComponent<EXPDroneOperation>().currentDrink;
            }
            else{
                Debug.LogError("Error: Invalid robot name.");
            }

            return true;
        }
        catch (Exception e){
            Debug.LogError("Error: " + e.Message);
            return false;
        }
    }

    public bool ResetContext(){
        try{
            for (int i = 0; i < objectList.Count; i++){
                if (savedLogData[i].onlySaveLocalScale){
                    objectList[i].transform.localScale = savedLogData[i].localScale;
                }
                else{
                    objectList[i].transform.position = savedLogData[i].position;
                    objectList[i].transform.rotation = savedLogData[i].rotation;
                    objectList[i].transform.SetParent(savedLogData[i].parent);
                    objectList[i].SetActive(savedLogData[i].active);
                }
            }

            // recover currentDrink
            if (globalPositionInfo.robot.name == "WaiterRobot"){
                globalPositionInfo.robot.GetComponent<EXPWaiterOperation>().currentDrink = currentDrinkSaved;
            }
            else if (globalPositionInfo.robot.name == "DroneRobot"){
                globalPositionInfo.robot.GetComponent<EXPDroneOperation>().currentDrink = currentDrinkSaved;
            }
            else{
                Debug.LogError("Error: Invalid robot name.");
            }

            return true;
        }
        catch (Exception e){
            Debug.LogError("Error: " + e.Message);
            return false;
        }
    }
}
