using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Miniblock : MonoBehaviour
{
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.AddForce(Random.Range(0f, 1.5f),Random.Range(2f, 3f),Random.Range(0f, 1.5f), ForceMode.Impulse);

    }

   
    void Update()
    {
        
    }
}
