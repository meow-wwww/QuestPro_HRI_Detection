using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using RosMessageTypes.Std;
using RosMessageTypes.Geometry;
using RosMessageTypes.BuiltinInterfaces;
using RosMessageTypes.PepperMoveitConfig;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;

public class PepperArmROSController : MonoBehaviour
{
    const int k_NumRobotJoints = 5;

    const float k_JointAssignmentWait = 0.1f;
    const float k_PoseAssignmentWait = 0.5f;

    public static readonly string[] LeftArmLinkNames =
        { "base_link/torso/LShoulder", "/LBicep", "/LElbow", "/LForeArm", "/l_wrist"};

    [SerializeField]
    public float fps = 20.0f;

    [SerializeField]
    string m_RosSubscriberName = "/pepper_moveit_config/left_arm/joints";
    public string RosSubscriberName { get => m_RosSubscriberName; set => m_RosSubscriberName = value; }

    [SerializeField]
    string m_RosPublisherName = "/pepper_moveit_config/left_arm/hand_pose";
    public string RosPublisherName { get => m_RosPublisherName; set => m_RosPublisherName = value; }

    [SerializeField]
    GameObject m_Pepper;
    public GameObject Pepper { get => m_Pepper; set => m_Pepper = value; }
    [SerializeField]
    GameObject m_LeftHandTargetPose;
    public GameObject LeftHandTargetPose { get => m_LeftHandTargetPose; set => m_LeftHandTargetPose = value; }

    private uint seq = 0;
    readonly Quaternion m_OrientationOffset = Quaternion.Euler(0, 0, 0);
    readonly Vector3 m_PoseOffset = Vector3.up * (-0.03f);

    // ROS Connector
    ROSConnection m_Ros = null;

    // Articulation Bodies
    ArticulationBody[] m_LeftArmArticulationBodies;

    Transform m_PepperLeftHandTransform;

    // Start is called before the first frame update
    void Start()
    {
        m_Ros = ROSConnection.GetOrCreateInstance();
        m_Ros.RegisterPublisher<PoseStampedMsg>(RosPublisherName);

        m_LeftArmArticulationBodies = new ArticulationBody[k_NumRobotJoints];

        var leftArmLink = string.Empty;
        for (int i = 0; i < k_NumRobotJoints; i++)
        {
            leftArmLink += LeftArmLinkNames[i];
            m_LeftArmArticulationBodies[i] = m_Pepper.transform.Find(leftArmLink).GetComponent<ArticulationBody>();
        }

        m_PepperLeftHandTransform = m_Pepper.transform.Find(leftArmLink);


        StartCoroutine(Initialization());
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.P))
        // {
        //     PublishPose();
        //     Debug.Log("Publishing Pose");
        // }
    }

    void PublishPose()
    {
        if (m_Ros && m_LeftHandTargetPose.transform.hasChanged)
        {
            PoseStampedMsg pose = new PoseStampedMsg();

            pose.header = new HeaderMsg(seq++, new TimeMsg(), "base");
            pose.pose.position = (m_LeftHandTargetPose.transform.localPosition - m_PoseOffset).To<FLU>();
            pose.pose.orientation = (m_LeftHandTargetPose.transform.localRotation * Quaternion.Inverse(m_OrientationOffset)).To<FLU>();

            m_Ros.Publish(RosPublisherName, pose);
            m_LeftHandTargetPose.transform.hasChanged = false;
        }
    }

    IEnumerator Initialization()
    {
        yield return InitJoints();
        m_Ros.Subscribe<PepperArmMoveitJointsMsg>(RosSubscriberName, TrajectoryResponse);
        InvokeRepeating("PublishPose", 0.0f, 1.0f / fps);
    }

    IEnumerator InitJoints()
    {
        var jointPositions = new double[] { 0, 0, 0, 0, 0 };
        var result = jointPositions.Select(r => (float)r /* * Mathf.Rad2Deg */).ToList();

        yield return JointPositionAssignment(result);

        // Align the end effector controller with the end effector
        var leftHandPosition = m_PepperLeftHandTransform.position;
        var leftHandRotation = m_PepperLeftHandTransform.rotation;
        m_LeftHandTargetPose.transform.position = leftHandPosition;
        m_LeftHandTargetPose.transform.rotation = leftHandRotation;
    }


    public IEnumerator JointPositionAssignment(List<float> jointPositions)
    {
        for (var joint = 0; joint < m_LeftArmArticulationBodies.Length; joint++)
        {
            var joint1XDrive = m_LeftArmArticulationBodies[joint].xDrive;
            joint1XDrive.target = jointPositions[joint];
            m_LeftArmArticulationBodies[joint].xDrive = joint1XDrive;
        }

        yield return new WaitForSeconds(k_JointAssignmentWait);
    }

    void TrajectoryResponse(PepperArmMoveitJointsMsg response)
    {
        if (response.joints != null)
        {
            if (response.joints != null && response.joints.Length == k_NumRobotJoints)
            {
                List<float> jointPositions = new List<float>(response.joints.Select(r => (float)r * Mathf.Rad2Deg));
                StartCoroutine(JointPositionAssignment(jointPositions));
            }
        }
    }

}
