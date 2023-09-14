using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioManager : MonoBehaviour
{
    bool sliderActive;
    public GameObject radioUI;
    public bool powerOn;
    public AudioSource radioAudioSource;
    public AudioClip[] channelClips;
    private int currentChannelIndex = 0;
    public RadioChannelSlider radioChannelSlider;


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
            // if the radio tower is without power, the static plays.
        }

        if(powerOn == true)
        {
            // if the radio tower has been given power, the audio plays
            PlayChannel(currentChannelIndex);
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

    public void PlayChannel(int channelIndex)
    {
        //plays audio clip when channel is changed
        if (channelIndex >= 0 && channelIndex < channelClips.Length)
        {
            currentChannelIndex = channelIndex;
            radioAudioSource.clip = channelClips[currentChannelIndex];
            radioAudioSource.Play();
            UpdateChannelName();
        }
    }

    private void UpdateChannelName()
    {
        //finds channel name and converts it into text
        radioChannelSlider.channelNameText.text = "Channel: " + currentChannelIndex; // Update the channel name text.
    }

    public void SetPowerOn()
    {
        powerOn = true;
    }
}

