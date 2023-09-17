using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneEnter : MonoBehaviour
{
    public GameObject thePlayer;
    public GameObject cutsceneCam;
    public GameObject fireObject;
    private LVLTWOTimer lvlTwoTimer;
    public GameObject beastEnemy;

    private Objectives objectives;

    private void Start()
    {
        lvlTwoTimer = GameObject.Find("TIMER").GetComponent<LVLTWOTimer>();
        objectives = GameObject.Find("TaskUI").GetComponent<Objectives>();
    }

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
            objectives.UpdateObjective("exit tunnel");
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
        lvlTwoTimer.StartTimer();
        beastEnemy.SetActive(true);

    }
}
