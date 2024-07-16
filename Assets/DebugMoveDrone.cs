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
        FlyToTarget();
    }

    

    void FlyToTarget(){
        robot.GetComponent<ExecuteMovement>().FlyAlongPath(new List<Vector3>{target.transform.position}, 2f, 80f);
    }
}
