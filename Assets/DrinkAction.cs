using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkAction : MonoBehaviour
{
    GameObject coffee;

    void Start()
    {
        coffee = transform.Find("coffee").gameObject;
    }

    public void DrinkUp()
    {
        coffee.transform.localScale = new Vector3(coffee.transform.localScale.x, coffee.transform.localScale.y * 0.1f, coffee.transform.localScale.z);
    }

}
