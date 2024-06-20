using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class QuestServerBehavior : WebSocketBehavior
{
    protected override void OnMessage(MessageEventArgs e){
        Debug.Log("Receive from client: " + e.Data);
        Send("OK");
    }
}

public class WebServer : MonoBehaviour {
    private WebSocketServer wss;

    void Start(){
        wss = new WebSocketServer("ws://0.0.0.0:8080");
        wss.AddWebSocketService<QuestServerBehavior>("/Server");
        wss.Start();
        Debug.Log("WebSocket Server started at ws://0.0.0.0:8080/Server");
    }

    void OnDestroy(){
        if (wss != null){
            wss.Stop();
            wss = null;
        }
    }
}
