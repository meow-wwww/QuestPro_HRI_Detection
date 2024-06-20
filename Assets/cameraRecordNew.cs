using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using NatML.Recorders;
using NatML.Recorders.Clocks;
using NatML.Recorders.Inputs;
using TMPro;

public class cameraRecordNew : MonoBehaviour
{
    public Camera secondCamera;
    // public string videoFileName = "CaptureVideo.mp4";
    private RenderTexture renderTexture;
    private Texture2D texture2D;
    private MP4Recorder recorder;
    private CameraInput cameraInput;

    public TextMeshPro TMPText;

    // gpt
    // private TaskCompletionSource<bool> stopRecordingCompletionSource;

    void Start()
    {
        secondCamera = gameObject.GetComponent<Camera>();

        // Setup RenderTexture
        renderTexture = new RenderTexture(1920, 1080, 24);
        secondCamera.targetTexture = renderTexture;

        // Setup Texture2D
        texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);

        // Start recording
        StartRecording();

        TMPText.text = "Recording...";

        // Invoke("TestOnApplicationQuit", 15.0f);
    }

    void StartRecording()
    {
        // Create the recorder
        recorder = new MP4Recorder(renderTexture.width, renderTexture.height, 30);
        cameraInput = new CameraInput(recorder, new RealtimeClock(), secondCamera);
    }

    void Update()
    {
        // Ensure the camera is rendered every frame
        secondCamera.Render();
    }

    // async void OnDestroy(){
    //     await StopRecording();
    //     Debug.Log("successfully stop recording");
    // }

    public async Task<string> StopRecording()
    {
        // Stop recording
        cameraInput.Dispose();
        var path = await recorder.FinishWriting();

        // Log the path to the saved video
        Debug.Log($"Saved recording to: {path}");
        return path;
        // TMPText.text = $"Path: {path}";
        // await Task.Delay(10000);
    }

    //gpt
    void OnDisable(){ // OnApplicationQuit
        StartCoroutine(StopRecordingCoroutine());
        Debug.Log("successfully stop recording");
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
            yield return new WaitForSeconds(10);
            TMPText.text = "Record finished.";
            // Application.Quit();
        }
    }
}
