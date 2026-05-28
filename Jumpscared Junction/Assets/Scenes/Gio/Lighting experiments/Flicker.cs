using UnityEngine;
using System.Collections;

public class FluorescentLight : MonoBehaviour
{
    Light flickerLight;

    [Header("Normal Flicker")]
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.2f;

    [Header("Burst Flicker")]
    public float burstChance = 0.3f;        // chance of triggering a flicker burst
    public int burstFlickerCount = 5;        // how many times it flickers in a burst
    public float burstFlickerSpeed = 0.04f; // how fast during burst

    [Header("Off State")]
    public float offChance = 0.1f;          // chance of turning fully off
    public float minOffTime = 0.1f;
    public float maxOffTime = 0.5f;

    [Header("Timing")]
    public float minIdleTime = 0.5f;
    public float maxIdleTime = 2f;

    void Start()
    {
        flickerLight = GetComponent<Light>();
        StartCoroutine(FluorescentFlicker());
    }

    IEnumerator FluorescentFlicker()
    {
        while (true)
        {
            // idle at normal brightness
            flickerLight.enabled = true;
            flickerLight.intensity = Random.Range(minIntensity, maxIntensity);
            yield return new WaitForSeconds(Random.Range(minIdleTime, maxIdleTime));

            float roll = Random.value;

            if (roll < offChance)
            {
                // fully turn off for a moment
                flickerLight.enabled = false;
                yield return new WaitForSeconds(Random.Range(minOffTime, maxOffTime));
            }
            else if (roll < offChance + burstChance)
            {
                // rapid burst fli
                for (int i = 0; i < burstFlickerCount; i++)
                {
                    flickerLight.enabled = !flickerLight.enabled;
                    yield return new WaitForSeconds(burstFlickerSpeed);
                }
                flickerLight.enabled = true;
            }
        }
    }
}