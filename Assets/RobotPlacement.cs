using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class RobotPlacement : MonoBehaviour
{
    public GameObject robotWhole;
    public GameObject robotTarget;
    public Vector3 robotTargetOffset = new Vector3(0.3f, 0, 0);
    public GameObject robotTargetPlacement;
    public Vector3 robotTargetPlacementOffset = new Vector3(-0.3f, 0, 0);
    public GameObject robotBottom;
    public GameObject targetFurniture;
    public MRUK mrukScript;

    // public Vector3 debugRobotBottomPosition, debugTargetFurnitureTopPosition;
    // public Vector3 debugRigidBodyCenter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaceRobotOnFurniture_1s(){
        Invoke("PlaceRobotOnFurniture", 1.0f);
    }

    public void PlaceRobotOnFurniture(){
        MRUKRoom currentRoom = mrukScript.GetRooms()[0];
        targetFurniture = currentRoom.gameObject.transform.Find("TABLE").gameObject;

        if (targetFurniture.transform.childCount == 1){
            targetFurniture = targetFurniture.transform.GetChild(0).gameObject;

            Vector3 robotBottomPosition = robotBottom.transform.position;
            Vector3 targetFurnitureTopPosition = targetFurniture.GetComponent<BoxCollider>().bounds.center + new Vector3(0, targetFurniture.GetComponent<BoxCollider>().bounds.extents.y, 0);

            robotTarget.transform.position = targetFurnitureTopPosition + robotTargetOffset;
            robotTarget.SetActive(true);

            robotTargetPlacement.transform.position = targetFurnitureTopPosition + robotTargetPlacementOffset;
            robotTargetPlacement.SetActive(true);

            robotWhole.transform.position = robotWhole.transform.position + (targetFurnitureTopPosition - robotBottomPosition);
            robotWhole.SetActive(true);
            // Debug.Log("++++++++ The robot is set active");
        }
        else {
            Debug.Log("!!!!!! target furniture has more than one child or no child !!!!!!" + targetFurniture.transform.childCount);
        }
    }
}
