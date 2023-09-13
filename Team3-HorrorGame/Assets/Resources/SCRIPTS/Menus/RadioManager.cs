using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioManager : MonoBehaviour
{
    bool sliderActive;
    public GameObject radioUI;
    bool powerOn; 

    // Start is called before the first frame update
    void Start()
    {
        powerOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (powerOn == false)
        {

        }

        if(powerOn == true)
        {

        }
    }

    public void SliderActive()
    {
        // on interact the ui for the Radio will display
        radioUI.SetActive(true);
        sliderActive = true;

        if (sliderActive == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }
}
