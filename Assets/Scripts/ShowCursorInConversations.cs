using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCursorInConversations : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.instance.StartConversation();

        }
    }
}
