using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneEnter : MonoBehaviour
{
    public GameObject thePlayer;
    public GameObject cutsceneCam;
    public GameObject fireObject;
    private LVLTWOTimer lvltwoTimer;
    public GameObject beastEnemy;

    void OnTriggerEnter(Collider other)
    {
        this.gameObject.GetComponent<BoxCollider>().enabled = false;
        cutsceneCam.SetActive(true);
        thePlayer.SetActive(false);
        beastEnemy.SetActive(false);

        if (gameObject.CompareTag ("CutsceneFire"))
        {
            StartCoroutine(FinishCutFire());
        }


        if (gameObject.CompareTag ("CutsceneExit"))
        {
            StartCoroutine(FinishCut());
        }

    }

    IEnumerator FinishCut()
    {
        
        yield return new WaitForSeconds(8);
        thePlayer.SetActive(true);
        cutsceneCam.SetActive(false);
        beastEnemy.SetActive(true);

    }

    IEnumerator FinishCutFire()
    {

        yield return new WaitForSeconds(18);
        thePlayer.SetActive(true);
        cutsceneCam.SetActive(false);
        fireObject.SetActive(true);
        lvltwoTimer.StartTimer();
        beastEnemy.SetActive(true);

    }
}
