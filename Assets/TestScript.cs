using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public WaiterCatcherController controller;
    public int functionId;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable(){
        switch (functionId){
            case 0:
                controller.OpenCatcher();
                break;
            case 1:
                controller.ForwardCatcher(0.1f);
                break;
            case 2:
                controller.LiftCatcher(0.05f);
                break;
            // case 3:
            //     controller.TestOpenCatcher();
            //     break;
        }
    }

    void OnDisable(){
        switch (functionId){
            case 0:
                controller.CloseCatcher();
                break;
            case 1:
                controller.BackwardCatcher(0.1f);
                break;
            case 2:
                controller.LowerCatcher(0.05f);
                break;
        }
    }
}
