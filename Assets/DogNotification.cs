using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogNotification : MonoBehaviour
{
    InstructionManager instructionManager;
    AudioPlayer requestAudioPlayer;
    public ObjectPlacementInitialization globalPositionInfo; // assigned in Unity Inspector

    // Start is called before the first frame update
    void Start()
    {
        instructionManager = GameObject.Find("InstructionManager").GetComponent<InstructionManager>();
        requestAudioPlayer = gameObject.transform.Find("spot1/base_link").GetComponent<AudioPlayer>();
    }

    public void SendVoiceRequestWithInstruction(string audioClipName, string instructionText, bool moveToUser1=false){
        List<IEnumerator> coroutinesList = new List<IEnumerator> {
            gameObject.GetComponent<ExecuteMovement>().MoveAlongPath_ROS_Coroutine(
                new List<Vector3> {},
                globalPositionInfo.robot.GetComponent<EXPDogOperation>().moveSpeed,
                globalPositionInfo.robot.GetComponent<EXPDogOperation>().rotateSpeed,
                finalRotate: true,
                finalFaceTowards: globalPositionInfo.userPosition
            ),
            SendVoiceRequest_Coroutine(audioClipName),
            instructionManager.SetText_Coroutine(instructionText)
        };
        if (moveToUser1){
            // insert to the first element
            coroutinesList.Insert(0, gameObject.GetComponent<ExecuteMovement>().MoveAlongPath_ROS_Coroutine(
                new List<Vector3> {
                    gameObject.GetComponent<EXPDogOperation>().user2Peripheral,
                    gameObject.GetComponent<EXPDogOperation>().middlePoint,
                    gameObject.GetComponent<EXPDogOperation>().user1Peripheral
                },
                globalPositionInfo.robot.GetComponent<EXPDogOperation>().moveSpeed,
                globalPositionInfo.robot.GetComponent<EXPDogOperation>().rotateSpeed,
                finalRotate: false
            ));
        }
        StartCoroutine(WaitForCoroutinesToEnd(coroutinesList));
    }

    public void SendVoiceRequest(string audioClipName){
        StartCoroutine(WaitForCoroutinesToEnd(new List<IEnumerator> {
            gameObject.GetComponent<ExecuteMovement>().MoveAlongPath_ROS_Coroutine(
                new List<Vector3> {},
                globalPositionInfo.robot.GetComponent<EXPDogOperation>().moveSpeed,
                globalPositionInfo.robot.GetComponent<EXPDogOperation>().rotateSpeed,
                finalRotate: true,
                finalFaceTowards: globalPositionInfo.userPosition
            ),
            SendVoiceRequest_Coroutine(audioClipName)
        }));
    }

    public IEnumerator SendVoiceRequest_Coroutine(string audioClipName){
        requestAudioPlayer.PlayAudio("Audio/"+audioClipName);
        yield return null;
    }

    private IEnumerator WaitForCoroutinesToEnd(List<IEnumerator> coroutines){
        foreach (IEnumerator coroutine in coroutines){
            yield return StartCoroutine(coroutine);
        }
    }
}
