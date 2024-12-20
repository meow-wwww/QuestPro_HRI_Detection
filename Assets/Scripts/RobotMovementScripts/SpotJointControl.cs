using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Float = RosMessageTypes.Std.Float64Msg;

public class SpotJointSubscriber : MonoBehaviour
{
    // Each link name
    public static readonly string[] LinkNames =
    {   "base_link/front_rail/front_left_hip",
        "base_link/front_rail/front_left_hip/front_left_upper_leg",
        "base_link/front_rail/front_left_hip/front_left_upper_leg/front_left_lower_leg",
        "base_link/front_rail/front_right_hip",
        "base_link/front_rail/front_right_hip/front_right_upper_leg",
        "base_link/front_rail/front_right_hip/front_right_upper_leg/front_right_lower_leg",
        "base_link/rear_rail/rear_left_hip",
        "base_link/rear_rail/rear_left_hip/rear_left_upper_leg",
        "base_link/rear_rail/rear_left_hip/rear_left_upper_leg/rear_left_lower_leg",
        "base_link/rear_rail/rear_right_hip",
        "base_link/rear_rail/rear_right_hip/rear_right_upper_leg",
        "base_link/rear_rail/rear_right_hip/rear_right_upper_leg/rear_right_lower_leg"
    };

    //     # ------------------ Standard pose for starting step
    // sit_down = [[0.20, 1.0, -2.49],  # Front left leg
    //             [-0.20, 1.0, -2.49],  # Front right leg
    //             [0.20, 1.0, -2.49],  # Rear left leg
    //             [-0.20, 1.0, -2.49]]  # Rear right leg

    // stand_up = [[0.20, 0.7, -1.39],  # Front left leg
    //             [-0.20, 0.7, -1.39],  # Front right leg
    //             [0.20, 0.7, -1.39],  # Rear left leg
    //             [-0.20, 0.7, -1.39]]  # Rear right leg
    private static readonly double[] m_StandUpPose = { 0.20, 0.7, -1.39, -0.20, 0.7, -1.39, 0.20, 0.7, -1.39, -0.20, 0.7, -1.39 };
    private static readonly double[] m_SitDownPose = { 0.20, 1.0, -2.49, -0.20, 1.0, -2.49, 0.20, 1.0, -2.49, -0.20, 1.0, -2.49 };

    // Hardcoded variables
    const int k_NumRobotJoints = 12;

    [SerializeField]
    GameObject m_Spot;
    public GameObject Spot { get => m_Spot; set => m_Spot = value; }

    // Articulation Bodies
    ArticulationBody[] m_JointArticulationBodies;

    void Start()
    {
        // Create array for articulation bodies of each joint
        m_JointArticulationBodies = new ArticulationBody[k_NumRobotJoints];

        // Get the articulationbody for each joint
        var linkName = string.Empty;
        for (var i = 0; i < k_NumRobotJoints; i++)
        {
            m_JointArticulationBodies[i] = m_Spot.transform.Find(LinkNames[i]).GetComponent<ArticulationBody>();
        }
        // Set the initial pose of the spot
        UpdateJointAngles(m_StandUpPose);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.U))
        {
            UpdateJointAngles(m_StandUpPose);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            UpdateJointAngles(m_SitDownPose);
        }
    }

    // Update the joint angle by setting the
    // xDrive.Target of each Articulationbody
    public void UpdateJointAngle(double cmd, int joint)
    {
        var angle = (float)cmd * Mathf.Rad2Deg;
        var jointXDrive = m_JointArticulationBodies[joint].xDrive;
        jointXDrive.target = angle;
        m_JointArticulationBodies[joint].xDrive = jointXDrive;
    }

    public void UpdateJointAngles(double[] cmds)
    {
        for (var i = 0; i < k_NumRobotJoints; i++)
        {
            UpdateJointAngle(cmds[i], i);
        }
    }

}
