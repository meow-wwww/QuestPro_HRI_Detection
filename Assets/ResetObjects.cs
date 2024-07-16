using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResetObjects : MonoBehaviour
{
    public List<GameObject> objectList;

    public List<TransformData> savedTransform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [System.Serializable]
    public class TransformData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 localScale;

        public TransformData(Vector3 pos, Quaternion rot)
        {
            position = pos;
            rotation = rot;
            // localScale = scale;
        }
    }

    public bool SaveContext(){
        try{
            savedTransform.Clear();
            for (int i = 0; i < objectList.Count; i++){
                TransformData data = new TransformData(objectList[i].transform.position, objectList[i].transform.rotation);
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
                objectList[i].transform.position = savedTransform[i].position;
                objectList[i].transform.rotation = savedTransform[i].rotation;
                // objectList[i].transform.localScale = savedTransform[i].localScale;
            }
            return true;
        }
        catch (Exception e){
            Debug.LogError("Error: " + e.Message);
            return false;
        }
    }
}
