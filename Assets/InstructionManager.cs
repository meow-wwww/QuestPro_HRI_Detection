using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionManager : MonoBehaviour
{
    public GameObject instruction; // assigned in Unity Inspector

    // Start is called before the first frame update
    void Start()
    {
        System.Diagnostics.Debug.Assert(instruction!= null, "Instruction object not assigned in Unity Inspector");
    }

    public void SetText(string text)
    {
        instruction.SetActive(true);
        instruction.transform.Find("Text").GetComponent<TMPro.TextMeshPro>().text = text;
        Invoke("Disappear", 5f);
    }

    void Disappear()
    {
        instruction.SetActive(false);
    }
}
