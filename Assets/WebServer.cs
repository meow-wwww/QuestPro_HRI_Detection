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
        if (e.Data.StartsWith("Waiter - ")){
            if (e.Data.StartsWith("Waiter - Move ")){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("WaiterRobot").transform.Find("Body").Find("screen").GetComponent<RobotScreenNotification>().SetScreenImage("CatSleep");
                });
                // GameObject.Find("WaiterRobot").transform.Find("Body").Find("screen").GetComponent<RobotScreenNotification>().SetScreenImage("CatSleep");

                if (e.Data == "Waiter - Move towards the table (half)"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().MoveToTableHalf();
                        Send("Received");
                    });
                }
                else if (e.Data == "Waiter - Move to the table user1"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().MoveToTableUser1();
                        Send("Received");       
                    });
                }
                else if (e.Data == "Waiter - Move to the table user1 dangerous"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().MoveToTableUser1Dangerous();
                        Send("Received");       
                    });
                }
                else if (e.Data == "Waiter - Move to the table user2"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().MoveToTableUser2();
                        Send("Received");
                    });
                }
                else if (e.Data == "Waiter - Move away"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().GoAway();
                        Send("Received");
                    });
                }
                else if (e.Data == "Waiter - Move around"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().WanderAround();
                        Send("Received");
                    });
                }
                else{
                    Send("Unknown Command");
                }
            }
            else if (e.Data == "Waiter - Stop wandering"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().StopWandering();
                    Send("Received");
                });
            }
            // Plate related
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
                    GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().SetCurrentDrink(GameObject.Find("Coffee_wrong"));
                    Send("Received");
                });
            }
            else if (e.Data == "Waiter - Current drink: user1"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().SetCurrentDrink(GameObject.Find("Coffee_user1"));
                    Send("Received");
                });
            }
            else if (e.Data == "Waiter - Current drink: user2"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().SetCurrentDrink(GameObject.Find("Coffee_user2"));
                    Send("Received");
                });
            }
            else if (e.Data == "Waiter - Current drink: attach"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().currentDrink.GetComponent<FollowPlate>().SetFollowPlate(true);
                    Send("Received");
                });
            }
            else if (e.Data == "Waiter - Current drink: detach"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().currentDrink.GetComponent<FollowPlate>().SetFollowPlate(false);
                    GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().currentDrink = null;
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
        }
        else if (e.Data.StartsWith("Drone - ")){ ////////////////////////// drone robot
            if (e.Data == "Drone - Current drink: user2"){

            }
            else if (e.Data == "Drone - Move towards the table (half)"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().MoveToTableHalf();
                    Send("Received");
                });
            }
            else if (e.Data == "Drone - Move to the table user1"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().MoveToTableUser1();
                    Send("Received");
                });
            }
            else if (e.Data == "Drone - Move to the table user1 dangerous"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().MoveToTableUser1Dangerous();
                    Send("Received");
                });
            }
            else if (e.Data == "Drone - Move to the table user2"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().MoveToTableUser2();
                    Send("Received");
                });
            }
            else if (e.Data == "Drone - Go away"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().GoAway();
                    Send("Received");
                });
            }
            else if (e.Data == "Drone - Wander around"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().WanderAround();
                    Send("Received");
                });
            }
            else if (e.Data == "Drone - Stop wandering"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().StopWandering();
                    Send("Received");
                });
            }
        }


        ////////////////////////// Other shared by all robots
        else if (e.Data == "Coffee - User2 drink up"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("Coffee_user2").GetComponent<DrinkAction>().DrinkUp();
                Send("Received");
            });
        }
        ////////////////////////// system control
        else if (e.Data == "Stop recording"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("PoseCamera").GetComponent<CameraRecord>().StopRecordingInterface();
                GameObject.Find("AvatarRelated").transform.Find("AvatarCamera").GetComponent<CameraRecord>().StopRecordingInterface();
                Send("Received");
            });
        }
        else if (e.Data == "Save context"){
            MainThreadDispatcher.Enqueue(() => {
                bool result = GameObject.Find("ResetManager").GetComponent<ResetObjects>().SaveContext();
                Send("Received; " + result);
            });
        }
        else if (e.Data == "Reset context"){
            MainThreadDispatcher.Enqueue(() => {
                bool result = GameObject.Find("ResetManager").GetComponent<ResetObjects>().ResetContext();
                Send("Received; " + result);
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
