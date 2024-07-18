using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkAction : MonoBehaviour
{
    GameObject coffee;

    void Start()
    {
        coffee = transform.Find("coffee_cup").Find("coffee").gameObject;
    }

    public void DrinkUp()
    {
        coffee.transform.localScale = new Vector3(coffee.transform.localScale.x, coffee.transform.localScale.y * 0.1f, coffee.transform.localScale.z);
    }

    public void Dangerous(){
        // gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(-20f, 0f, 0f);
        // gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x - 0.012f, gameObject.transform.localPosition.y - 0.005f, gameObject.transform.localPosition.z);
        StartCoroutine(Dangerous_Coroutine());
    }

    private IEnumerator Dangerous_Coroutine(float moveSpeed=0.1f, float rotateSpeed=20f)
    {
        Quaternion targetRotation = gameObject.transform.localRotation * Quaternion.Euler(-20f, 0f, 0f);
        Vector3 targetPosition = gameObject.transform.localPosition - new Vector3(0.012f, 0.005f, 0f);
        while (Vector3.Distance(gameObject.transform.localPosition, targetPosition) > 0.005f || Quaternion.Angle(gameObject.transform.localRotation, targetRotation) > 1f){
            if (Vector3.Distance(gameObject.transform.localPosition, targetPosition) > 0.005f){
                gameObject.transform.localPosition = Vector3.MoveTowards(gameObject.transform.localPosition, targetPosition, moveSpeed * Time.deltaTime);
            }
            if (Quaternion.Angle(gameObject.transform.localRotation, targetRotation) > 1f){
                gameObject.transform.localRotation = gameObject.transform.localRotation * Quaternion.Euler(rotateSpeed * Time.deltaTime * -1, 0, 0);
            }
            yield return null;
        }
    }
}
