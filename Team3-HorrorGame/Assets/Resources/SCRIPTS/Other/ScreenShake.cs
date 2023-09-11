using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public Transform cameraTransform; // Reference to the camera's Transform component.
    public float shakeDuration = 0.5f; // Duration of the screen shake.
    public float shakeIntensity = 0.2f; // Intensity of the shake.

    private Vector3 originalPosition; // Store the original camera position.
    private float shakeTimer = 0f;

    private void Start()
    {
        // Initialize the original camera position.
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform; // If not specified, use the main camera.
        }
        originalPosition = cameraTransform.localPosition;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            // Shake the camera.
            cameraTransform.localPosition = originalPosition + Random.insideUnitSphere * shakeIntensity;

            // Decrease the timer.
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            // Reset the camera position when the shake duration is over.
            cameraTransform.localPosition = originalPosition;
        }
    }

    // Call this method to trigger the screen shake.
    public void Shake()
    {
        shakeTimer = shakeDuration;
    }
}


