using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{

    public GameObject inventoryUI;
    public GameObject playerUI;
    public GameObject pauseMenuUI;
    public bool inventoryVisible = false;
    public PauseManager pauseManager;
    public AudioSource inventoryZipper;

    void Start()
    {
        pauseManager = GetComponent<PauseManager>();

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (pauseManager.GameIsPaused == true)
            {
                inventoryVisible = false;

            }
            else
            { 
                if (inventoryVisible)
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
        inventoryUI.SetActive(false);
        playerUI.SetActive(true);
        inventoryVisible = false;
        inventoryZipper.Play();
    }

    public void Visible()
    {
        
        inventoryUI.SetActive(true);
        playerUI.SetActive(false);
        inventoryVisible = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        inventoryZipper.Play();
    }

}
