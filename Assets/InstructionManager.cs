using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionManager : MonoBehaviour
{
    public GameObject instruction; // assigned in Unity Inspector
    public float instructionShowTime;

    // Start is called before the first frame update
    void Start()
    {
        System.Diagnostics.Debug.Assert(instruction!= null, "Instruction object not assigned in Unity Inspector");
    }

    public void SetText(string text)
    {
        StartCoroutine(SetText_Coroutine(text));
    }

    public IEnumerator SetText_Coroutine(string text, bool returnImmediately=false)
    {
        instruction.SetActive(true);
        instruction.transform.Find("Text").GetComponent<TMPro.TextMeshPro>().text = text;
        Invoke("Disappear", instructionShowTime);
        if (!returnImmediately){
            // sleep for 4 seconds then return
            yield return new WaitForSeconds(instructionShowTime);
        }
        else{
            yield return null;
        }
    }

    void Disappear()
    {
        instruction.SetActive(false);
    }
}
