using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    public Slider healthSlider;
    public Text healthText,ammoText;

    public Image damageEffect;
    public float damageAlpha = .25f, damageFadeSpeed = 2f;

    public GameObject pauseScreen;

    [Header("Outro Video")]
    [SerializeField] private GameObject outroVideoObject;
    [SerializeField] private float videoLength = 46f;
    [SerializeField] private GameObject skipButton;
    [SerializeField] private GameObject[] objectsToDisable;
    private UnityEngine.Video.VideoPlayer videoPlayer;
    private float videoTimeElapsed = 0f;
    private bool isVideoPlaying = false;
    private void Awake()
{
    instance = this;
}

    private void Start()
    {
        videoPlayer = outroVideoObject.GetComponent<UnityEngine.Video.VideoPlayer>();
        outroVideoObject.SetActive(false);
        skipButton.SetActive(false);
    }

    void Update()
    {
        if (damageEffect.color.a != 0)
        {
            damageEffect.color = new Color(damageEffect.color.r, damageEffect.color.g, damageEffect.color.b, Mathf.MoveTowards(damageEffect.color.a, 0f, damageFadeSpeed * Time.deltaTime));
        }



        if (isVideoPlaying)
        {
            videoTimeElapsed += Time.unscaledDeltaTime;

            if (videoTimeElapsed >= videoLength)
            {
                pauseScreen.GetComponent<PauseScreen>().MainMenu();
            }
        }
    }

    public void PlayVideoOutro()
    {
        isVideoPlaying = true;
        outroVideoObject.SetActive(true);
        skipButton.SetActive(true);
        
        foreach(GameObject GO in objectsToDisable)
        {
            GO.SetActive(false);
        }
    }

    public void SkipVideo()
    {
        videoTimeElapsed = videoLength;
    }

    public void ShowDamage()
    {
        damageEffect.color = new Color(damageEffect.color.r, damageEffect.color.g, damageEffect.color.b, .25f);
    }

}
