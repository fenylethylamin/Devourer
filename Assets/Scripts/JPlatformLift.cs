using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JPlatformLift : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
            other.transform.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
            GetComponentInParent<Animator>().SetTrigger("playerOnPlatform");
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
            other.transform.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
        }
    }
}
