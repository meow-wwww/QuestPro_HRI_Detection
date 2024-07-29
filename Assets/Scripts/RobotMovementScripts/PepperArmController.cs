using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PepperArmController : MonoBehaviour
{
    const int k_NumRobotJoints = 5;

    const float k_JointAssignmentWait = 0.1f;
    const float k_PoseAssignmentWait = 0.5f;

    static readonly List<float> initialJointValues = new List<float>(
        new float[] { 30f,6.3f, 36f, 55.1f, 104.5f }
    );

    // - name: fake_right_arm_controller
    //     joints:
    //     - RShoulderPitch
    //     - RShoulderRoll
    //     - RElbowYaw
    //     - RElbowRoll
    //     - RWristYaw
    // - name: fake_left_arm_controller
    //     joints:
    //     - LShoulderPitch
    //     - LShoulderRoll
    //     - LElbowYaw
    //     - LElbowRoll
    //     - LWristYaw
    public static readonly string[] LeftArmLinkNames =
    {
        "base_link/torso/LShoulder",
        "/LBicep",
        "/LElbow",
        "/LForeArm",
        "/l_wrist"
    };
    public static readonly string[] RightArmLinkNames =
    {
        "base_link/torso/RShoulder",
        "/RBicep",
        "/RElbow",
        "/RForeArm",
        "/r_wrist"
    };

    [SerializeField]
    GameObject m_Pepper;
    public GameObject Pepper
    {
        get => m_Pepper;
        set => m_Pepper = value;
    }

    // Serialize a DropdownField to select control group: right_arm, left_arm or both_arms
    [SerializeField]
    private string controlGroup = "both_arms";

    // SerializeField to assign values in range to the joints of the selected control group
    [Range(-119.5f, 119.5f)]
    [SerializeField]
    private float shoulderTarget = initialJointValues[0];

    [Range(0.5f, 89.5f)]
    [SerializeField]
    private float bicepTarget = initialJointValues[1];

    [Range(-119.5f, 119.5f)]
    [SerializeField]
    private float elbowTarget = initialJointValues[2];

    [Range(0.5f, 89.5f)]
    [SerializeField]
    private float foreArmTarget = initialJointValues[3];

    [Range(-104.5f, 104.5f)]
    [SerializeField]
    private float wristTarget = initialJointValues[4];

    List<float> jointTargetValues = new List<float>(
        initialJointValues
    );

    ArticulationBody[] m_LeftArmArticulationBodies;
    ArticulationBody[] m_RightArmArticulationBodies;

    // Start is called before the first frame update
    void Start()
    {
        m_LeftArmArticulationBodies = FindArticulationBodies(LeftArmLinkNames);
        m_RightArmArticulationBodies = FindArticulationBodies(RightArmLinkNames);
        ResetJoints("both_arms");
    }

    // Update is called once per frame
    void Update()
    {
        jointTargetValues = new List<float>(
            new float[] { shoulderTarget, bicepTarget, elbowTarget, foreArmTarget, wristTarget }
        );
        JointPositionAssignment(controlGroup, jointTargetValues);

        if (Input.GetKeyDown(KeyCode.B))
        {
            ResetJoints("both_arms");
            controlGroup = "both_arms";
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetJoints("right_arm");
            controlGroup = "right_arm";
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            ResetJoints("left_arm");
            controlGroup = "left_arm";
        }
    }

    ArticulationBody[] FindArticulationBodies(string[] linkNames)
    {
        ArticulationBody[] articulationBodies = new ArticulationBody[k_NumRobotJoints];

        var armLink = string.Empty;
        for (int i = 0; i < k_NumRobotJoints; i++)
        {
            armLink += linkNames[i];
            articulationBodies[i] = m_Pepper
                .transform.Find(armLink)
                .GetComponent<ArticulationBody>();
        }

        return articulationBodies;
    }

    void ResetJoints(string armName = "both_arms")
    {
        jointTargetValues = initialJointValues;
        controlGroup = armName;
        shoulderTarget = jointTargetValues[0];
        bicepTarget = jointTargetValues[1];
        elbowTarget = jointTargetValues[2];
        foreArmTarget = jointTargetValues[3];
        wristTarget = jointTargetValues[4];
        JointPositionAssignment(armName, jointTargetValues);
    }

    void LeftArmJointPositionAssignment(List<float> jointPositions)
    {
        List<float> leftPositions = new List<float>(jointPositions.Select(r => -r));
        leftPositions[0] = -leftPositions[0];
        leftPositions[1] = -leftPositions[1];

        for (var joint = 0; joint < m_LeftArmArticulationBodies.Length; joint++)
        {
            var joint1XDrive = m_LeftArmArticulationBodies[joint].xDrive;
            joint1XDrive.target = leftPositions[joint];
            m_LeftArmArticulationBodies[joint].xDrive = joint1XDrive;
        }
    }

    void RightArmJointPositionAssignment(List<float> jointPositions)
    {
        List<float> rightPositions = new List<float>(jointPositions);
        rightPositions[1] = -rightPositions[1];

        for (var joint = 0; joint < m_RightArmArticulationBodies.Length; joint++)
        {
            var joint1XDrive = m_RightArmArticulationBodies[joint].xDrive;
            joint1XDrive.target = rightPositions[joint];
            m_RightArmArticulationBodies[joint].xDrive = joint1XDrive;
        }
    }

    void JointPositionAssignment(string armName, List<float> jointPositions)
    {
        if (armName == "both_arms")
        {
            LeftArmJointPositionAssignment(jointPositions);
            RightArmJointPositionAssignment(jointPositions);
        }
        else if (armName == "left_arm")
        {
            LeftArmJointPositionAssignment(jointPositions);
        }
        else if (armName == "right_arm")
        {
            RightArmJointPositionAssignment(jointPositions);
        }
        else
        {
            Debug.Log("Invalid arm name: " + armName);
        }
    }
}
