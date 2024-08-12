using System.Collections;
using System.Collections.Generic;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;

public class SpotROSBodyPoseController : MonoBehaviour
{
    private static readonly string m_BodyPosePublisherName = "spot1/body_pose";
    private static readonly string m_ResetPublisherName = "spot1/reset";

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

    // ROS Connector
    static ROSConnection m_Ros;

    private Vector3 m_LastPosition;
    private Vector3 m_LastRotation;

    // Start is called before the first frame update
    void Start()
    {
        // ROS Connector
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.RegisterPublisher<BoolMsg>(m_ResetPublisherName);
        m_Ros.RegisterPublisher<PoseMsg>(m_BodyPosePublisherName);

        m_LastPosition = new Vector3(m_PositionX, m_PositionY, m_PositionZ);
        m_LastRotation = new Vector3(m_RotationRoll, m_RotationPitch, m_RotationYaw);
        m_Ros.Publish(m_ResetPublisherName, new BoolMsg(true));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.B))
        {
            UpdatePose();
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            SetPosition(0, 0, 0);
            SetRotation(0, 0, 0);

            m_Ros.Publish(m_ResetPublisherName, new BoolMsg(true));
            UpdatePose();
        }
    }

    void UpdatePose()
    {
        Vector3 position = new Vector3(m_PositionX, m_PositionY, m_PositionZ);
        Vector3 rotation = new Vector3(m_RotationRoll, m_RotationPitch, m_RotationYaw);

        if (position != m_LastPosition || rotation != m_LastRotation)
        {
            m_LastPosition = position;
            m_LastRotation = rotation;

            Quaternion orientation = Quaternion.Euler(rotation);
            PublishBodyPose(position, orientation);
        }
    }

    public static void PublishBodyPose(Vector3 position, Quaternion orientation)
    {
        Quaternion rosOrientation = new Quaternion(-orientation.x, -orientation.y, -orientation.z, orientation.w);
        PoseMsg msg = new PoseMsg
        {
            position = position.To<FLU>(),
            orientation = rosOrientation.To<FLU>()
        };
        m_Ros.Publish(m_BodyPosePublisherName, msg);
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
