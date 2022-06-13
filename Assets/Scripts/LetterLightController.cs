using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterLightController : MonoBehaviour
{
    [SerializeField] private float lightIntensity = 2;
    [SerializeField] private Vector3 lightPos = new Vector3(0f, 0.381f, 0.304f);
    [SerializeField] private float innerSpotAngle = 96.3f;
    [SerializeField] private float outerSpotAngle = 102.87f;
    [SerializeField] private float range = 1f;
    private Light spotLight;
    // Start is called before the first frame update
    void Start()
    {
        spotLight = GetComponentInChildren<Light>();
        if (spotLight != null)
        {
            spotLight.range = range;
            spotLight.intensity = lightIntensity;
            spotLight.gameObject.transform.localPosition = lightPos;
            spotLight.spotAngle = outerSpotAngle;
            spotLight.innerSpotAngle = innerSpotAngle;
        }
           
    }
}
