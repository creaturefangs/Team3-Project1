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
    [HideInInspector] public bool canRegen = true;
    [HideInInspector] public bool canSprint = true;
    [HideInInspector] public bool weAreSprinting = false;

    [Header("Stamina Regen Parameters")]
    [Range(0, 50)][SerializeField] private float staminaDrain = 20.0f;
    [Range(0, 50)][SerializeField] private float staminaRegen = 40.0f;

    [Header("Stamina UI Elements")]
    [SerializeField] private Image staminaProgressBar = null;
    [SerializeField] private CanvasGroup sliderCanvasGroup = null;

    [SerializeField] EvolveGames.PlayerController playerController;
    private bool staminaHidden = false;
    private bool changingDisplay = false; // Whether the stamina bar is currently transitioning between being shown/hidden.
    private bool regenerating = false;
    private bool draining = false;

    private void Start()
    {
        playerController = GetComponent<EvolveGames.PlayerController>();
    }

    private void Update()
    {
        weAreSprinting = playerController.isRunning;
        UpdateStamina();

        if (Input.GetKeyDown(KeyCode.Space) && playerStamina >= jumpCost && canSprint)
        {
            StaminaJump();
        }

        if (weAreSprinting)
        {
            if (playerStamina >= 0 && !draining) // If player is not out of stamina...
            {
                StartCoroutine(DrainStamina());
            }

            if (playerStamina <= 0) // If player is out of stamina...
            {
                canSprint = false;
                canRegen = false;
                StartCoroutine(StaminaDrained());
            }
        }

        if (!weAreSprinting)
        {
            if (!canSprint && canRegen && Input.GetKeyDown(KeyCode.LeftShift)) { StartCoroutine(InvalidSprint()); } // Visual feedback for if the player tries to sprint while out of stamina.
            if ((playerStamina <= maxStamina) && canRegen && !regenerating) { StartCoroutine(RegenStamina()); } // Stamina regenerating after a delay of one second, so long as the player can regenerate.
            if (playerStamina >= maxStamina) { canSprint = true; } // If player has the max amount of stamina, they can then sprint.
        }
    }

    private IEnumerator RegenStamina()
    {
        regenerating = true;
        yield return new WaitForSeconds(1.0f);
        while (playerStamina < maxStamina && !weAreSprinting)
        {
            playerStamina += staminaRegen * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        regenerating = false;
    }

    private IEnumerator DrainStamina()
    {
        draining = true;
        while (playerStamina > 0 && weAreSprinting)
        {
            playerStamina -= staminaDrain * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        draining = false;
    }

    public void StaminaJump()
    {
        if (playerStamina >= (maxStamina * jumpCost / maxStamina))
        {
            playerStamina -= jumpCost;
            //allow the player to jump
            UpdateStamina();
        }
    }


    // DISPLAY //
    void UpdateStamina()
    {
        staminaProgressBar.fillAmount = (playerStamina / maxStamina);

        if (!weAreSprinting && (playerStamina >= maxStamina)) // Hides the stamina bar if player isn't sprinting or their stamina isn't regenerating.
        {
            if (!staminaHidden && !changingDisplay) { StartCoroutine(HideStamina(0.05f, false)); }
        }
        else
        {
            if (!changingDisplay) { StartCoroutine(ShowStamina(0.05f)); }
        }
    }

    private IEnumerator ShowStamina(float speed) // Gradually shows the stamina bar. Smaller speed means it shows more quickly.
    {
        staminaHidden = false;
        changingDisplay = true;
        while (sliderCanvasGroup.alpha != 1)
        {
            yield return new WaitForSeconds(speed);
            sliderCanvasGroup.alpha += 0.05f;
        }
        changingDisplay = false;
    }

    private IEnumerator HideStamina(float speed, bool delay) // Gradually hides the stamina bar. Smaller speed mean it hides more quickly.
    {
        staminaHidden = true;
        changingDisplay = true;
        while (sliderCanvasGroup.alpha != 0)
        {
            yield return new WaitForSeconds(speed);
            sliderCanvasGroup.alpha -= 0.05f;
        }
        if (delay) // If there is a delay after the stamina bar is hidden...
        {
            yield return new WaitForSeconds(2.5f);
            canRegen = true;
        }
        changingDisplay = false;
    }


    // FEEDBACK //
    private IEnumerator StaminaDrained() // Stamina bar background flashes red 
    {
        GameObject staminaBG = sliderCanvasGroup.transform.GetChild(0).gameObject;
        Image background = staminaBG.GetComponent<Image>();
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(0.25f);
            background.color = Color.red;
            yield return new WaitForSeconds(0.25f);
            background.color = Color.black;
        }
        StartCoroutine(HideStamina(0.05f, true));
    }

    private IEnumerator InvalidSprint() // Activates when player tries to sprint while unable to.
    {
        if (staminaHidden) { StartCoroutine(ShowStamina(0.01f)); }
        GameObject staminaBG = sliderCanvasGroup.transform.GetChild(0).gameObject;
        Image background = staminaBG.GetComponent<Image>();

        // Bar background flashes red once before going back to normal.
        background.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        background.color = Color.black;

        if (playerStamina <= 0 && !staminaHidden) { StartCoroutine(HideStamina(0.01f, false)); }
    }


    // OLD CODE //
    void RegenerateStamina() // Old code for stamina regen.
    {
        float regeneratedStamina = staminaRegen * Time.deltaTime; // Regenerate stamina based on time.
        if ((playerStamina + regeneratedStamina) > maxStamina) { playerStamina = maxStamina; } // Avoiding going over the max stamina amount.
        else { playerStamina += regeneratedStamina; }
    }

    void VampireDrainStamina() // Old code for stamina drain.
    {
        float drainedStamina = staminaDrain * Time.deltaTime;
        if ((playerStamina - drainedStamina) < 0) { playerStamina = 0; } // Avoiding going under the min stamina amount.
        else { playerStamina -= drainedStamina; }
    }
}
