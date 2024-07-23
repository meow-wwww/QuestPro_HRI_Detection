using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PepperHeadController : MonoBehaviour
{
    // name: fake_head_controller
    // joints:
    //   - HeadYaw
    //   - HeadPitch
    ArticulationBody m_HeadYaw;
    ArticulationBody m_HeadPitch;


    [SerializeField]
    GameObject m_Pepper;
    public GameObject Pepper { get => m_Pepper; set => m_Pepper = value; }

    // xDrive sliders with xDrive limits
    // yaw: -119.5001, 119.5001
    // pitch: -40.49998, 36.49999
    [Range(-119.5001f, 119.5001f)]
    public float headYawDriveTarget = 0.0f;
    [Range(-40.49998f, 36.49999f)]
    public float headPitchDriveTarget = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        // link names to head yaw and pitch
        var neckLink = "base_link/torso/Neck";
        var headLink = neckLink + "/Head";

        // find head pitch and yaw articulation bodies
        m_HeadYaw = Pepper.transform.Find(neckLink).GetComponent<ArticulationBody>();
        m_HeadPitch = Pepper.transform.Find(headLink).GetComponent<ArticulationBody>();
    }

    // Update is called once per frame
    void Update()
    {
        // set xDrive if headYawDriveTarget or headPitchDriveTarget has changed
        if (m_HeadYaw.xDrive.target != headYawDriveTarget || m_HeadPitch.xDrive.target != headPitchDriveTarget)
        {
            setHeadTarget(headYawDriveTarget, headPitchDriveTarget);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            resetHeadTarget();
        }
    }

    // set xDrive for head yaw and pitch
    void setHeadTarget(float yaw, float pitch)
    {
        // set xDrive for head yaw
        ArticulationDrive headYawDrive = m_HeadYaw.xDrive;
        headYawDrive.target = yaw;
        m_HeadYaw.xDrive = headYawDrive;

        // set xDrive for head pitch
        ArticulationDrive headPitchDrive = m_HeadPitch.xDrive;
        headPitchDrive.target = pitch;
        m_HeadPitch.xDrive = headPitchDrive;
    }

    void resetHeadTarget()
    {
        setHeadTarget(0.0f, 0.0f);
    }
}
