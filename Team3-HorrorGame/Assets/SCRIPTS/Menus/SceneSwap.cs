using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour
{
    private void OnGUI()
    {
        int xCenter = (Screen.width / 2);
        int yCenter = (Screen.height / 2);
        int width = 1;
        int height = 1;

        GUIStyle fontSize = new GUIStyle(GUI.skin.GetStyle("button"));
        fontSize.fontSize = 1;

        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == "scene1")
        {
            // Show a button to allow scene2 to be switched to.
            if (GUI.Button(new Rect(xCenter - width / 2, yCenter - height / 2, width, height), "Load second scene", fontSize))
            {
                SceneManager.LoadScene("scene2");
            }
        }
        else
        {
            // Show a button to allow scene1 to be returned to.
            if (GUI.Button(new Rect(xCenter - width / 2, yCenter - height / 2, width, height), "Return to first scene", fontSize))
            {
                SceneManager.LoadScene("scene1");
            }
        }
    }
}
