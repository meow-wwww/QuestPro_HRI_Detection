using UnityEngine;

public class ArticulationPositionFollow : MonoBehaviour
{
    [SerializeField] GameObject targetObject;
    private Vector3 relativePos;
    private Quaternion relativeRot;

    ArticulationBody articulationBody;

    void Start()
    {
        articulationBody = GetComponent<ArticulationBody>();

        relativePos = transform.position - targetObject.transform.position;
        relativeRot = transform.rotation * Quaternion.Inverse(targetObject.transform.rotation);

        // Vector3 updatedPos = targetObject.transform.position + targetObject.transform.rotation * initialRelativePos;
    }

    void Update()
    {
        articulationBody.TeleportRoot(
            targetObject.transform.position + targetObject.transform.rotation * relativePos,
            targetObject.transform.rotation * relativeRot);
    }
}
