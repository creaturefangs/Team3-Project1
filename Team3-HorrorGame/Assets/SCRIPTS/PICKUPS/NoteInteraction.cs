using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NoteInteraction : MonoBehaviour
{
    public NoteContent noteContent;
    public TMP_Text noteDisplayText;
    public GameObject noteDisplayPanel;
    private bool isInRange = false;
    public AudioSource noteSFX;

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!noteDisplayPanel.activeSelf)
            {
                PickUpNote();
            }
            else
            {
                ExitNote();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }

    public void PickUpNote()
    {
        // Display note content on the screen
        DisplayNoteContent(noteContent);

        // Deactivate the note object
        gameObject.SetActive(false);

        // Set the cursor to visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // play sound effect
        noteSFX.Play();

       
    }

    public void ExitNote()
    {
        // Hide the note display panel
        noteDisplayPanel.SetActive(false);

        // hide the cursor during gameplay
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Activate the note object
        gameObject.SetActive(true);

        // play sound effect
        noteSFX.Play();
    }

    private void DisplayNoteContent(NoteContent content)
    {
        // Clear previous content
        noteDisplayText.text = "";

        // Display each paragraph of the note's content
        foreach (string paragraph in content.paragraphs)
        {
            noteDisplayText.text += paragraph + "\n\n";
        }
    }
}
