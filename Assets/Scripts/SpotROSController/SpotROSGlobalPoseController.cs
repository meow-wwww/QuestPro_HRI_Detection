using System.Collections;
using System.Collections.Generic;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;

public class SpotROSGlobalPoseController : MonoBehaviour
{
    private static readonly string m_GlobalPosePublisherName = "spot1/global_pose";
    private static readonly string m_CurrentGlobalPosePublisherName = "spot1/current_global_pose";
    private static readonly string m_ResetPublisherName = "spot1/reset";

    [SerializeField]
    Transform m_CurrentGlobalPose;

    [Header("Position")]
    [SerializeField]
    private float m_PositionX;

    [SerializeField]
    private float m_PositionY;

    [SerializeField]
    private float m_PositionZ;

    [Header("Rotation (in degrees)")]
    [SerializeField] private float m_RotationRoll;
    [SerializeField] private float m_RotationPitch;
    [SerializeField] private float m_RotationYaw;

    [Header("Update Rate")]
    [SerializeField] private float m_UpdateRate = 20f; // Update 20 times per second by default

    // ROS Connector
    static ROSConnection m_Ros;

    private float m_TimeElapsed;

    // Start is called before the first frame update
    void Start()
    {
        // ROS Connector
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.RegisterPublisher<PoseMsg>(m_GlobalPosePublisherName);
        m_Ros.RegisterPublisher<PoseMsg>(m_CurrentGlobalPosePublisherName);

        SetPosition(m_CurrentGlobalPose.localPosition.x, m_CurrentGlobalPose.localPosition.y, m_CurrentGlobalPose.localPosition.z);
        SetRotation(m_CurrentGlobalPose.localRotation.eulerAngles.x, m_CurrentGlobalPose.localRotation.eulerAngles.y, m_CurrentGlobalPose.localRotation.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        PublishCurrentGlobalPose();
        if (Input.GetKey(KeyCode.S))
        {
            StopSpot();
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            InvokeRepeating("UpdatePose", 0f, 1f / m_UpdateRate);
        }
    }

    void StopSpot()
    {
        CancelInvoke("UpdatePose");
        SpotROSTwistController.PublishTwistTarget(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f));
        SetPosition(m_CurrentGlobalPose.localPosition.x, m_CurrentGlobalPose.localPosition.y, m_CurrentGlobalPose.localPosition.z);
        SetRotation(m_CurrentGlobalPose.localRotation.eulerAngles.x, m_CurrentGlobalPose.localRotation.eulerAngles.y, m_CurrentGlobalPose.localRotation.eulerAngles.z);
        UpdatePose();
        m_Ros.Publish(m_ResetPublisherName, new BoolMsg(true));
    }

    void UpdatePose()
    {
        Vector3 position = new Vector3(m_PositionX, m_PositionY, m_PositionZ);
        Vector3 rotation = new Vector3(m_RotationRoll, m_RotationPitch, m_RotationYaw);

        if (position != m_CurrentGlobalPose.localPosition || rotation != m_CurrentGlobalPose.localRotation.eulerAngles)
        {
            Quaternion orientation = Quaternion.Euler(rotation);
            PublishGlobalPose(position, orientation);
        }
    }

    public static void PublishGlobalPose(Vector3 position, Quaternion orientation)
    {
        Quaternion rosOrientation = new Quaternion(-orientation.x, -orientation.y, -orientation.z, orientation.w);
        PoseMsg msg = new PoseMsg
        {
            position = position.To<FLU>(),
            orientation = rosOrientation.To<FLU>()
        };
        m_Ros.Publish(m_GlobalPosePublisherName, msg);
    }

    void PublishCurrentGlobalPose()
    {
        PoseMsg msg = new PoseMsg
        {
            position = m_CurrentGlobalPose.localPosition.To<FLU>(),
            orientation = m_CurrentGlobalPose.localRotation.To<FLU>()
        };
        m_Ros.Publish(m_CurrentGlobalPosePublisherName, msg);
    }

    // Public methods to set values from other scripts if needed
    public void SetPosition(float x, float y, float z)
    {
        m_PositionX = x;
        m_PositionY = y;
        m_PositionZ = z;
    }

    public void SetRotation(float roll, float pitch, float yaw)
    {
        m_RotationRoll = roll;
        m_RotationPitch = pitch;
        m_RotationYaw = yaw;
    }
}
