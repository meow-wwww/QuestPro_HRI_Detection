using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlate : MonoBehaviour
{
    public GameObject plate;
    bool followPlate = false;
    Vector3 deltaPosition;
    Quaternion deltaRotation;
    // Start is called before the first frame update
    void Start()
    {
        // plate = GameObject.Find("plate_middle");
    }

    // Update is called once per frame
    void Update()
    {
        if (followPlate) {
            gameObject.transform.position = plate.transform.position + deltaPosition;
            // rotate with plate
            gameObject.transform.rotation = plate.transform.rotation * deltaRotation;
        }
    }

    // void SetFollowPlateWrapper(){
    //     SetFollowPlate(true);
    // }

    public void SetFollowPlate(bool state){
        followPlate = state;
        if (state == true){
            deltaPosition = plate.transform.position - gameObject.transform.position;
            deltaRotation = plate.transform.rotation * Quaternion.Inverse(gameObject.transform.rotation);
        }
    }
}
