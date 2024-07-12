using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneLifterController : MonoBehaviour
{
    public Rigidbody leftLifterRigidbody;
    public Rigidbody rightLifterRigidbody;
    public GameObject rigidbodyParentObject;
    public GameObject leftLifterChild;
    public GameObject rightLifterChild;
    GameObject leftLifter;
    GameObject rightLifter;

    public float openRotationZ = 105f;
    public float rotationSpeed = 6f;

    GameObject frontHolder;
    GameObject backHolder;

    // Start is called before the first frame update
    void Start()
    {
        leftLifter = leftLifterRigidbody.gameObject;
        rightLifter = rightLifterRigidbody.gameObject;
        // frontHolder: find this gameobject's child object that named "FrontHolder"
        frontHolder = transform.Find("plate_lifters").Find("FrontHolder_Unseen").gameObject;
        backHolder = transform.Find("plate_lifters").Find("BackHolder_Unseen").gameObject;
        Invoke("OpenLifter", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OpenLifter(){ // 99.8 -> 105
        Debug.Log("OpenLifter() called");
        // first disable the front and back holder of the drone (they are only needed when the lifter is closed)
        frontHolder.SetActive(false);
        backHolder.SetActive(false);
        // best practice: the drone bring the plate to guests; open lifter and disable holders; then keep them open.

        StartCoroutine(Rotate_Rigidbody_Coroutine(leftLifterRigidbody.gameObject.transform.localRotation * Quaternion.Euler(0, 0, (openRotationZ - 99.8f)), leftLifterRigidbody, rotationSpeed));
        StartCoroutine(Rotate_Rigidbody_Coroutine(rightLifterRigidbody.gameObject.transform.localRotation * Quaternion.Euler(0, 0, (openRotationZ - 99.8f)), rightLifterRigidbody, rotationSpeed));

        // Debug.Log("LeftLifterChild" + leftLifterChild.transform.parent.name);
        // Debug.Log("RightLifterChild" + rightLifterChild.transform.parent.name);
    }

    void CloseLifter(){ // 105 -> 99.8

    }

    private IEnumerator Rotate_Rigidbody_Coroutine(Quaternion targetRotation, Rigidbody rigidbody, float rotateSpeed)
    {
        // Vector3 initialOffset = rigidbody.gameObject.transform.
        while (Quaternion.Angle(rigidbody.gameObject.transform.localRotation, targetRotation) > 0.5f)
        {
            // rigidbody.MoveRotation(rigidbody.gameObject.transform.rotation * Quaternion.Euler(0, 0, rotateSpeed*Time.deltaTime));
            rigidbody.gameObject.transform.localRotation = rigidbody.gameObject.transform.localRotation * Quaternion.Euler(0, 0, rotateSpeed*Time.deltaTime);
            yield return null;
        }
    }

}
