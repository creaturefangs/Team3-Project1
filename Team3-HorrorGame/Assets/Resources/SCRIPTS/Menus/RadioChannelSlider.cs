using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RadioChannelSlider : MonoBehaviour
{
    public TMP_Text channelNameText;
    public string[] channelNames;
    public RadioManager radioManager;
    private Slider slider;
    

    private void Start()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(ChangeChannel);
    }

    private void ChangeChannel(float value)
    {
        int index = Mathf.RoundToInt(value * (channelNames.Length -1));
        channelNameText.text = channelNames[index];
        // Here, you can implement logic to actually change the radio channel.
        // You may use events or a radio manager script for this purpose.
        radioManager.PlayChannel(index); // Call the PlayChannel method of the RadioManager.
    }

}

