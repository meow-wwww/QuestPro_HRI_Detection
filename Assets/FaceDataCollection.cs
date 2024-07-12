using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class FaceDataCollection : MonoBehaviour
{
    // public GameObject faceManager;
    public OVRFaceExpressions ovrFaceExpressionsScript;

    string fileName = "";
    string filePath = "";

    // Start is called before the first frame update
    void Start()
    {
        fileName = "FacialExpression_" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv";
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        string expressionNameCollection = "";
        foreach (OVRFaceExpressions.FaceExpression expressionName in Enum.GetValues(typeof(OVRFaceExpressions.FaceExpression))){
            if (expressionName.ToString() != "Invalid" && expressionName.ToString() != "Max")
                expressionNameCollection += expressionName.ToString() + ",";
        }
        // remove the ending ","
        expressionNameCollection = expressionNameCollection.TrimEnd(',');
        File.WriteAllText(filePath, "time," + expressionNameCollection + "\n");
    }

    // Update is called once per frame
    void Update()
    {
        if (ovrFaceExpressionsScript.FaceTrackingEnabled && ovrFaceExpressionsScript.ValidExpressions){
            string expressionValueCollection = "";
            foreach (OVRFaceExpressions.FaceExpression expressionName in Enum.GetValues(typeof(OVRFaceExpressions.FaceExpression))){
                if (expressionName.ToString() != "Invalid" && expressionName.ToString() != "Max")
                    expressionValueCollection += ovrFaceExpressionsScript[expressionName] + ",";
            }
            expressionValueCollection = expressionValueCollection.TrimEnd(',');
            File.AppendAllText(filePath, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + expressionValueCollection + "\n");
        }


        
       
        // Debug.Log("AllFace weight:" + ovrFaceExpressionsScript);
        // if (ovrFaceExpressionsScript.FaceTrackingEnabled){
        //     Debug.Log("JawDrop weight:"+ovrFaceExpressionsScript[OVRFaceExpressions.FaceExpression.JawDrop]);
        // }
        
    }
}
