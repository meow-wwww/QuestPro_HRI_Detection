using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarScript : MonoBehaviour
{
    public GameObject avatar, avatarCamera;
    public GameObject questCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        avatar.transform.position = questCamera.transform.position - questCamera.transform.forward * 2f - questCamera.transform.up * 1.2f;
        // avatar.transform.rotation = questCamera.transform.rotation; // don't know why this is not needed; but removing this line works.
        
        avatarCamera.transform.position = avatar.transform.position + 1.2f * questCamera.transform.up + 0.8f * questCamera.transform.forward;
        // avatarCamera.transform.up = questCamera.transform.up;
        // avatarCamera.transform.forward = -questCamera.transform.forward;
        avatarCamera.transform.rotation = Quaternion.LookRotation(-questCamera.transform.forward, questCamera.transform.up);
        
        
        
        // avatarCamera.transform.position = questCamera.transform.position - questCamera.transform.forward * 1.5f - questCamera.transform.up * 1.2f;

        // new Vector3(0f, 1.6f, 0.5f);
        
        
        // avatarCamera.transform.forward = -questCamera.transform.forward;
    }
}
