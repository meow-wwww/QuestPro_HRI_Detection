using System.Collections;
using System.Collections.Generic;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;

public class SpotROSGlobalPoseController : MonoBehaviour
{
    private string m_GlobalPosePublisherName = "spot1/global_pose";

    [SerializeField]
    Transform m_TargetPose=null;

    [SerializeField]
    GameObject m_BodyTarget;

    [SerializeField]
    GameObject m_SpotBody;

    // ROS Connector
    ROSConnection m_Ros;

    // Start is called before the first frame update
    void Start()
    {
        // ROS Connector
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.RegisterPublisher<PoseMsg>(m_GlobalPosePublisherName, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_TargetPose != null && m_TargetPose.hasChanged){
            PublishGlobalPose(m_TargetPose.transform.localPosition, m_TargetPose.transform.localRotation);
            BodyTargetFollow();
        }
    }

    void BodyTargetFollow()
    {
        // let m_BodyTarget move with m_SpotBody
        m_BodyTarget.transform.position = m_SpotBody.transform.position;
        m_BodyTarget.transform.rotation = m_SpotBody.transform.rotation;
    }

    public void PublishGlobalPose(Vector3 position, Quaternion orientation)
    {
        // Quaternion rosOrientation = new Quaternion(-orientation.x, -orientation.y, -orientation.z, orientation.w);
        PoseMsg msg = new PoseMsg
        {
            position = position.To<FLU>(),
            orientation = orientation.To<FLU>()
        };
        m_Ros.Publish(m_GlobalPosePublisherName, msg);
    }
}
