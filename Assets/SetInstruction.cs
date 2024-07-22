using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetInstruction : MonoBehaviour
{
    TextMeshPro tmp;

    void Start()
    {
        tmp = transform.Find("Text").GetComponent<TextMeshPro>();
    }

    // public void SetText(string text)
    // {
    //     gameObject.SetActive(true);
    //     tmp.text = text;
    //     Invoke("Disappear", 5f);
    // }

    void Disappear()
    {
        gameObject.SetActive(false);
    }
}
