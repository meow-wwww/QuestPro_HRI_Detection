using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacementInitialization : MonoBehaviour
{
    GameObject user1Seat, user2Seat;

    [Header("Global Position Info")]
    public Vector3 userForward, userRight;
    public Vector3 userPosition;
    public Vector3 performerPosition;
    public Vector3 robotInitialPosition;

    [Header("Robot References")]
    public GameObject robot;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (user1Seat == null || user2Seat == null){
            user1Seat = GameObject.Find("COUCH");
            user2Seat = GameObject.Find("BED");
            if (user1Seat != null && user2Seat != null){
                FindGlobalPositionInfo(); // only execute once
            }
        }
    }

    void FindGlobalPositionInfo(){
        userPosition = new Vector3(user1Seat.transform.position.x, 0, user1Seat.transform.position.z);
        performerPosition = new Vector3(user2Seat.transform.position.x, 0, user2Seat.transform.position.z);

        userForward = user2Seat.transform.position - user1Seat.transform.position;
        userForward = new Vector3(userForward.x, 0, userForward.z).normalized;
        userRight = new Vector3(userForward.z, 0, -userForward.x);

        // set robots' initial positions
        // waiter robot: 5.5 right + 3 forward
        robot.transform.position = userPosition + userRight * 5.5f + userForward * 3f;
        robotInitialPosition = robot.transform.position;

        GameObject.Find("Coffee_user1").transform.position = userPosition - userRight * 2f + userForward * 0.2f;
        GameObject.Find("Coffee_user2").transform.position = userPosition - userRight * 2f;
        GameObject.Find("Coffee_wrong").transform.position = userPosition - userRight * 2f - userForward * 0.2f;
    }
}
