using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using NatML.Recorders;
using NatML.Recorders.Clocks;
using NatML.Recorders.Inputs;
using TMPro;
using static OVRInput;

public class CameraRecord : MonoBehaviour
{
    public Camera secondCamera;
    private RenderTexture renderTexture;
    private Texture2D texture2D;
    private MP4Recorder recorder;
    private CameraInput cameraInput;

    public bool isRecording = false;

    void Start()
    {
        // Setup RenderTexture
        renderTexture = new RenderTexture(1920, 1080, 24);
        secondCamera.targetTexture = renderTexture;

        // Setup Texture2D
        texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);

        // Start recording
        // StartRecording();
    }

    public void StartRecording()
    {
        // Create the recorder
        isRecording = true;
        recorder = new MP4Recorder(renderTexture.width, renderTexture.height, 30);
        cameraInput = new CameraInput(recorder, new RealtimeClock(), secondCamera);
    }

    void Update()
    {
        // OVRInput.Update();
        // Ensure the camera is rendered every frame
        if (isRecording){
            secondCamera.Render();
        }
    }

    public async void StopRecordingInterface(string filename=""){
        // await StopRecording(filename);
        // get the return value
        string path = await StopRecording(filename);
        // return path;
    }

    public async Task<string> StopRecording(string filename)
    {
        if (isRecording){
            isRecording = false;
            // Stop recording
            cameraInput.Dispose();
            var path = await recorder.FinishWriting();
            var newPath = "";

            if (filename != ""){
                // Rename the file in the file system. Add an suffix to the filename, eg. 20230101.mp4 -> 20230101_<suffix>.mp4
                newPath = path.Replace(".mp4", $"_{filename}.mp4");
                System.IO.File.Move(path, newPath);
            }
            else {
                newPath = path;
            }

            Debug.Log($"Saved recording to: {newPath}");
            return newPath;
        }
        else{
            return "Not recording.";
        }
    }

    // private IEnumerator StopRecordingCoroutine(){
    //     var stopRecordingTask = StopRecording("");
    //     yield return new WaitUntil(() => stopRecordingTask.IsCompleted);
    //     if (stopRecordingTask.Exception != null){
    //         Debug.LogError(stopRecordingTask.Exception);
    //     }
    //     else{
    //         var path = stopRecordingTask.Result;
    //     }
    // }
}
