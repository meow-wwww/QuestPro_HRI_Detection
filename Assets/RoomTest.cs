using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class RoomTest : MonoBehaviour
{
    public MRUK mrukScript;

    // public List<MRUKRoom> rooms;
    public MRUKRoom currentRoom;
    public List<MRUKAnchor> anchors;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PrintRoomInfo(){
        currentRoom = mrukScript.GetRooms()[0];
        anchors = currentRoom.GetRoomAnchors();
    }
}
