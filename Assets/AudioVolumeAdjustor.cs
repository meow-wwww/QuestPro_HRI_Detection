using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioVolumeAdjustor : MonoBehaviour
{
    AudioSource audioSource;
    ObjectPlacementInitialization globalPositionInfo;
    public string volumeMode = "distance";
    
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        globalPositionInfo = GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>();
    }

    void Update()
    {
        if (volumeMode == "distance"){
            float objectToUserDistance = Vector3.Distance(
                new Vector3(globalPositionInfo.userPosition.x, 0, globalPositionInfo.userPosition.z),
                new Vector3(transform.position.x, 0, transform.position.z)
            );
            float maxDistance = Vector3.Distance(
                new Vector3(globalPositionInfo.userPosition.x, 0, globalPositionInfo.userPosition.z),
                new Vector3(globalPositionInfo.robotInitialPosition.x, 0, globalPositionInfo.robotInitialPosition.z)
            ) * 1.1f;
            audioSource.volume = 1 - (float)Math.Pow(objectToUserDistance / maxDistance, 2);
        }
        else if (volumeMode == "fixed"){
            audioSource.volume = 1;
        }
        else {
            Debug.LogError("Invalid volume mode");
        }
    }
}
