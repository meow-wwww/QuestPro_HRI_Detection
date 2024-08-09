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
    private string maxVelocityPublisherName = k_RobotName + "/max_velocity";
    private string twistPublisherName = k_RobotName + "/cmd_vel";

    [SerializeField]
    float m_MaxLinearSpeedZ = 1.9f; // m/sec
    private float prevMaxLinearSpeedZ = 0f;

    [SerializeField]
    float m_MaxLinearSpeedX = 0.5f; // m/sec
    private float prevMaxLinearSpeedX = 0f;

    [SerializeField]
    float m_MaxAngularSpeed = 1.0f; // rad/sec
    private float prevMaxAngularSpeed = 0f;

    private ROSConnection ros;

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
        if (Input.GetKeyDown(KeyCode.P))
        {
            PublishTwistTarget(new Vector3(0.1f, 0f, 0.2f), new Vector3(0f, 0f, 0.1f));
        }
    }

    public void SetMaxVelocities(float maxLinearZ, float maxLinearX, float maxAngular)
    {
        TwistMsg msg = new TwistMsg
        {
            linear = new Vector3Msg(Math.Abs(maxLinearZ), Math.Abs(maxLinearX), 0f),
            angular = new Vector3Msg(0f, 0f, Math.Abs(maxAngular))
        };
        ros.Publish(maxVelocityPublisherName, msg);
    }

    public void PublishTwistTarget(Vector3 linearVelocity, Vector3 angularVelocity)
    {
        TwistMsg twistMsg = new TwistMsg
        {
            linear = linearVelocity.To<FLU>(),
            angular = angularVelocity.To<FLU>()
        };

        ros.Publish(twistPublisherName, twistMsg);
    }
}
