using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EvolveGames;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.ConstrainedExecution;

public class Visibility : MonoBehaviour
{
    public GameObject playerObject;
    private Animator iconAnimator;
    private Image visIcon;
    private Image visOverlay;
    private ItemChange itemScript;
    private StaminaController staminaScript;
    private DevTools devTools;

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
    [HideInInspector] public bool enemyChase = false;

    private bool godMode = false;
    private GameObject enemy;
    public float enemyMod = 2; // How much to multiply visibility gain by if the player is nearby the enemy.
    private int enemyNearby = 0; // C# is evil and true/false aren't treated as 1/0 like in other languages, so we're just gonna use an int in place of a bool.

    // Start is called before the first frame update
    void Start()
    {
        itemScript = playerObject.GetComponent<ItemChange>();
        staminaScript = playerObject.GetComponent<StaminaController>();
        devTools = playerObject.GetComponent<DevTools>();

        visOverlay = gameObject.transform.GetChild(0).GetComponent<Image>();
        visIcon = gameObject.transform.GetChild(1).GetComponent<Image>();

        iconAnimator = gameObject.transform.GetChild(1).GetComponent<Animator>();

        enemy = GameObject.Find("ENEMY");
    }

    // Update is called once per frame
    void Update()
    {
        godMode = devTools.godMode;
        if (!godMode && !enemyChase)
        {
            visIcon.color = Color.white;
            CheckEnemyDistance();
            VisibilityStatus();

            isSprinting = staminaScript.weAreSprinting;
            if (isSprinting && !sprintMod) { StartCoroutine(SprintVisibility()); } // If player is sprinting and sprint is not already being applied to visibility...

            currentItem = itemScript.ItemIdInt;
            if (currentItem == 3 || currentItem == 4) { playerVisible = true; } // Player gets increasing visibility if holding flashlight/lantern.
            else { playerVisible = false; }

            if (playerVisible && !visChange) { StartCoroutine(GainVisibility()); }
            if (!playerVisible && !visChange) { StartCoroutine(LoseVisibility()); }
        }

        else if (godMode) { visIcon.color = Color.green; }
    }

    void VisibilityStatus() // Effects that take place based on the level of the player's visibility.
    {
        // Handle if statements in descending order since otherwise it would stick to the first "if" statement (since it's the lowest and wouldn't check the other else-if statements).
        if (visibility >= maxVisibility)
        {
            // Enemy spawns!
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
            iconAnimator.SetBool("Animate", false);
        }

        else if (visibility < visSafe)
        {
            ChangeIcon("Visibility-Hidden");
            iconAnimator.SetBool("Animate", false);
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
            visibility += lightVisGain * Time.deltaTime + (lightVisGain * Time.deltaTime * enemyMod * enemyNearby); // Regular vis gain will be multiplied by the enemyMod if the enemy is nearby.
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
            visibility += noiseVisGain * Time.deltaTime + (noiseVisGain * Time.deltaTime * enemyMod * enemyNearby); // Regular vis gain will be multiplied by the enemyMod if the enemy is nearby.
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

    void CheckEnemyDistance()
    {
        float distance = Vector3.Distance(playerObject.transform.position, enemy.transform.position);
        if (distance < 50) { enemyNearby = 1; }
        else { enemyNearby = 0; }
    }
}
