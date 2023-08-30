using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EvolveGames;
using UnityEngine.SceneManagement;
using System;

public class Visibility : MonoBehaviour
{
    public GameObject playerObject;
    private Animator iconAnimator;
    private Image visIcon;
    private Image visOverlay;
    private ItemChange itemScript;
    private StaminaController staminaScript;

    public float visibility = 0;

    [Header("Visibility Parameters")]
    // Vis gain and loss numbers do NOT add/subtract directly to visibility and end up multiplying w/time mod before being factored in.
    [Range(0, 10)] public float lightVisGain = 1.0f; // How much visibility is gained from light sources.
    [Range(0, 10)] public float noiseVisGain = 2.0f; // How much visibility is gained from making noise (like sprinting).
    [Range(0, 10)] public float visLoss = 3.0f; // How much to subtract from visibility on each interval.
    [Range(0, 50)] public int maxVisibility = 25; // The maximum amount of visibility a player can have before game over.
    [Header("Visibility Thresholds")]
    public int visSafe = 5;
    public int visCaution = 10;
    public int visDanger = 20;

    private bool playerVisible = false;
    private int currentItem; // The current item the player is holding (as int/index).
    private bool isSprinting;
    private bool visChange = false;
    private bool sprintMod = false;

    // Start is called before the first frame update
    void Start()
    {
        itemScript = playerObject.GetComponent<ItemChange>();
        staminaScript = playerObject.GetComponent<StaminaController>();

        visOverlay = gameObject.transform.GetChild(0).GetComponent<Image>();
        visIcon = gameObject.transform.GetChild(1).GetComponent<Image>();

        iconAnimator = gameObject.transform.GetChild(1).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        VisibilityStatus();

        isSprinting = staminaScript.weAreSprinting;
        if (isSprinting && !sprintMod) { StartCoroutine(SprintVisibility()); } // If player is sprinting and sprint is not already being applied to visibility...

        currentItem = itemScript.ItemIdInt;
        if (currentItem == 3 || currentItem == 4) { playerVisible = true; } // Player gets increasing visibility if holding flashlight/lantern.
        else { playerVisible = false; }

        if (playerVisible && !visChange)
        {
            StartCoroutine(GainVisibility());
        }

        if (!playerVisible && !visChange)
        {
            StartCoroutine(LoseVisibility());
        }
    }

    void VisibilityStatus() // Effects that take place based on the level of the player's visibility.
    {
        // Handle if statements in descending order since otherwise it would stick to the first "if" statement (since it's the lowest and wouldn't check the other else-if statements).
        if (visibility >= maxVisibility)
        {
            //set the cursor to visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("GAMEOVER");
        }

        else if (visibility >= visDanger)
        {
            ChangeIcon("Visibility-Danger");
            iconAnimator.SetBool("Animate", true);
        }

        else if (visibility >= visCaution)
        {
            ChangeIcon("Visibility-Caution");
            iconAnimator.SetBool("Animate", false);
        }

        else if (visibility >= visSafe)
        {
            ChangeIcon("Visibility-Safe");
        }

        else if (visibility < visSafe)
        {
            ChangeIcon("Visibility-Hidden");
        }
    }

    void ChangeIcon(string name) // Changes the visibility icon.
    {
        Sprite newIcon = Resources.Load<Sprite>("2D/UI/" + name);
        visIcon.sprite = newIcon;
    }

    private IEnumerator GainVisibility()
    {
        visChange = true;
        while (playerVisible && visibility < maxVisibility)
        {
            visibility += lightVisGain * Time.deltaTime;
            UpdateOverlay();
            yield return new WaitForSeconds(Time.deltaTime);
        }
        visChange = false;
    }

    private IEnumerator LoseVisibility()
    {
        visChange = true;
        while (!playerVisible && !isSprinting && visibility > 0)
        {
            visibility -= visLoss * Time.deltaTime;
            UpdateOverlay();
            yield return new WaitForSeconds(Time.deltaTime);
        }
        visChange = false;
    }

    private IEnumerator SprintVisibility()
    {
        sprintMod = true;
        while (isSprinting && visibility < maxVisibility)
        {
            visibility += noiseVisGain * Time.deltaTime;
            UpdateOverlay();
            yield return new WaitForSeconds(Time.deltaTime);
        }
        sprintMod = false;
    }

    void UpdateOverlay()
    {
        Color currentAlpha = visOverlay.color;
        currentAlpha.a = visibility / maxVisibility;
        visOverlay.color = currentAlpha;
    }
}
