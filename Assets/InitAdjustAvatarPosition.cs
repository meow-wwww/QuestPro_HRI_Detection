using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitAdjustAvatarPosition : MonoBehaviour
{
    public GameObject avatar;
    public GameObject questCamera;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("AdjustAvatarPosition", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void AdjustAvatarPosition(){
        avatar.transform.position = questCamera.transform.position - questCamera.transform.forward.normalized * 50f - questCamera.transform.up.normalized * 1.2f;
       
    }
}