using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class WriteFileTest : MonoBehaviour
{
    public TextMeshPro tmp;
    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(Application.dataPath); // on PC, it is the Assets folder
        // filePath = Path.Combine(Application.dataPath, "test.txt");
        // filePath = Application.

        // Invoke("WriteFile", 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTMP(){
        tmp.text = "button pressed";
        // using (StreamWriter writer = new StreamWriter(filePath)){
        //     writer.WriteLine("Hello World!");
        // }
    }
}
