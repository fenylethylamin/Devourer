using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
 

{
    [SerializeField] private GameObject introVideoObject, videoTextureObject;
    [Tooltip("")]
    [SerializeField] private float videoLength = 46f;
    private UnityEngine.Video.VideoPlayer videoPlayer;
    private bool isVideoPlaying = false;

    private float videoTimeElapsed = 0f;
    public string firstLevel;
    public GameObject continueButton;
    public GameObject quitButton;
    public GameObject gameTitle;
    public GameObject startButton;
    public GameObject skipButton;
    

   void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            if (PlayerPrefs.GetString("CurrentLevel") == "")
            {
                continueButton.SetActive(false);
            }
        }
        else
        {
            continueButton.SetActive(false);
        }

        introVideoObject.SetActive(false);
        videoTextureObject.SetActive(false);
        skipButton.SetActive(false);
        videoPlayer = introVideoObject.GetComponent<UnityEngine.Video.VideoPlayer>();

    }

    private void Update()
    {
        if(isVideoPlaying)
        {
            videoTimeElapsed += Time.unscaledDeltaTime;

            if (videoTimeElapsed >= videoLength)
            {
                PlayGame();
            }
        }
    }

    public void Continue()
    {
        SceneManager.LoadScene(PlayerPrefs.GetString("CurrentLevel"));
    }

    public void StartVideo()
    {
        introVideoObject.SetActive(true);
        continueButton.SetActive(false);
        quitButton.SetActive(false);
        gameTitle.SetActive(false);
        startButton.SetActive(false);
        skipButton.SetActive(true);
      //  videoTextureObject.SetActive(true);

        isVideoPlaying = true;

    }

    public void SkipVideo()
    {
        videoTimeElapsed = videoLength;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(firstLevel);

        PlayerPrefs.SetString("CurrentLevel", "");

        PlayerPrefs.SetString(firstLevel + "_cp", "");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting Game");
    }
}
