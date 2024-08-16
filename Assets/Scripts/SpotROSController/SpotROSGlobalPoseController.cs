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
    static Transform m_CurrentGlobalPose;

    [Header("Position")]
    [SerializeField]
    private static float m_PositionX;

    [SerializeField]
    private static float m_PositionY;

    [SerializeField]
    private static float m_PositionZ;

    [Header("Rotation (in degrees)")]
    [SerializeField] private static float m_RotationRoll;
    [SerializeField] private static float m_RotationPitch;
    [SerializeField] private static float m_RotationYaw;

    [Header("Update Rate")]
    [SerializeField] private float m_UpdateRate = 20f; // Update 20 times per second by default

    // ROS Connector
    static ROSConnection m_Ros;

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentGlobalPose = transform.parent.Find("base_link");

        // ROS Connector
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.RegisterPublisher<PoseMsg>(m_GlobalPosePublisherName);
        m_Ros.RegisterPublisher<PoseMsg>(m_CurrentGlobalPosePublisherName);

        SetPosition(m_CurrentGlobalPose.position.x, m_CurrentGlobalPose.position.y, m_CurrentGlobalPose.position.z);
        SetRotation(m_CurrentGlobalPose.rotation.eulerAngles.x, m_CurrentGlobalPose.rotation.eulerAngles.y, m_CurrentGlobalPose.rotation.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        PublishCurrentGlobalPose();
        if (Input.GetKey(KeyCode.S))
        {
            CancelInvoke("UpdatePose");
            StopSpot();
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            InvokeRepeating("UpdatePose", 0f, 1f / m_UpdateRate);
        }
    }

    public static Transform GetGlobalPoseTransform()
    {
        return m_CurrentGlobalPose;
    }

    public static void StopSpot()
    {
        SetPosition(m_CurrentGlobalPose.position.x, m_CurrentGlobalPose.position.y, m_CurrentGlobalPose.position.z);
        SetRotation(m_CurrentGlobalPose.rotation.eulerAngles.x, m_CurrentGlobalPose.rotation.eulerAngles.y, m_CurrentGlobalPose.rotation.eulerAngles.z);
        UpdatePose();
        SpotROSTwistController.PublishTwistTarget(new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f));
        m_Ros.Publish(m_ResetPublisherName, new BoolMsg(true));
    }

    public static void UpdatePose()
    {
        Vector3 position = new Vector3(m_PositionX, m_PositionY, m_PositionZ);
        Vector3 rotation = new Vector3(m_RotationRoll, m_RotationPitch, m_RotationYaw);

        if (position != m_CurrentGlobalPose.position || rotation != m_CurrentGlobalPose.rotation.eulerAngles)
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
            position = m_CurrentGlobalPose.position.To<FLU>(),
            orientation = m_CurrentGlobalPose.rotation.To<FLU>()
        };
        m_Ros.Publish(m_CurrentGlobalPosePublisherName, msg);
    }

    // Public methods to set values from other scripts if needed
    public static void SetPosition(float x, float y, float z)
    {
        m_PositionX = x;
        m_PositionY = y;
        m_PositionZ = z;
    }

    public static void SetPosition(Vector3 position)
    {
        m_PositionX = position.x;
        m_PositionY = position.y;
        m_PositionZ = position.z;
    }

    public static void SetRotation(float roll, float pitch, float yaw)
    {
        m_RotationRoll = roll;
        m_RotationPitch = pitch;
        m_RotationYaw = yaw;
    }

    public static void SetRotation(Vector3 rotation)
    {
        m_RotationRoll = rotation.x;
        m_RotationPitch = rotation.y;
        m_RotationYaw = rotation.z;
    }
}
