using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceDataCollection : MonoBehaviour
{
    public GameObject faceManager;
    public OVRFaceExpressions ovrFaceExpressionsScript;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ovrFaceExpressionsScript.FaceTrackingEnabled){
            // Debug.Log("JawDrop weight:"+ovrFaceExpressionsScript[OVRFaceExpressions.FaceExpression.JawDrop]);
        }
        
    }
}
