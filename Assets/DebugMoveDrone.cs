using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugMoveDrone : MonoBehaviour
{

    public GameObject robot, target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable(){
        // FlyToTarget();
        robot.transform.Find("CupCatcher").gameObject.GetComponent<DroneCatcherController>().LowerCatcher(0.25f);
        robot.transform.Find("CupCatcher").gameObject.GetComponent<DroneCatcherController>().ForwardCatcher(0.1f);
    }

    // void OnDisable(){
    //     robot.transform.Find("CupCatcher").gameObject.GetComponent<DroneCatcherController>().LiftCatcher(0.25f);
    // }

    

    

    void FlyToTarget(){
        robot.GetComponent<ExecuteMovement>().FlyAlongPath(new List<Vector3>{target.transform.position}, 2f, 80f);
    }
}
