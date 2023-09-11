using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskListManager : MonoBehaviour
{
    public GameObject taskUI;
    public GameObject playerUI;
    public GameObject pauseMenuUI;
    public bool taskVisible = false;
    public PauseManager pauseManager;
    public AudioSource noteSFX;

    void Start()
    {
       pauseManager = GetComponent<PauseManager>();

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (pauseManager.GameIsPaused == true)
            {
                taskVisible = false;
            }
            else
            {
                if (taskVisible)
                {
                    Invisible();
                }
                else
                {
                    Visible();
                }
            }

        }
    }

    public void Invisible()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        taskUI.SetActive(false);
        playerUI.SetActive(true);
        taskVisible = false;
        noteSFX.Play();
    }

    public void Visible()
    {
        taskUI.SetActive(true);
        playerUI.SetActive(true);
        taskVisible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        noteSFX.Play();
    }

}


