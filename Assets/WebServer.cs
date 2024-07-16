using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
using RosMessageTypes.Geometry;
using RosMessageTypes.NiryoMoveit;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;

public class QuestServerBehavior : WebSocketBehavior
{
    // some reference variables used in the experiment
    // public Gameobject waiterRobot;
    // public GameObject droneRobot;

    protected override void OnMessage(MessageEventArgs e){
        Debug.Log("Receive from client: " + e.Data);
        if (e.Data == "Plan") {
            MainThreadDispatcher.Enqueue(() => {
                TrajectoryPlanner trajectoryPlanner = GameObject.Find("Publisher").GetComponent<TrajectoryPlanner>();
                if (trajectoryPlanner != null){
                    trajectoryPlanner.PublishJoints();
                    Send("Plan Success");
                }
                else{
                    Debug.Log("TrajectoryPlanner not found! 404 404 404");
                    Send("Plan Fail: TrajectoryPlanner not found!");
                }
            // Send("Plan Finish");
            });
        }
        else if (e.Data == "Waiter - Move towards the table (half)"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("WaiterRobot").GetComponent<EXPMoveToTable>().MoveToTableHalf();
                Send("Received");
            });
        }
        else if (e.Data == "Waiter - Move to the table user1"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("WaiterRobot").GetComponent<EXPMoveToTable>().MoveToTableUser1();
                Send("Received");       
            });
        }
        else if (e.Data == "Waiter - Move to the table user1 dangerous"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("WaiterRobot").GetComponent<EXPMoveToTable>().MoveToTableUser1Dangerous();
                Send("Received");       
            });
        }
        else if (e.Data == "Waiter - Adjust to the table user1 from dangerous"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("WaiterRobot").GetComponent<EXPMoveToTable>().MoveToTableUser1();//AdjustToTableUser1FromDangerous();
                Send("Received");       
            });
        }
        else if (e.Data == "Waiter - Move to the table user2"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("WaiterRobot").GetComponent<EXPMoveToTable>().MoveToTableUser2();
                Send("Received");
            });
        }
        else if (e.Data == "Waiter - Go away"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("WaiterRobot").GetComponent<EXPMoveToTable>().GoAway();
                Send("Received");
            });
        }
        else if (e.Data == "Waiter - Wander around"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("WaiterRobot").GetComponent<EXPMoveToTable>().WanderAround();
                Send("Received");
            });
        }
        else if (e.Data == "Waiter - Stop wandering"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("WaiterRobot").GetComponent<EXPMoveToTable>().StopWandering();
                Send("Received");
            });
        }


        else if (e.Data == "Waiter - Plate out"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("WaiterRobot").GetComponent<PlateController>().MovePlate(1);
                Send("Received");
            });
        }
        else if (e.Data == "Waiter - Plate back"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("WaiterRobot").GetComponent<PlateController>().MovePlate(-1);
                Send("Received");
            });
        }
        else if (e.Data == "Waiter - Current drink: wrong"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("WaiterRobot").GetComponent<EXPMoveToTable>().SetCurrentDrink(GameObject.Find("Coffee_wrong"));
                Send("Received");
            });
        }
        else if (e.Data == "Waiter - Current drink: user1"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("WaiterRobot").GetComponent<EXPMoveToTable>().SetCurrentDrink(GameObject.Find("Coffee_user1"));
                Send("Received");
            });
        }
        else if (e.Data == "Waiter - Current drink: user2"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("WaiterRobot").GetComponent<EXPMoveToTable>().SetCurrentDrink(GameObject.Find("Coffee_user2"));
                Send("Received");
            });
        }
        else if (e.Data == "Waiter - Current drink: attach"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("WaiterRobot").GetComponent<EXPMoveToTable>().currentDrink.GetComponent<FollowPlate>().SetFollowPlate(true);
                Send("Received");
            });
        }
        else if (e.Data == "Waiter - Current drink: detach"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("WaiterRobot").GetComponent<EXPMoveToTable>().currentDrink.GetComponent<FollowPlate>().SetFollowPlate(false);
                Send("Received");
            });
        }
        else if (e.Data == "Coffee - User2 drink up"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("Coffee_user2").GetComponent<DrinkAction>().DrinkUp();
                Send("Received");
            });
        }
        //// All audios
        else if (e.Data.StartsWith("Waiter - Audio: ")){
            string prefix = "Waiter - Audio: ";
            string audioClipName = e.Data.Substring(prefix.Length);
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("WaiterRobot").transform.Find("Body").Find("screen").GetComponent<RobotScreenNotification>().SendVoiceRequest(audioClipName);
                Send("Received");
            });
        }
        else if (e.Data == "Stop recording"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("PoseCamera").GetComponent<CameraRecord>().StopRecordingInterface();
                GameObject.Find("AvatarRelated").transform.Find("AvatarCamera").GetComponent<CameraRecord>().StopRecordingInterface();
                Send("Received");
            });
        }
        else{
            Send("Unknown Command");
        }
    }
}

public class WebServer : MonoBehaviour {
    private WebSocketServer wss;

    void Start(){
        wss = new WebSocketServer("ws://0.0.0.0:8080");
        wss.AddWebSocketService<QuestServerBehavior>("/Server");
        wss.Start();
        Debug.Log("WebSocket Server started at ws://0.0.0.0:8080/Server");
    }

    void OnDestroy(){
        if (wss != null){
            wss.Stop();
            wss = null;
        }
    }
}
