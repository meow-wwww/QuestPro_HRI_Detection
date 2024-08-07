using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidNotification : MonoBehaviour
{
    InstructionManager instructionManager;
    AudioPlayer requestAudioPlayer;

    // Start is called before the first frame update
    void Start()
    {
        instructionManager = GameObject.Find("InstructionManager").GetComponent<InstructionManager>();
        requestAudioPlayer = gameObject.transform.Find("JulietteY20MP").GetComponent<AudioPlayer>();
    }

    public void SendVoiceRequestWithInstruction(string audioClipName, string instructionText){
        StartCoroutine(WaitForCoroutinesToEnd(new List<IEnumerator> {
            SendVoiceRequest_Coroutine(audioClipName),
            instructionManager.SetText_Coroutine(instructionText)
        }));
    }

    public void SendVoiceRequest(string audioClipName){
        StartCoroutine(SendVoiceRequest_Coroutine(audioClipName));
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
