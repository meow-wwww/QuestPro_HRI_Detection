using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;

public class TestScriptMoveit : MonoBehaviour
{
    public TrajectoryPlanner trajectoryPlanner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable(){
        trajectoryPlanner.PublishJoints();
        Debug.Log("PublishJoints() called");
    }
}
