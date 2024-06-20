using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBoneRecord : MonoBehaviour
{
    public GameObject bones;
    public GameObject headCameraObject;

    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        // poseCamera.transform.localPosition = new Vector3(0, 0, 0);
        // Invoke("ChangePoseCameraPos", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (bones == null){
            bones = GameObject.Find("Bones");
            offset = gameObject.transform.position - headCameraObject.transform.position;
            SetLayerRecursively(bones, LayerMask.NameToLayer("Character"));
        }
        else {
            gameObject.transform.position = headCameraObject.transform.position + offset;
        }
        // Debug.Log("PoseCamera pos:" + poseCamera.transform.position);
    }

    void SetLayerRecursively(GameObject obj, int layer){
        if (obj == null) return;
        obj.layer = layer;
        foreach (Transform child in obj.transform) {
            if (child == null) continue;
            SetLayerRecursively(child.gameObject, layer);
        }
    }
}
