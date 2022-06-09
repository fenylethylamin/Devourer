using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyConversationTrigger : MonoBehaviour
{
    private bool isCollected;



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isCollected)
        {
            Destroy(gameObject);

        }
    }
}

