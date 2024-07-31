using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotScreenNotification : MonoBehaviour
{
    public GameObject quad; // assigned in Unity inspector
    public InstructionManager instructionManager; 


    private IEnumerator WaitForCoroutinesToEnd(List<IEnumerator> coroutines)
    {
        foreach (IEnumerator coroutine in coroutines)
        {
            yield return StartCoroutine(coroutine);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        instructionManager = GameObject.Find("InstructionManager").GetComponent<InstructionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScreenImage(string filename){
        quad.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture2D>("Image/" + filename);
    }

    public void DrinkReady(){
        gameObject.GetComponent<AudioPlayer>().PlayAudio("Audio/Ding");
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
        if (audioClipName == "AskForFeedback" || audioClipName == "HowMayIHelpYou" || audioClipName == "WhereShouldIPlace")
            SetScreenImage("CatQuestion");
        gameObject.GetComponent<AudioPlayer>().PlayAudio("Audio/"+audioClipName);
        yield return null;
    }

    public void StopVoice(){
        gameObject.GetComponent<AudioPlayer>().StopAudio();
    }
}
