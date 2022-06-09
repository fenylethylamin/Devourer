using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointController : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            CheckpointManager.Instance.UpdateCurrentCheckpoint(transform);
        }
    }
    //public string cpName;


    //void Start()
    //{
    //    if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_cp"))
    //    {
    //        if (PlayerPrefs.GetString(SceneManager.GetActiveScene().name + "_cp") == cpName)
    //        {
    //            PlayerController.instance.transform.position = transform.position;
    //            Debug.Log("Player starting at " + cpName);
    //        }
    //    }
    //}

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_cp", "");
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        PlayerPrefs.SetString(SceneManager.GetActiveScene().name + "_cp", cpName);
    //        Debug.Log("Player hit " + cpName);

    //        ES3AutoSaveMgr.Current.Save();

    //    }

    //}

}

