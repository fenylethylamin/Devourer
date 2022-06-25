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
            position.x += Random.Range(-3.5f, 3f);
            position.z += Random.Range(-3.5f, 3.5f);


            Instantiate(_objectToSpawn[i], position, Quaternion.Euler(Random.Range(0.0f, 0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 0f)));
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
