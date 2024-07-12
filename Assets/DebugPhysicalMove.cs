using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPhysicalMove : MonoBehaviour
{
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        InvokeRepeating("Move", 6f, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Move(){
        rb.MovePosition(transform.position + new Vector3(0, 0, 0.008f));
    }
}
