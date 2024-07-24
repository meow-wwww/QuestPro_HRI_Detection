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

                if (e.Data == "Waiter - Move towards the table (half)"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().MoveToTableHalf_Fixed();
                        Send("Received");
                    });
                }
                else if (e.Data == "Waiter - Move to the table user1"){
                    MainThreadDispatcher.Enqueue(() => {
                        // GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().MoveToTableUser1(rigid: true);
                        GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().MoveToTableUser1_Fixed();
                        Send("Received");       
                    });
                }
                else if (e.Data == "Waiter - Move to the table user1 collision"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().MoveToTableUser1Collision_Fixed();
                        Send("Received");       
                    });
                }
                else if (e.Data == "Waiter - Move to the table user1 dangerous"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().MoveToTableUser1Dangerous_Fixed();
                        // MoveToTableUser2(rigid: true);
                        Send("Received");       
                    });
                }
                else if (e.Data == "Waiter - Move to the table user1 from collision"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().MoveToTableUser1FromCollision_Fixed();
                        Send("Received");       
                    });
                }
                else if (e.Data == "Waiter - Move to the table user2"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().MoveToTableUser2_Fixed();
                        // MoveToTableUser2(rigid: true);
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
            else if (e.Data == "Waiter - Send out drink"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().SendOutDrink();
                    Send("Received");
                });
            }
            else if (e.Data == "Waiter - Send out drink dangerous"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().SendOutDrink(dangerous: true);
                    GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>().SetDrinkPositionIndicator(false);
                    Send("Received");
                });
            }
            else if (e.Data == "Waiter - Collect drink"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().CollectDrink();
                    Send("Received");
                });
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
                    if (audioClipName == "WhereShouldIPlace")
                        GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>().SetDrinkPositionIndicator(true);
                    Send("Received");
                });
            }
            else if (e.Data == "Waiter - Audio info stop"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("WaiterRobot").transform.Find("Body").Find("screen").GetComponent<RobotScreenNotification>().StopVoice();
                    Send("Received");
                });
            }
        }
        else if (e.Data.StartsWith("Drone - ")){ ////////////////////////// drone robot
            if (e.Data.StartsWith("Drone - Move ")){
                if (e.Data == "Drone - Move towards the table (half)"){
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
                else if (e.Data == "Drone - Move to the table user1 above"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().MoveToTableUser1(above: true);
                        Send("Received");
                    });
                }
                else if (e.Data == "Drone - Move to the table user1 collision"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().MoveToTableUser1Collision();
                        Send("Received");
                    });
                }
                else if (e.Data == "Drone - Move to the table user1 dangerous"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().MoveToTableUser1Dangerous();
                        GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>().SetDrinkPositionIndicator(false);
                        Send("Received");
                    });
                }
                else if (e.Data == "Drone - Move to the table user2"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().MoveToTableUser2();
                        Send("Received");
                    });
                }
                else if (e.Data == "Drone - Move up"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().MoveUp(height: 0.4f);
                        Send("Received");
                    });
                }
                else if (e.Data == "Drone - Move away"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().GoAway();
                        Send("Received");
                    });
                }
                else if (e.Data == "Drone - Move around"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().WanderAround();
                        Send("Received");
                    });
                }
            }
            else if (e.Data.StartsWith("Drone - Current drink: ")){
                if (e.Data == "Drone - Current drink: user2"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().SetCurrentDrink(GameObject.Find("Coffee_user2"));
                        Send("Received");
                    });
                }
                else if (e.Data == "Drone - Current drink: user1"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().SetCurrentDrink(GameObject.Find("Coffee_user1"));
                        Send("Received");
                    });
                }
                else if (e.Data == "Drone - Current drink: wrong"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().SetCurrentDrink(GameObject.Find("Coffee_wrong"));
                        Send("Received");
                    });
                }
                else if (e.Data == "Drone - Current drink: attach"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().SetCurrentDrink(
                            GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().currentDrink
                        );
                        Send("Received");
                    });
                }
                else if (e.Data == "Drone - Current drink: detach"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().SetCurrentDrink(null);
                        Send("Received");
                    });
                }
                else if (e.Data == "Drone - Current drink: dangerous"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().currentDrink.GetComponent<DrinkAction>().Dangerous();
                        Send("Received");
                    });
                }
            }
            else if (e.Data == "Drone - Stop wandering"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().StopWandering();
                    Send("Received");
                });
            }
            else if (e.Data == "Drone - Open cup catchers"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DroneRobot").transform.Find("CupCatcher").gameObject.GetComponent<DroneCatcherController>().OpenCatcher();
                    Send("Received");
                });
            }
            else if (e.Data == "Drone - Close cup catchers"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DroneRobot").transform.Find("CupCatcher").gameObject.GetComponent<DroneCatcherController>().CloseCatcher();
                    Send("Received");
                });
            }
            // else if (e.Data == "Drone - Open lifters"){
            //     MainThreadDispatcher.Enqueue(() => {
            //         GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().OpenLifter();
            //         Send("Received");
            //     });
            // }
            // else if (e.Data == "Drone - Close lifters"){
            //     MainThreadDispatcher.Enqueue(() => {
            //         GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().CloseLifter(true);
            //         Send("Received");
            //     });
            // }
            else if (e.Data.StartsWith("Drone - Audio: ")){
                string prefix = "Drone - Audio: ";
                string audioClipName = e.Data.Substring(prefix.Length);
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DroneRobot").transform.Find("DroneBody").GetComponent<AudioPlayer>().PlayAudio("Audio/" + audioClipName);
                    if (audioClipName == "WhereShouldIPlace")
                        GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>().SetDrinkPositionIndicator(true);
                    Send("Received");
                });
            }
            else if (e.Data == "Drone - Audio info stop"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DroneRobot").transform.Find("DroneBody").GetComponent<AudioPlayer>().StopAudio();
                    Send("Received");
                });
            }
            else{
                Send("Unknown Command");
            }
        }


        ////////////////////////// Other shared by all robots
        else if (e.Data == "Coffee - User2 drink up"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("Coffee_user2").GetComponent<DrinkAction>().DrinkUp();
                Send("Received");
            });
        }
        else if (e.Data == "Coffee1 - Enable interaction"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("Coffee_user1").GetComponent<DrinkAction>().SetInteractionMode(true);
                Send("Received");
            });
        }
        ////////////////////////// system control
        else if (e.Data == "Start recording"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("PoseCamera").GetComponent<CameraRecord>().StartRecording();
                GameObject.Find("AvatarRelated").transform.Find("AvatarCamera").GetComponent<CameraRecord>().StartRecording();
                GameObject.Find("MainCameraRecord").GetComponent<CameraRecord>().StartRecording();
                Send("Received");
            });
        }
        else if (e.Data == "Stop recording"){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("PoseCamera").GetComponent<CameraRecord>().StopRecordingInterface();
                GameObject.Find("AvatarRelated").transform.Find("AvatarCamera").GetComponent<CameraRecord>().StopRecordingInterface();
                GameObject.Find("MainCameraRecord").GetComponent<CameraRecord>().StopRecordingInterface();
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
        else if (e.Data.StartsWith("Instruction - ")){
            MainThreadDispatcher.Enqueue(() => {
                GameObject.Find("InstructionManager").GetComponent<InstructionManager>().SetText(e.Data.Substring("Instruction - ".Length));
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
