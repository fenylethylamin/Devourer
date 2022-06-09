using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuoteAnimation : MonoBehaviour
{
    private Vector3 position;
    
    void Start()
    {
        position = transform.position;
    }

  
    void Update()
    {
        transform.position = position + Vector3.up * Mathf.Sin(Time.time);
    }
}
