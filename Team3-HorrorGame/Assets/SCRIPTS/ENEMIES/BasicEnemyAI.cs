using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class BasicEnemyAI : MonoBehaviour
{

    public GameObject playerPrefab;
    public AudioSource goreSFX;
    public GameObject playerDmgUI;

    private GameObject blinkOverlay;
    private bool staring = false;

    public Transform[] waypoints;
    public Transform player;
    public float detectionRange = 10f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;

    private Transform currentWaypoint;
    private int waypointIndex = 0;
    private Animator animator;

    private enum AIState { Patrol, Chase }
    private AIState currentState = AIState.Patrol;

    private void Start()
    {
        currentWaypoint = waypoints[waypointIndex];
        animator = GetComponent<Animator>();

        blinkOverlay = GameObject.Find("Overlay");
    }

    private void Update()
    {
        switch (currentState)
        {
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Chase:
                Chase();
                break;
        }
        CheckIfSeen();
        if (currentState == AIState.Patrol && staring) { StartCoroutine(StaringContest()); }
    }

    private void Patrol()
    {
        float step = patrolSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, step); // Enemy moves from current position to the next point of its patrol.

        if (Vector3.Distance(transform.position, currentWaypoint.position) < 0.1f)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
            currentWaypoint = waypoints[waypointIndex];
        }

        // Check if player is within detection range
        if (Vector3.Distance(transform.position, player.position) < detectionRange)
        {
            currentState = AIState.Chase;
        }
    }

    private void Stalk() // At around medium visibility, enemy will appear at a randomly selected point in a radius around the player.
    {

    }

    private void Chase()
    {
        float step = chaseSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.position, step); // Enemy moves towards player from current position at given chase speed.

        // Check if player is out of detection range
        if (Vector3.Distance(transform.position, player.position) > detectionRange)
        {
            currentState = AIState.Patrol;
        }
    }

    void OnTriggerEnter(Collider other)
    {

        goreSFX.Play();
        playerDmgUI.SetActive(true);

        // plays the damage sound effect and displays player's damage
    }

    IEnumerator StaringContest() // If player looks at enemy while enemy is stalking them and player continues to stare, enemy disappears.
    {
        float stareLength = 5.0f;
        yield return new WaitForSeconds(stareLength);
        blinkOverlay.SetActive(true);
        UpdateOverlay(0.5f);
        yield return new WaitForSeconds(0.1f);
        UpdateOverlay(0.75f);
        yield return new WaitForSeconds(0.1f);
        UpdateOverlay(1f);
        yield return new WaitForSeconds(0.1f);
        UpdateOverlay(0.75f);
        yield return new WaitForSeconds(0.1f);
        UpdateOverlay(0.5f);
        blinkOverlay.SetActive(false);
    }

    void UpdateOverlay(float alpha)
    {
        Color currentAlpha = blinkOverlay.GetComponent<Image>().color;
        currentAlpha.a = alpha;
        blinkOverlay.GetComponent<Image>().color = currentAlpha;
    }

    void CheckIfSeen() // Checks if the enemy is currently being DIRECTLY seen by the player.
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 7))
        {
            if (hit.collider.gameObject == gameObject) { staring = true; }
            else { staring = false; }
        }
    }
}
