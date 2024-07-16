using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlate : MonoBehaviour
{
    public GameObject plate;
    bool followPlate = false;
    Vector3 drinkOriginGlobalScale;
    
    // Start is called before the first frame update
    void Start()
    {
        drinkOriginGlobalScale = gameObject.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetFollowPlate(bool state){
        followPlate = state;
        if (state == true){
            // Vector3 drinkOriginGlobalScale = gameObject.transform.lossyScale;
            gameObject.transform.parent = plate.transform;
            // Vector3 parentGlobalScale = plate.transform.lossyScale;
            // gameObject.transform.localScale = new Vector3(drinkOriginGlobalScale.x / parentGlobalScale.x, drinkOriginGlobalScale.y / parentGlobalScale.y, drinkOriginGlobalScale.z / parentGlobalScale.z);
        }
        else{
            gameObject.transform.parent = null;
            // gameObject.transform.localScale = drinkOriginGlobalScale;
        }
    }
}
