using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StaminaController : MonoBehaviour
{
    [Header("Stamina Main Parameters")]
    public float playerStamina = 100.0f;
    [SerializeField] private float maxStamina = 100.0f;
    [SerializeField] private float jumpCost = 20;
    [HideInInspector] public bool hasRegenerated = true;
    [HideInInspector] public bool weAreSprinting = false;

    [Header("Stamina Regen Parameters")]
    [Range(0, 50)] [SerializeField] private float staminaDrain = 0.5f;
    [Range(0, 50)] [SerializeField] private float staminaRegen = 0.5f;

    [Header("Stamina Speed Parameters")]
    [SerializeField] private int slowedRunSpeed = 4;
    [SerializeField] private int normalRunSpeed = 8;

    [Header("Stamina UI Elements")]
    [SerializeField] private Image staminaProgressUI = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;

    [SerializeField] EvolveGames.PlayerController playerController;
    [SerializeField] GameObject playerObj;
    [SerializeField] private EvolveGames.PlayerController controllerScript;

    private void Start()
    {
        playerController = GetComponent<EvolveGames.PlayerController>();
        controllerScript = playerObj.GetComponent<EvolveGames.PlayerController>();

    }

    private void Update()
    {
        weAreSprinting = controllerScript.isRunning;
        if (!weAreSprinting)
        {
            if (playerStamina <= maxStamina - 0.01) // If player has less than the max amount of stamina...
            {
                playerStamina += staminaRegen * Time.deltaTime; // Regenerate stamina based on time.
                UpdateStamina(1);

                if (playerStamina >= maxStamina) // If player has the max amount of stamina...
                {
                    //set to normal speed
                    sliderCanvasGroup.alpha = 0; // Hide stamina bar.
                    hasRegenerated = true;
                }
            }
        }
    }

    public void Sprinting()
    {
        if (hasRegenerated)
        {
            weAreSprinting = true;
            playerStamina -= staminaDrain * Time.deltaTime;
            UpdateStamina(1);

            if (playerStamina <= 0)
            {
                hasRegenerated = false;
                //slow the player
                sliderCanvasGroup.alpha = 0; 
            }
        }
    }

    public void StaminaJump()
    {
        if (playerStamina >= (maxStamina * jumpCost / maxStamina))
        {
            playerStamina -= jumpCost;
            //allow the player to jump
            UpdateStamina(1);
        }
    }

    void UpdateStamina(int value)
    {
        staminaProgressUI.fillAmount = (playerStamina / maxStamina);

        if (value == 0 )
        {
            sliderCanvasGroup.alpha = 0;
        }
        else
        {
            sliderCanvasGroup.alpha = 1;
        }
    }
}
