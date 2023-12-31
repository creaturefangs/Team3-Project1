using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public void OnQuitButtonClick()
    {
        Application.Quit();
    }

    public void OnHelpButtonClick()
    {
        SceneManager.LoadScene("HelpScene");
    }

    public void OnCreditButtonClick()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickPlaytest()
    {
        SceneManager.LoadScene("PLAYTEST");
    }

    public void OnClickCutsceneOne()
    {
        SceneManager.LoadScene("INTROCUTSCENE");
    }

    public void OnClickLoadScreen()
    {
        SceneManager.LoadScene("LoadingSreen");
    }
}
