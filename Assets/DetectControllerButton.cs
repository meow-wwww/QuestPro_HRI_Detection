using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectControllerButton : MonoBehaviour
{
    bool stopRecord = true;
    public CameraRecord cameraRecordScript;
    public CameraRecordPose cameraRecordPoseScript;

    void Start()
    {
        Invoke("SetFalse", 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopRecord){
            if (!OVRManager.hasInputFocus){
                Debug.Log("!!!!!!Lost input focus");
                stopRecord = true;
                if (cameraRecordScript!= null){
                    cameraRecordScript.StopRecordingInterface();
                }
                if (cameraRecordPoseScript!= null){
                    cameraRecordPoseScript.StopRecordingInterface();
                }
            }
        }


        // Debug.Log("input focus:" + OVRManager.hasInputFocus);
        // Debug.Log("Controller:" + controller.ControllerInput);
    //    if (OVRInput.Get(OVRInput.Button.Two)){
    //         Debug.Log("B button is pressed");
    //         // B = true;
    //         // cameraRecordScript.StopRecordingInterface();
    //     }
    //     else if (OVRInput.Get(OVRInput.Button.One)){
    //         Debug.Log("A button is pressed");
    //     }
    //     else if (OVRInput.Get(OVRInput.Button.Three)){
    //         Debug.Log("X button is pressed");
    //     }
    //     else if (OVRInput.Get(OVRInput.Button.Four)){
    //         Debug.Log("Y button is pressed");
    //     }
    }

    void SetFalse(){
        stopRecord = false;
    }
}
