using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WXYDebugAStar : MonoBehaviour
{
    public GameObject debugTarget;
    public GameObject robot;
    
    void OnEnable()
    {
        robot.GetComponent<AudioPlayer>().PlayAudio("Audio/doorbell");
        robot.GetComponent<ExecuteMovement>().PlanAndMoveTo(debugTarget.transform.position, 1f, 90.0f);
    }

}
