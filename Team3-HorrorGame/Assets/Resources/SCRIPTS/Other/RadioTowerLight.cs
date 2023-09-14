using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioTowerLight : MonoBehaviour
{
    GameObject realLight;
    GameObject uiLight;

    int maxIntensity = 50000;
    int minIntensity = 500;

    // Start is called before the first frame update
    void Start()
    {
        realLight = transform.GetChild(0).gameObject;
        uiLight = transform.GetChild(1).GetChild(0).gameObject;
        StartCoroutine(Blink());
    }

    // Update is called once per frame
    void Update()
    {
        uiLight.transform.LookAt(GameObject.Find("PlayerController").transform.position);
        float distance = Vector3.Distance(transform.position, GameObject.Find("PlayerController").transform.position);
        if (distance < 200)
        {
            realLight.GetComponent<Light>().intensity = ((distance / 200) / 10) * maxIntensity;
        }
        else { realLight.GetComponent<Light>().intensity = maxIntensity; }
    }

    private IEnumerator Blink()
    {
        Image image = uiLight.GetComponent<Image>();
        Color opacity = image.color;
        while (true)
        {
            opacity.a = 1;
            image.color = opacity;
            realLight.SetActive(true);
            yield return new WaitForSeconds(1f);
            opacity.a = 0;
            image.color = opacity;
            realLight.SetActive(false);
            yield return new WaitForSeconds(1f);
        }
    }
}
