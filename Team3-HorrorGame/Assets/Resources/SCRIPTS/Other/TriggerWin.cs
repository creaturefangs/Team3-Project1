using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerWin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene("CREDITS");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
