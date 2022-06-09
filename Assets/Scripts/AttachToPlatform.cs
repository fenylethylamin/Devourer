using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToPlatform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.gameObject.transform.SetParent(this.gameObject.transform);
            other.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
            Debug.Log("ATTACHED");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.transform.SetParent(null);
            other.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
            Debug.Log("DETTACHED");
        }
    }
}
