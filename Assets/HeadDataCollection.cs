using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HeadDataCollection : MonoBehaviour
{
    public GameObject camera;

    string fileName = "";
    string filePath = "";

    // Start is called before the first frame update
    void Start()
    {
        fileName = "Head_" + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv";
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        string headNameCollection = "time,head_position_x,head_position_y,head_position_z,head_forward_x,head_forward_y,head_forward_z";
        File.WriteAllText(filePath, headNameCollection + "\n");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 headPosition = camera.transform.position;
        Vector3 headForward = camera.transform.forward;

        string headValueCollection = "";
        headValueCollection += headPosition.x + "," + headPosition.y + "," + headPosition.z + ",";
        headValueCollection += headForward.x + "," + headForward.y + "," + headForward.z;

        File.AppendAllText(filePath, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "," + headValueCollection + "\n");
    }
}
