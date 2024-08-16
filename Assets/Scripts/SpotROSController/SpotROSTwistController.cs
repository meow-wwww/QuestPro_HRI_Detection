using System;
using System.Collections;
using System.Collections.Generic;
using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;

public class SpotROSTwistController : MonoBehaviour
{
    const string k_RobotName = "spot1";
    private static readonly string maxVelocityPublisherName = k_RobotName + "/max_velocity";
    private static readonly string twistPublisherName = k_RobotName + "/cmd_vel";

    [SerializeField]
    float m_MaxLinearSpeedZ = 1.9f; // m/sec
    private float prevMaxLinearSpeedZ = 0f;

    [SerializeField]
    float m_MaxLinearSpeedX = 1.9f; // m/sec
    private float prevMaxLinearSpeedX = 0f;

    [SerializeField]
    float m_MaxAngularSpeed = 1.5f; // rad/sec
    private float prevMaxAngularSpeed = 0f;

    [SerializeField]
    float m_LinearSpeedForward = 0f; // m/sec

    [SerializeField]
    float m_LinearSpeedLeftward = 0f; // m/sec

    [SerializeField]
    float m_AngularSpeed = 0f; // rad/sec


    private static ROSConnection ros;

    // Start is called before the first frame update
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<TwistMsg>(maxVelocityPublisherName);
        ros.RegisterPublisher<TwistMsg>(twistPublisherName);
    }

    // Update is called once per frame
    void Update()
    {
        // publish once max twist is changed
        if (m_MaxLinearSpeedZ != prevMaxLinearSpeedZ || m_MaxLinearSpeedX != prevMaxLinearSpeedX || m_MaxAngularSpeed != prevMaxAngularSpeed)
        {
            SetMaxVelocities(m_MaxLinearSpeedZ, m_MaxLinearSpeedX, m_MaxAngularSpeed);
            prevMaxLinearSpeedZ = m_MaxLinearSpeedZ;
            prevMaxLinearSpeedX = m_MaxLinearSpeedX;
            prevMaxAngularSpeed = m_MaxAngularSpeed;
        }

        // Test publish twist
        if (Input.GetKeyDown(KeyCode.T))
        {
            PublishTwistTarget(new Vector3(m_LinearSpeedLeftward, 0f, m_LinearSpeedForward), new Vector3(0f, m_AngularSpeed, 0f));
        }
    }

    public static void SetMaxVelocities(float maxLinearZ, float maxLinearX, float maxAngular)
    {
        TwistMsg msg = new TwistMsg
        {
            linear = new Vector3Msg(Math.Abs(maxLinearZ), Math.Abs(maxLinearX), 0f),
            angular = new Vector3Msg(0f, 0f, Math.Abs(maxAngular))
        };
        ros.Publish(maxVelocityPublisherName, msg);
    }

    public static void PublishTwistTarget(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        TwistMsg twistMsg = new TwistMsg
        {
            linear = linearVelocity.To<FLU>(),
            angular = angularVelocity.To<FLU>()
        };

        ros.Publish(twistPublisherName, twistMsg);
    }
}
