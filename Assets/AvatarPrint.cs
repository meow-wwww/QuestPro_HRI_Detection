using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarPrint : MonoBehaviour
{
    public GameObject avatar;
    public GameObject avatarCamera;
    public GameObject questCamera;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("AdjustAvatarPosition", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        // avatar.transform.position = questCamera.transform.position + questCamera.transform.forward.normalized * 1f - questCamera.transform.up.normalized * 1.2f;
    }

    void AdjustAvatarPosition(){
        avatar.transform.position = questCamera.transform.position - questCamera.transform.forward.normalized * 50f - questCamera.transform.up.normalized * 1.2f;
        // avatarCamera.transform.position = questCamera.transform.position + questCamera.transform.forward.normalized * 1.5f;
    }
}