using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerJumpScare : MonoBehaviour
{
    public GameObject monster; // Reference to the scary monster GameObject.
    public AudioClip jumpScareSound; // Sound to play during the jump scare.
    private ScreenShake screenShake; // variable for the shaking of the screen
    public GameObject jumpscareTrigger; // object with on trigger collider

    public float disableAfterSeconds = 3.0f;
    public float timer;

     void Start()
    {
        timer = disableAfterSeconds;
        screenShake = GetComponent<ScreenShake>();
    }

    void Update()
    {
        if (monster.activeSelf == true)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0.0f)
        {
            monster.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            // Activate the scary monster.
            monster.SetActive(true);
            
            // Play the jump scare sound.
            AudioSource.PlayClipAtPoint(jumpScareSound, transform.position);

            // You can add other effects like screen shake, flashing lights, etc.
            screenShake.Shake();

            // Optional: Disable the jump scare trigger to prevent it from happening again.
            jumpscareTrigger.SetActive(false);
        }
    }


}

