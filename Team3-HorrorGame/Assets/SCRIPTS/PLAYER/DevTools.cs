using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevTools : MonoBehaviour
{
    public bool devToolsEnabled = true;
    public bool godMode = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (devToolsEnabled)
        {
            // Makes the player have infinite visibility and sprint when toggled.
            if (Input.GetKeyDown(KeyCode.Keypad0)) { godMode = !godMode; }

            if (Input.GetKeyDown(KeyCode.Keypad1)) { SceneManager.LoadScene("LEVELONE"); }
            if (Input.GetKeyDown(KeyCode.Keypad2)) { SceneManager.LoadScene("LEVELTWO"); }
            if (Input.GetKeyDown(KeyCode.Keypad3)) { SceneManager.LoadScene("LEVELTHREE"); }
        }
    }
}
