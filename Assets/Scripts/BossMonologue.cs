using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonologue : MonoBehaviour

{
    public int indexMonologue;
    private bool isCollected;

    public GameObject monologueWindow;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !isCollected)
        {
            GameManager.instance.Monologue.PlayMonologue(indexMonologue);

            Destroy(gameObject);

        }
    }
}