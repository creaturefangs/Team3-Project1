using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemyAI : MonoBehaviour
{

    public GameObject playerPrefab;
    public AudioSource goreSFX;
    public GameObject playerDmgUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        goreSFX.Play();
        playerDmgUI.SetActive(true);

    }
}
