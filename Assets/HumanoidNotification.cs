using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidNotification : MonoBehaviour
{
    InstructionManager instructionManager;
    AudioPlayer requestAudioPlayer;
    public ObjectPlacementInitialization globalPositionInfo; // assigned in Unity Inspector
    public PepperHeadController headController; // assigned in Unity Inspector

    private IEnumerator ShakeHead_Async(){
        StartCoroutine(ShakeHeadCore());
        yield return null;
    }

    private IEnumerator ShakeHeadCore(){
        headController.headPitchDriveTarget = 20f;
        yield return new WaitForSeconds(1.5f);
        headController.headPitchDriveTarget = 0f;
        yield return new WaitForSeconds(1.5f);
    }

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
                new List<Vector3> {},
                globalPositionInfo.robot.GetComponent<EXPHumanoidOperation>().moveSpeed,
                globalPositionInfo.robot.GetComponent<EXPHumanoidOperation>().rotateSpeed,
                finalRotate: true,
                finalFaceTowards: globalPositionInfo.userPosition
            ),
            SendVoiceRequest_Coroutine(audioClipName),
            ShakeHead_Async(),
            instructionManager.SetText_Coroutine(instructionText)
        }));
    }

    public void SendVoiceRequest(string audioClipName){
        StartCoroutine(WaitForCoroutinesToEnd(new List<IEnumerator> {
            gameObject.GetComponent<ExecuteMovement>().MoveAlongPath_Coroutine(
                new List<Vector3> {},
                globalPositionInfo.robot.GetComponent<EXPHumanoidOperation>().moveSpeed,
                globalPositionInfo.robot.GetComponent<EXPHumanoidOperation>().rotateSpeed,
                finalRotate: true,
                finalFaceTowards: globalPositionInfo.userPosition
            ),
            SendVoiceRequest_Coroutine(audioClipName),
            ShakeHead_Async()
        }));
    }

    public IEnumerator SendVoiceRequest_Coroutine(string audioClipName){
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
