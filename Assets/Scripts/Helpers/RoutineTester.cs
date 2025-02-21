using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoutineTester : MonoBehaviour
{
    [SerializeField]
    private float firstTime = 5f;
    [SerializeField]
    private float secondTime = 10f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CoroutineHelper.CallAfterSeconds(PrintHelloWorld, firstTime));
        StartCoroutine(CoroutineHelper.CallAfterSeconds(() =>
        {
            Debug.Log("Print this!");
            Debug.Log("And this!");
            PrintHelloWorld(); //And run this!
        }, secondTime));
    }

    private void PrintHelloWorld()
    {
        Debug.Log("Hello, World!");
    }
}
