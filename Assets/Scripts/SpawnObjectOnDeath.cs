using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EnemyHealthController))]
public class SpawnObjectOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectToSpawn;
    [SerializeField] private GameObject _deadEmoji;


    public void SpawnObject()
    {
        Vector3 position = transform.GetChild(0).position;
        for (int i = 0; i < _objectToSpawn.Length; i++)
        {
            position.x += Random.Range(-1f, 1f);
            position.z += Random.Range(-1f, 1f);
         

            Instantiate(_objectToSpawn[i], position, Quaternion.identity);
        }

        _deadEmoji.transform.position = transform.position;

        ChangeToDead();
    }

    public void ChangeToDead()
    {
        _deadEmoji.SetActive(true);
        gameObject.SetActive(false);
    }
}
