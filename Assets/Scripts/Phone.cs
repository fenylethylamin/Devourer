using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            UIController.instance.PlayVideoOutro();
            PlayerController.watchingOutroVideo = true;
            GameManager.instance.locker = true;
            GameManager.instance.SetCursor(true);
            GameManager.instance.SetGameState(GameState.WalkMode);
        }
    }
}
