using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public void OnPlayLevel1Click()
    {
        SceneManager.LoadScene("PlayTest");
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
        Debug.Log("It Works");
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
}
