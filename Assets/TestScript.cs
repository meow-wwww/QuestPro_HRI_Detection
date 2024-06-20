using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestScript : MonoBehaviour
{
    public TextMeshPro tmp;
    // Start is called before the first frame update
    void Start()
    {
        tmp.text = Application.persistentDataPath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
