using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    private string sceneName;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        if (SceneManager.GetActiveScene().name == "INTROCUTSCENE")
        {
            
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
