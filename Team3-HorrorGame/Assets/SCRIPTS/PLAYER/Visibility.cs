using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using EvolveGames;
using UnityEngine.SceneManagement;

public class Visibility : MonoBehaviour
{
    public GameObject playerObject;
    public TMP_Text indicator;
    public GameObject visIcon;
    private ItemChange itemScript;
    private StaminaController staminaScript;

    public float visibility = 0;

    [Header("Visibility Parameters")]
    [Range(0, 25)] public float visGain = 2.5f; // How much to add to visibility on each interval.
    [Range(0, 25)] public float visLose = 5.0f;
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
            SceneManager.LoadScene("GAMEOVER");
        }

        else if (visibility >= visDanger)
        {
            ChangeIcon("Visibility-Danger");
        }

        else if (visibility >= visCaution)
        {
            ChangeIcon("Visibility-Caution");
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
        Sprite newIcon = Resources.Load<Sprite>(name);
        visIcon.GetComponent<Image>().sprite = newIcon;
    }

    private IEnumerator GainVisibility()
    {
        visChange = true;
        while (playerVisible && visibility < maxVisibility)
        {
            visibility += visGain * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        visChange = false;
    }

    private IEnumerator LoseVisibility()
    {
        visChange = true;
        while (!playerVisible && !isSprinting && visibility > 0)
        {
            visibility -= visLose * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        visChange = false;
    }

    private IEnumerator SprintVisibility()
    {
        sprintMod = true;
        while (isSprinting && visibility < maxVisibility)
        {
            visibility += visGain * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        sprintMod = false;
    }
}
