using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadDataCollection : MonoBehaviour
{
    public GameObject headIndicator;
    public Vector3 headIndicatorUpward;
    public GameObject camera;
    public Vector3 cameraForward;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        headIndicatorUpward = headIndicator.transform.up;
        cameraForward = camera.transform.forward;
        headIndicator.transform.up = cameraForward;
        headIndicator.transform.position = camera.transform.position + cameraForward * 1.0f;
    }
}
