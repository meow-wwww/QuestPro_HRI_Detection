using System.Collections;
using System.Collections.Generic;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;

public class SpotROSBodyPoseController : MonoBehaviour
{
    private string m_BodyPosePublisherName = "spot1/body_pose";

    [SerializeField]
    Transform m_TargetPose=null;

    // ROS Connector
    ROSConnection m_Ros;
    
    // Start is called before the first frame update
    void Start()
    {
        // ROS Connector
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.RegisterPublisher<PoseMsg>(m_BodyPosePublisherName, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_TargetPose != null && m_TargetPose.hasChanged){
            PublishBodyPose(m_TargetPose.transform.localPosition, m_TargetPose.transform.localRotation);
        }
    }

    public void PublishBodyPose(Vector3 position, Quaternion orientation)
    {
        Quaternion rosOrientation = new Quaternion(-orientation.x, -orientation.y, -orientation.z, orientation.w);
        PoseMsg msg = new PoseMsg
        {
            position = position.To<FLU>(),
            orientation = rosOrientation.To<FLU>()
        };
        m_Ros.Publish(m_BodyPosePublisherName, msg);
    }
}
