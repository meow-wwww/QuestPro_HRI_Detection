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
            string sceneName = e.Data.Substring(prefix.Length);
            MainThreadDispatcher.Enqueue(() => {
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
