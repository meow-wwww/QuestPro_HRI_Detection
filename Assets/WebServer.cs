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
                else if (e.Data.StartsWith("Waiter - Move to the table user1#")){
                    string instructionText = e.Data.Substring("Waiter - Move to the table user1#".Length);
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("WaiterRobot").GetComponent<EXPWaiterOperation>().MoveToTableUser1_Fixed(instructionText);
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
            //// All audios
            else if (e.Data.StartsWith("Waiter - Audio: ")){
                string prefix = "Waiter - Audio: ";
                string audioClipName_Instruction = e.Data.Substring(prefix.Length);
                // if "#" in audioClipName_Instruction, then the string after "#" is the instruction text, before is the audio clip name
                if (audioClipName_Instruction.Contains("#")){
                    string audioClipName = audioClipName_Instruction.Split('#')[0];
                    string instructionText = audioClipName_Instruction.Split('#')[1];
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("WaiterRobot").transform.Find("Body").Find("screen").GetComponent<RobotScreenNotification>().SendVoiceRequestWithInstruction(audioClipName, instructionText);
                        if (audioClipName == "WhereShouldIPlace")
                            GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>().SetDrinkPositionIndicator(true);
                        Send("Received");
                    });
                }
                else{
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("WaiterRobot").transform.Find("Body").Find("screen").GetComponent<RobotScreenNotification>().SendVoiceRequest(audioClipName_Instruction);
                        if (audioClipName_Instruction == "WhereShouldIPlace")
                            GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>().SetDrinkPositionIndicator(true);
                        Send("Received");
                    });
                }
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
                else if (e.Data.StartsWith("Drone - Move to the table user1#")){
                    string instructionText = e.Data.Substring("Drone - Move to the table user1#".Length);
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().MoveToTableUser1(instructionText: instructionText);
                        Send("Received");
                    });
                }
                else if (e.Data.StartsWith("Drone - Move to the table user1 above#")){
                    string instructionText = e.Data.Substring("Drone - Move to the table user1 above#".Length);
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().MoveToTableUser1(above: true, instructionText: instructionText);
                        Send("Received");
                    });
                }
                else if (e.Data == "Drone - Move to the table user1 collision"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().MoveToTableUser1Collision();
                        Send("Received");
                    });
                }
                else if (e.Data == "Drone - Move to the table user2"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().MoveToTableUser2();
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
            else if (e.Data == "Drone - Send out drink"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().SendOutDrink();
                    Send("Received");
                });
            }
            else if (e.Data == "Drone - Send out drink dangerous"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().SendOutDrink(dangerous: true);
                    GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>().SetDrinkPositionIndicator(false);
                    Send("Received");
                });
            }
            else if (e.Data == "Drone - Collect drink"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().CollectDrink();
                    Send("Received");
                });
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
            }
            else if (e.Data == "Drone - Stop wandering"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().StopWandering();
                    Send("Received");
                });
            }
            else if (e.Data == "Drone - Open cup catchers"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DroneRobot").GetComponent<EXPDroneOperation>().OpenCupCatcher();
                    Send("Received");
                });
            }
            else if (e.Data.StartsWith("Drone - Audio: ")){
                string prefix = "Drone - Audio: ";
                // string audioClipName = e.Data.Substring(prefix.Length);
                string audioClipName_Instruction = e.Data.Substring(prefix.Length);
                if (audioClipName_Instruction.Contains("#")){
                    string audioClipName = audioClipName_Instruction.Split('#')[0];
                    string instructionText = audioClipName_Instruction.Split('#')[1];
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<DroneNotification>().SendVoiceRequestWithInstruction(audioClipName, instructionText);
                        if (audioClipName == "WhereShouldIPlace")
                            GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>().SetDrinkPositionIndicator(true);
                        Send("Received");
                    });
                }
                else{
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DroneRobot").GetComponent<DroneNotification>().SendVoiceRequest(audioClipName_Instruction);
                        if (audioClipName_Instruction == "WhereShouldIPlace")
                            GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>().SetDrinkPositionIndicator(true);
                        Send("Received");
                    });
                }
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
        else if (e.Data.StartsWith("Humanoid - ")){
            string command = e.Data.Substring("Humanoid - ".Length);
            if (command.StartsWith("Move ")){
                if (command == "Move towards the table (half)"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("HumanoidRobot").GetComponent<EXPHumanoidOperation>().MoveToTableHalf_Fixed();
                        Send("Received");
                    });
                }
                else if (command.StartsWith("Move to the table user1#")){
                    string instructionText = command.Substring("Move to the table user1#".Length);
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("HumanoidRobot").GetComponent<EXPHumanoidOperation>().MoveToTableUser1_Fixed(instructionText);
                        Send("Received");       
                    });
                }
                else if (command == "Move to the table user1 collision"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("HumanoidRobot").GetComponent<EXPHumanoidOperation>().MoveToTableUser1Collision_Fixed();
                        Send("Received");       
                    });
                }
                else if (command == "Move to the table user1 dangerous"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("HumanoidRobot").GetComponent<EXPHumanoidOperation>().MoveToTableUser1Dangerous_Fixed();
                        // MoveToTableUser2(rigid: true);
                        Send("Received");       
                    });
                }
                else if (command == "Move to the table user1 from collision"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("HumanoidRobot").GetComponent<EXPHumanoidOperation>().MoveToTableUser1FromCollision_Fixed();
                        Send("Received");       
                    });
                }
                else if (command == "Move to the table user2"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("HumanoidRobot").GetComponent<EXPHumanoidOperation>().MoveToTableUser2_Fixed();
                        // MoveToTableUser2(rigid: true);
                        Send("Received");
                    });
                }
                else if (command == "Move away"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("HumanoidRobot").GetComponent<EXPHumanoidOperation>().GoAway();
                        Send("Received");
                    });
                }
                else if (command == "Move around"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("HumanoidRobot").GetComponent<EXPHumanoidOperation>().WanderAround();
                        Send("Received");
                    });
                }
                else{
                    Send("Unknown Command");
                }
            }
            else if (command == "Send out drink"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("HumanoidRobot").GetComponent<EXPHumanoidOperation>().SendOutDrink();
                    Send("Received");
                });
            }
            else if (command == "Send out drink dangerous"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("HumanoidRobot").GetComponent<EXPHumanoidOperation>().SendOutDrink(dangerous: true);
                    GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>().SetDrinkPositionIndicator(false);
                    Send("Received");
                });
            }
            else if (command == "Collect drink"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("HumanoidRobot").GetComponent<EXPHumanoidOperation>().CollectDrink();
                    Send("Received");
                });
            }
            else if (command == "Stop wandering"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("HumanoidRobot").GetComponent<EXPHumanoidOperation>().StopWandering();
                    Send("Received");
                });
            }
            else if (command == "Current drink: wrong"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("HumanoidRobot").GetComponent<EXPHumanoidOperation>().SetCurrentDrink(GameObject.Find("Coffee_wrong"));
                    Send("Received");
                });
            }
            else if (command == "Current drink: user1"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("HumanoidRobot").GetComponent<EXPHumanoidOperation>().SetCurrentDrink(GameObject.Find("Coffee_user1"));
                    Send("Received");
                });
            }
            else if (command == "Current drink: user2"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("HumanoidRobot").GetComponent<EXPHumanoidOperation>().SetCurrentDrink(GameObject.Find("Coffee_user2"));
                    Send("Received");
                });
            }
            //// All audios
            else if (command.StartsWith("Audio: ")){
                string prefix = "Audio: ";
                string audioClipName_Instruction = command.Substring(prefix.Length);
                // if "#" in audioClipName_Instruction, then the string after "#" is the instruction text, before is the audio clip name
                if (audioClipName_Instruction.Contains("#")){
                    string audioClipName = audioClipName_Instruction.Split('#')[0];
                    string instructionText = audioClipName_Instruction.Split('#')[1];
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("HumanoidRobot").gameObject.GetComponent<HumanoidNotification>().SendVoiceRequestWithInstruction(audioClipName, instructionText);
                        if (audioClipName == "WhereShouldIPlace")
                            GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>().SetDrinkPositionIndicator(true);
                        Send("Received");
                    });
                }
                else{
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("HumanoidRobot").gameObject.GetComponent<HumanoidNotification>().SendVoiceRequest(audioClipName_Instruction);
                        if (audioClipName_Instruction == "WhereShouldIPlace")
                            GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>().SetDrinkPositionIndicator(true);
                        Send("Received");
                    });
                }
            }
            else if (command == "Audio info stop"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("HumanoidRobot").transform.Find("JulietteY20MP").gameObject.GetComponent<AudioPlayer>().StopAudio();
                    Send("Received");
                });
            }
        }
        else if (e.Data.StartsWith("Dog - ")){
            string command = e.Data.Substring("Dog - ".Length);
            if (command.StartsWith("Move ")){
                if (command == "Move towards the table (half)"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DogRobot").GetComponent<EXPDogOperation>().MoveToTableHalf_Fixed();
                        Send("Received");
                    });
                }
                else if (command.StartsWith("Move to the table user1#")){
                    string instructionText = command.Substring("Move to the table user1#".Length);
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DogRobot").GetComponent<EXPDogOperation>().MoveToTableUser1_Fixed(instructionText);
                        Send("Received");       
                    });
                }
                else if (command == "Move to the table user1 collision"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DogRobot").GetComponent<EXPDogOperation>().MoveToTableUser1Collision_Fixed();
                        Send("Received");       
                    });
                }
                else if (command == "Move to the table user1 dangerous"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DogRobot").GetComponent<EXPDogOperation>().MoveToTableUser1Dangerous_Fixed();
                        // MoveToTableUser2(rigid: true);
                        Send("Received");       
                    });
                }
                else if (command == "Move to the table user1 from collision"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DogRobot").GetComponent<EXPDogOperation>().MoveToTableUser1FromCollision_Fixed();
                        Send("Received");       
                    });
                }
                else if (command == "Move to the table user2"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DogRobot").GetComponent<EXPDogOperation>().MoveToTableUser2_Fixed();
                        // MoveToTableUser2(rigid: true);
                        Send("Received");
                    });
                }
                else if (command == "Move away"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DogRobot").GetComponent<EXPDogOperation>().GoAway();
                        Send("Received");
                    });
                }
                else if (command == "Move around"){
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DogRobot").GetComponent<EXPDogOperation>().WanderAround();
                        Send("Received");
                    });
                }
                else{
                    Send("Unknown Command");
                }
            }
            else if (command == "Send out drink"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DogRobot").GetComponent<EXPDogOperation>().SendOutDrink();
                    Send("Received");
                });
            }
            else if (command == "Send out drink dangerous"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DogRobot").GetComponent<EXPDogOperation>().SendOutDrink(dangerous: true);
                    GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>().SetDrinkPositionIndicator(false);
                    Send("Received");
                });
            }
            else if (command == "Collect drink"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DogRobot").GetComponent<EXPDogOperation>().CollectDrink();
                    Send("Received");
                });
            }
            else if (command == "Stop wandering"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DogRobot").GetComponent<EXPDogOperation>().StopWandering();
                    Send("Received");
                });
            }
            else if (command == "Current drink: wrong"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DogRobot").GetComponent<EXPDogOperation>().SetCurrentDrink(GameObject.Find("Coffee_wrong"));
                    Send("Received");
                });
            }
            else if (command == "Current drink: user1"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DogRobot").GetComponent<EXPDogOperation>().SetCurrentDrink(GameObject.Find("Coffee_user1"));
                    Send("Received");
                });
            }
            else if (command == "Current drink: user2"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DogRobot").GetComponent<EXPDogOperation>().SetCurrentDrink(GameObject.Find("Coffee_user2"));
                    Send("Received");
                });
            }
            //// All audios
            else if (command.StartsWith("Audio: ")){
                string prefix = "Audio: ";
                string audioClipName_Instruction = command.Substring(prefix.Length);
                // if "#" in audioClipName_Instruction, then the string after "#" is the instruction text, before is the audio clip name
                if (audioClipName_Instruction.Contains("#")){
                    string audioClipName = audioClipName_Instruction.Split('#')[0];
                    string instructionText = audioClipName_Instruction.Split('#')[1];
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DogRobot").gameObject.GetComponent<DogNotification>().SendVoiceRequestWithInstruction(audioClipName, instructionText);
                        if (audioClipName == "WhereShouldIPlace")
                            GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>().SetDrinkPositionIndicator(true);
                        Send("Received");
                    });
                }
                else{
                    MainThreadDispatcher.Enqueue(() => {
                        GameObject.Find("DogRobot").gameObject.GetComponent<DogNotification>().SendVoiceRequest(audioClipName_Instruction);
                        if (audioClipName_Instruction == "WhereShouldIPlace")
                            GameObject.Find("MRUK").GetComponent<ObjectPlacementInitialization>().SetDrinkPositionIndicator(true);
                        Send("Received");
                    });
                }
            }
            else if (command == "Audio info stop"){
                MainThreadDispatcher.Enqueue(() => {
                    GameObject.Find("DogRobot").transform.Find("spot1").gameObject.GetComponent<AudioPlayer>().StopAudio();
                    Send("Received");
                });
            }
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
        else if (e.Data.StartsWith("Stop recording - ")){
            MainThreadDispatcher.Enqueue(() => {
                string newFilename = e.Data.Substring("Stop recording - ".Length);
                GameObject.Find("PoseCamera").GetComponent<CameraRecord>().StopRecordingInterface(newFilename);
                GameObject.Find("AvatarRelated").transform.Find("AvatarCamera").GetComponent<CameraRecord>().StopRecordingInterface(newFilename);
                GameObject.Find("MainCameraRecord").GetComponent<CameraRecord>().StopRecordingInterface(newFilename);
                Send("Received");
            });
        }
        else if (e.Data.StartsWith("Save context - ")){
            MainThreadDispatcher.Enqueue(() => {
                int trial_id = int.Parse(e.Data.Substring("Save context - ".Length));
                int result = GameObject.Find("ResetManager").GetComponent<ResetObjects>().SaveContext(trial_id);
                Send("Received; " + result);
            });
        }
        else if (e.Data.StartsWith("Reset context - ")){
            MainThreadDispatcher.Enqueue(() => {
                int trial_id = int.Parse(e.Data.Substring("Reset context - ".Length));
                int result = GameObject.Find("ResetManager").GetComponent<ResetObjects>().ResetContext(trial_id);
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
