using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneNotification : MonoBehaviour
{
    InstructionManager instructionManager;
    AudioPlayer requestAudioPlayer;
    public ObjectPlacementInitialization globalPositionInfo; // assigned in Unity inspector
    // GameObject robot;

    // Start is called before the first frame update
    void Start()
    {
        instructionManager = GameObject.Find("InstructionManager").GetComponent<InstructionManager>();
        requestAudioPlayer = gameObject.transform.Find("DroneBody").GetComponent<AudioPlayer>();
        // robot = gameObject.transform.parent.gameObject;
    }

    public void SendVoiceRequestWithInstruction(string audioClipName, string instructionText){
        StartCoroutine(WaitForCoroutinesToEnd(new List<IEnumerator> {
            globalPositionInfo.robot.GetComponent<ExecuteMovement>().MoveAlongPath_Coroutine(
                new List<Vector3> {globalPositionInfo.robot.transform.position},
                globalPositionInfo.robot.GetComponent<EXPDroneOperation>().moveSpeed,
                globalPositionInfo.robot.GetComponent<EXPDroneOperation>().rotateSpeed,
                finalRotate: true,
                finalFaceTowards: new Vector3(globalPositionInfo.userPosition.x, globalPositionInfo.robot.transform.position.y, globalPositionInfo.userPosition.z)
                //globalPositionInfo.userPosition
            ),
            SendVoiceRequest_Coroutine(audioClipName),
            instructionManager.SetText_Coroutine(instructionText)
        }));
    }

    public void SendVoiceRequest(string audioClipName){
        StartCoroutine(WaitForCoroutinesToEnd(new List<IEnumerator> {
            globalPositionInfo.robot.GetComponent<ExecuteMovement>().MoveAlongPath_Coroutine(
                new List<Vector3> {globalPositionInfo.robot.transform.position},
                globalPositionInfo.robot.GetComponent<EXPDroneOperation>().moveSpeed,
                globalPositionInfo.robot.GetComponent<EXPDroneOperation>().rotateSpeed,
                finalRotate: true,
                finalFaceTowards: new Vector3(globalPositionInfo.userPosition.x, globalPositionInfo.robot.transform.position.y, globalPositionInfo.userPosition.z)
            ),
            SendVoiceRequest_Coroutine(audioClipName)
        }));
    }

    private IEnumerator SendVoiceRequest_Coroutine(string audioClipName){
        // change screen image for specific audios
        requestAudioPlayer.PlayAudio("Audio/"+audioClipName);
        yield return null;
    }



    private IEnumerator WaitForCoroutinesToEnd(List<IEnumerator> coroutines)
    {
        foreach (IEnumerator coroutine in coroutines)
        {
            yield return StartCoroutine(coroutine);
        }
    }
}
