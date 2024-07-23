using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResetObjects : MonoBehaviour
{
    public List<GameObject> objectList;

    public List<LogData> savedTransform;

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
            savedTransform.Clear();
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
                savedTransform.Add(data);
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
                if (savedTransform[i].onlySaveLocalScale){
                    objectList[i].transform.localScale = savedTransform[i].localScale;
                }
                else{
                    objectList[i].transform.position = savedTransform[i].position;
                    objectList[i].transform.rotation = savedTransform[i].rotation;
                    objectList[i].transform.SetParent(savedTransform[i].parent);
                    objectList[i].SetActive(savedTransform[i].active);
                }
            }
            return true;
        }
        catch (Exception e){
            Debug.LogError("Error: " + e.Message);
            return false;
        }
    }
}
