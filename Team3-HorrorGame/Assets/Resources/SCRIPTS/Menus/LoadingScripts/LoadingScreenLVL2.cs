using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LoadingScreenLVL2 : MonoBehaviour
{
    public Slider loadingBar;
    public TextMeshProUGUI progressText;

    private void Start()
    {
        StartCoroutine(LoadAsyncScene());
    }

    IEnumerator LoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("LevelTwo");
        asyncLoad.allowSceneActivation = false;
        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // Loading progress is from 0 to 0.9
            loadingBar.value = progress;
            progressText.text = "Loading: " + (progress * 100) + "%";
            if (asyncLoad.progress >= 0.9f)
            {
                loadingBar.value = 1f;
                progressText.text = "Press any key to continue";
                if (Input.anyKeyDown)
                {
                    asyncLoad.allowSceneActivation = true; // Activate the next scene
                }
            }
            yield return null;
        }
    }
}
