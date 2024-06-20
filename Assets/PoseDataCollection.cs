using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Movement.BodyTrackingForFitness;
// using Oculus.Movement.Utils;
// using Oculus.Interaction.Body.PoseDetection;


public class PoseDataCollection : MonoBehaviour
{
    public BodyPoseController bodyPoseController;
    public GameObject ball;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Current Pose: " + bodyPoseController.BonePoses[54].position);
        ball.transform.position = bodyPoseController.BonePoses[54].position;
    }
}
