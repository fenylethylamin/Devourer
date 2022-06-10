using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideCursorAfterConversations : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.EndConversation();

        }
    }
}
