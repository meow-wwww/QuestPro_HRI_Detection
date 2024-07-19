using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetInstruction : MonoBehaviour
{
    TextMeshPro tmp;

    void Start()
    {
        tmp = GetComponent<TextMeshPro>();
    }

    public void SetText(string text)
    {
        tmp.text = text;
    }

    void ClearText()
    {
        tmp.text = "";
    }
}
