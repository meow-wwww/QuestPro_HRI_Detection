using System.Collections;
using System.Collections.Generic;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;

public class SpotROSTartgetPublisher : MonoBehaviour
{
    const string k_RobotName = "spot1";

    [SerializeField]
    float m_LinearSpeed = 10f;

    [SerializeField]
    float m_AngularSpeed = 1.5f;

    [SerializeField]
    string m_TwistPublisherName = k_RobotName + "/cmd_vel";

    [SerializeField]
    string maxVelocityTopic = "/spot1/max_velocity";

    [SerializeField]
    string m_BodyPosePublisherName = k_RobotName + "/body_pose";

    [SerializeField]
    string m_GlobalPoseSubscriberName = k_RobotName + "/global_pose";

    // ROS Connector
    ROSConnection m_Ros;

    // initialize target object
    [SerializeField]
    GameObject m_Target;

    // spot object
    [SerializeField]
    GameObject m_Spot;

    readonly Quaternion m_OrientationOffset = Quaternion.Euler(0f, 0f, 0f);
    readonly Vector3 m_PoseOffset = new Vector3(-0.62f, 1f, 2.565f);

    // Start is called before the first frame update
    void Start()
    {
        m_Ros = ROSConnection.GetOrCreateInstance();
        // initialize publishers
        m_Ros.RegisterPublisher<TwistMsg>(m_TwistPublisherName, 1);
        m_Ros.RegisterPublisher<PoseMsg>(m_BodyPosePublisherName, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            InvokeRepeating("TransformController", 0.0f, 1.0f);
            Debug.Log("Publishing Pose");
        }
        m_Target.transform.position = m_Spot.transform.position;
        m_Target.transform.rotation = m_Spot.transform.rotation;
    }

    void TransformController()
    {
        // get target transform
        Vector3 targetLinearTwist = m_Spot.transform.position * m_LinearSpeed;
        Vector3 targetAngularTwist =
            new Vector3(0, m_Spot.transform.rotation.eulerAngles.y, 0) * m_AngularSpeed;
        Vector3 targetPosition = m_Target.transform.position - m_PoseOffset;
        Quaternion targetRotationQuaternion = m_Target.transform.rotation;

        // publish target transform
        // PublishTarget(targetLinearTwist, targetAngularTwist, targetPosition, targetRotationQuaternion);
    }

    void PublishTarget(
        Vector3 targetLinearTwist,
        Vector3 targetAngularTwist,
        Vector3 targetPosition,
        Quaternion targetRotationQuaternion
    )
    {
        TwistMsg Twist = new TwistMsg();
        Twist.linear = targetLinearTwist.To<FLU>();
        Twist.angular = targetAngularTwist.To<FLU>();

        PoseMsg BodyPose = new PoseMsg();
        BodyPose.position = targetPosition.To<FLU>();
        BodyPose.orientation = targetRotationQuaternion.To<FLU>();

        m_Ros.Publish(m_TwistPublisherName, Twist);
        m_Ros.Publish(m_BodyPosePublisherName, BodyPose);
    }
}
