using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public void OnPlayLevel1Click()
    {
        SceneManager.LoadScene("LevelOne");
    }

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
}
