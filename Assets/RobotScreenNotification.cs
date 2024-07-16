using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotScreenNotification : MonoBehaviour
{
    public GameObject quad;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScreenImage(string filename){
        quad.GetComponent<Renderer>().material.mainTexture = Resources.Load<Texture2D>(filename);
    }

    public void DrinkReady(){
        gameObject.GetComponent<AudioPlayer>().PlayAudio("Audio/Ding");
    }
}
