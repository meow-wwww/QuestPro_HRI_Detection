using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EyeTracking : MonoBehaviour
{
    public GameObject cubeLeft, cubeRight;
    public GameObject eyeLeft, eyeRight;
    public GameObject sphere;

    // Vector3 forwardNormalizedLeft;

    string fileName = "";
    string filePath = "";

    // Start is called before the first frame update
    void Start()
    {
        fileName = "Gaze" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv";
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(filePath, "time,left_src_x,left_src_y,left_src_z,left_dir_x,left_dir_y,left_dir_z,right_src_x,right_src_y,right_src_z,right_dir_x,right_dir_y,right_dir_z,focus_x,focus_y,focus_z\n");
        // Debug.Log("write first line");
    }

    // Update is called once per frame
    // void Update()
    // {
    //     // update the sphere's position, s.t. it is eyeLeft's position + 0.5 unit * cube's forward vector
    //     if (cubeLeft != null && eyeLeft != null && sphereLeft != null){
    //         forwardNormalizedLeft = (cubeLeft.transform.forward).normalized; // cubeLeft.transform.forward + eyeLeft
    //         Vector3 sphereLeftPos = eyeLeft.transform.position + 1f * forwardNormalizedLeft;
    //         sphereLeft.transform.position = sphereLeftPos;
    //     }

    //     if (cubeRight!= null && eyeRight!= null && sphereRight!= null){
    //         forwardNormalizedRight = (cubeRight.transform.forward).normalized; //cubeRight.transform.forward + eyeRight
    //         Vector3 sphereRightPos = eyeRight.transform.position + 1f * forwardNormalizedRight;
    //         sphereRight.transform.position = sphereRightPos;
    //     }
    // }

    void FixedUpdate()
    {
        Vector3 focusPoint = FindIntersection(eyeLeft.transform.position, cubeLeft.transform.forward, eyeRight.transform.position, cubeRight.transform.forward);
        sphere.transform.position = focusPoint;
        string dataLine = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + eyeLeft.transform.position.x + "," + eyeLeft.transform.position.y + "," + eyeLeft.transform.position.z + "," + cubeLeft.transform.forward.x + "," + cubeLeft.transform.forward.y + "," + cubeLeft.transform.forward.z + "," + eyeRight.transform.position.x + "," + eyeRight.transform.position.y + "," + eyeRight.transform.position.z + "," + cubeRight.transform.forward.x + "," + cubeRight.transform.forward.y + "," + cubeRight.transform.forward.z + "," + focusPoint.x + "," + focusPoint.y + "," + focusPoint.z + "\n";
        File.AppendAllText(filePath, dataLine);
        // Debug.Log("write one new line");
    }


    Vector3 FindIntersection(Vector3 source1, Vector3 dir1, Vector3 source2, Vector3 dir2){
        dir1 = dir1.normalized;
        dir2 = dir2.normalized;

        Vector3 crossDir = Vector3.Cross(dir1, dir2);
        float crossDirLengthSquared = crossDir.sqrMagnitude;
        if (crossDirLengthSquared < 0.000001f){ // almost parallel
            float t = Vector3.Dot(source2 - source1, dir1) / Vector3.Dot(dir1, dir1);
            Vector3 closestPoint1 = source1 + t * dir1;
            Vector3 closestPoint2 = source2 + t * dir2;
            // if ((closestPoint1 - closestPoint2).magnitude < 0.000001f){
                return (closestPoint1 + closestPoint2) / 2f;
            // }
            // else{
            //     return new Vector3(1000f, 1000f, 1000f);
            // }
        }
        else {
            Vector3 r = source2 - source1;
            float t1 = (Vector3.Dot(Vector3.Cross(r, dir2), crossDir)) / crossDirLengthSquared;
            float t2 = (Vector3.Dot(Vector3.Cross(r, dir1), crossDir)) / crossDirLengthSquared;
            
            Vector3 intersectionPoint1 = source1 + t1 * dir1;
            Vector3 intersectionPoint2 = source2 + t2 * dir2;

            // if ((intersectionPoint1 - intersectionPoint2).magnitude < 0.000001f){
                return (intersectionPoint1 + intersectionPoint2) / 2f;
            // }
            // else{
            //     return new Vector3(1000f, 1000f, 1000f);
            // }
        }
    }
}
