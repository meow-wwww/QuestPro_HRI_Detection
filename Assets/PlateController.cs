using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateController : MonoBehaviour
{
    public GameObject topPlate;


    public float plateMoveSpeed = 1f;
    public float plateMoveDistance = 1f;
    // Start is called before the first frame update
    void Start()
    {
        // Invoke("MovePlate", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovePlate(int direction)
    {
        // direction: 1 out, -1 in
        StartCoroutine(MovePlate_Coroutine(topPlate, plateMoveDistance, plateMoveSpeed, direction));
    }

    private IEnumerator MovePlate_Coroutine(GameObject plate, float distance, float moveSpeed, int direction)
    {
        Vector3 startPosition = plate.transform.position;
        while (Vector3.Distance(plate.transform.position, startPosition + gameObject.transform.forward * direction * distance) > 0.01f)
        {
            // Debug.Log("Moving plate ... ..." + Vector3.Distance(plate.transform.position, startPosition + Vector3.forward * distance));
            plate.GetComponent<Rigidbody>().MovePosition(plate.transform.position + gameObject.transform.forward * moveSpeed * direction * Time.deltaTime);
            yield return null;
        }
    }
}
