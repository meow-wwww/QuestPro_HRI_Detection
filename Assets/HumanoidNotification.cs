using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidNotification : MonoBehaviour
{
    InstructionManager instructionManager;
    AudioPlayer requestAudioPlayer;
    public ObjectPlacementInitialization globalPositionInfo; // assigned in Unity Inspector

    // Start is called before the first frame update
    void Start()
    {
        instructionManager = GameObject.Find("InstructionManager").GetComponent<InstructionManager>();
        requestAudioPlayer = gameObject.transform.Find("JulietteY20MP").GetComponent<AudioPlayer>();
    }

    public void SendVoiceRequestWithInstruction(string audioClipName, string instructionText){
        // Debug.Log("+++++++ in SendVoiceRequestWithInstruction:" + audioClipName + " " + instructionText);
        StartCoroutine(WaitForCoroutinesToEnd(new List<IEnumerator> {
            gameObject.GetComponent<ExecuteMovement>().MoveAlongPath_Coroutine(
                new List<Vector3> {globalPositionInfo.robot.transform.position},
                globalPositionInfo.robot.GetComponent<EXPHumanoidOperation>().moveSpeed,
                globalPositionInfo.robot.GetComponent<EXPHumanoidOperation>().rotateSpeed,
                finalRotate: true,
                finalFaceTowards: globalPositionInfo.userPosition
            ),
            SendVoiceRequest_Coroutine(audioClipName),
            instructionManager.SetText_Coroutine(instructionText)
        }));
    }

    public void SendVoiceRequest(string audioClipName){
        StartCoroutine(WaitForCoroutinesToEnd(new List<IEnumerator> {
            gameObject.GetComponent<ExecuteMovement>().MoveAlongPath_Coroutine(
                new List<Vector3> {globalPositionInfo.robot.transform.position},
                globalPositionInfo.robot.GetComponent<EXPHumanoidOperation>().moveSpeed,
                globalPositionInfo.robot.GetComponent<EXPHumanoidOperation>().rotateSpeed,
                finalRotate: true,
                finalFaceTowards: globalPositionInfo.userPosition
            ),
            SendVoiceRequest_Coroutine(audioClipName)
        }));
    }

    private IEnumerator SendVoiceRequest_Coroutine(string audioClipName){
        // change screen image for specific audios
        requestAudioPlayer.PlayAudio("Audio/"+audioClipName);
        yield return null;
    }

    private IEnumerator WaitForCoroutinesToEnd(List<IEnumerator> coroutines){
        foreach (IEnumerator coroutine in coroutines){
            yield return StartCoroutine(coroutine);
        }
    }
}
