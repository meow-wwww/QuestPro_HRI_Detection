using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlate : MonoBehaviour
{
    public GameObject plate;
    bool followPlate = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFollowPlate(bool state){
        followPlate = state;
        if (state == true){
            gameObject.transform.SetParent(plate.transform, worldPositionStays: true);
        }
        else{
            gameObject.transform.SetParent(null, worldPositionStays: true);
        }
    }
}
