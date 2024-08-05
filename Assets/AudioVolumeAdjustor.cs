using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVolumeAdjustor : MonoBehaviour
{
    AudioSource audioSource;
    ObjectPlacementInitialization globalPositionInfo;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        globalPositionInfo = GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>();
    }

    // Update is called once per frame
    void Update()
    {
        float objectToUserDistance = Vector3.Distance(
            new Vector3(globalPositionInfo.userPosition.x, 0, globalPositionInfo.userPosition.z),
            new Vector3(transform.position.x, 0, transform.position.z)
        );
        float maxDistance = Vector3.Distance(
            new Vector3(globalPositionInfo.userPosition.x, 0, globalPositionInfo.userPosition.z),
            new Vector3(globalPositionInfo.robotInitialPosition.x, 0, globalPositionInfo.robotInitialPosition.z)
        );
        audioSource.volume = 1- objectToUserDistance / maxDistance;
    }
}
