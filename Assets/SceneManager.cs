using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;
using UnityEngine.SceneManagement;

public class SceneManagerBehavior : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e){
        Debug.Log("Receive from client: " + e.Data);
        
        if (e.Data.StartsWith("Change to scene: ")) {
            string prefix = "Change to scene: ";
            string sceneNameAndMode = e.Data.Substring(prefix.Length);
            string sceneName = sceneNameAndMode.Split(' ')[0];
            string robot = sceneNameAndMode.Split(' ')[1];
            string mode = sceneNameAndMode.Split(' ')[2];
            MainThreadDispatcher.Enqueue(() => {
                PlayerPrefs.SetString("mode", mode); // pass the mode (Standing/Sitting) to specific scene
                PlayerPrefs.SetString("robot", robot); // pass the robot (Waiter/Drone/...) to specific scene
                PlayerPrefs.Save();
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
                Send("Received");
            });
        }
        else{
            Send("Unknown Command");
        }
    }
}

public class SceneManager : MonoBehaviour {
    private WebSocketServer wss;

    void Start(){
        wss = new WebSocketServer("ws://0.0.0.0:8081");
        wss.AddWebSocketService<SceneManagerBehavior>("/Server");
        wss.Start();
    }

    void OnDestroy(){
        if (wss != null){
            wss.Stop();
            wss = null;
        }
    }
}
