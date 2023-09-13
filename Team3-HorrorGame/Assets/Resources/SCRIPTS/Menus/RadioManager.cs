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

        }

        if(powerOn == true)
        {
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
        radioChannelSlider.channelNameText.text = "Channel: " + currentChannelIndex; // Update the channel name text.
    }
}

