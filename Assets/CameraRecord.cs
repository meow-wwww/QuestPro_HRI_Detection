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
    // public string videoFileName = "CaptureVideo.mp4";
    private RenderTexture renderTexture;
    private Texture2D texture2D;
    private MP4Recorder recorder;
    private CameraInput cameraInput;

    public TextMeshPro TMPText;

    public bool isRecording = false;

    void Start()
    {
        // Setup RenderTexture
        renderTexture = new RenderTexture(1920, 1080, 24);
        secondCamera.targetTexture = renderTexture;

        // Setup Texture2D
        texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);

        // Start recording
        StartRecording();

        TMPText.text = "Recording...";
    }

    void StartRecording()
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

    public async void StopRecordingInterface(){
        await StopRecording();
        Debug.Log("successfully stop recording");
    }

    public async Task<string> StopRecording()
    {
        if (isRecording){
            isRecording = false;
            // Stop recording
            cameraInput.Dispose();
            var path = await recorder.FinishWriting();

            Debug.Log($"Saved recording to: {path}");
            return path;
        }
        else{
            return "Not recording.";
        }
    }

    private IEnumerator StopRecordingCoroutine(){
        var stopRecordingTask = StopRecording();
        yield return new WaitUntil(() => stopRecordingTask.IsCompleted);
        if (stopRecordingTask.Exception != null){
            Debug.LogError(stopRecordingTask.Exception);
        }
        else{
            var path = stopRecordingTask.Result;
            TMPText.text = $"Path: {path}";
            // yield return new WaitForSeconds(10);
            TMPText.text = "Record finished.";
            // Application.Quit();
        }
    }
}
