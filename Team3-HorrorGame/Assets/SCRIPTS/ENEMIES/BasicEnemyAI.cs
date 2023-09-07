using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class BasicEnemyAI : MonoBehaviour
{
    public enum AIState { Idle, Patrol, Stalk, Chase }
    public AIState currentState = AIState.Idle;

    public GameObject player;
    public AudioSource goreSFX;
    public GameObject playerDmgUI;

    private GameObject blinkOverlay;
    public bool staring = false;
    private bool stalking = false;
    public bool contest = false;
    private Vector3 idlePos;

    private Visibility visScript;
    public float visibility = 0;
    private float maxVis;

    public Transform[] waypoints;
    public float detectionRange = 10f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;

    private Transform currentWaypoint;
    private int waypointIndex = 0;
    private Animator animator;

    private void Start()
    {
        player = GameObject.Find("PlayerController");
        playerDmgUI = GameObject.Find("PlayerDMG");
        visScript = GameObject.Find("VisibilityUI").GetComponent<Visibility>();
        maxVis = visScript.maxVisibility;

        //currentWaypoint = waypoints[waypointIndex];
        animator = GetComponent<Animator>();

        blinkOverlay = GameObject.Find("BlinkOverlay");
        idlePos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
    }

    private void Update()
    {
        FollowPlayer();
        visibility = visScript.visibility;
        SetState();
        switch (currentState)
        {
            case AIState.Idle:
                Idle();
                break;
            case AIState.Patrol:
                Patrol();
                break;
            case AIState.Stalk:
                if (!stalking) { Stalk(); }
                break;
            case AIState.Chase:
                Chase();
                break;
        }
        CheckIfStaring();
        if (currentState == AIState.Stalk && staring && !contest) { StartCoroutine(StaringContest()); }
    }

    void SetState()
    {
        if (visibility >= maxVis) { Debug.Log("holy freaking bingle whaaat"); } // currentState = AIState.Chase;
        else if (visibility >= visScript.visCaution) { currentState = AIState.Stalk; }
        else if (visibility < visScript.visCaution && !stalking) { currentState = AIState.Idle; } // Originally AIState.Patrol
    }


    // ENEMY STATES //
    void Idle()
    {
        
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
        //if (Vector3.Distance(transform.position, player.transform.position) < detectionRange)
        //{
        //    currentState = AIState.Chase;
        //}
    }

    private void Stalk() // At around medium visibility, enemy will appear at a randomly selected point in a radius around the player.
    {
        stalking = true;
        Vector3 position = new Vector3(player.transform.position.x + Random.Range(-40, 41), player.transform.position.y, player.transform.position.z + Random.Range(-40, 41)); // Random.rotation.z, (Random.insideUnitSphere * 30).x
        transform.position = position;
        GetTerrainHeight();
        while (Vector3.Distance(transform.position, player.transform.position) < 20 || CheckIfVisible()) // While enemy moves to a point that's visible to the player, go to a new point until not seen. Orig CheckIfVisible().
        {
            Debug.Log("Enemy respawned: too close to player.");
            Vector3 newPos = new Vector3(player.transform.position.x + Random.Range(-40, 41), player.transform.position.y, player.transform.position.z + Random.Range(-40, 41));
            transform.position = newPos;
            GetTerrainHeight();
        }
    }

    private void Chase()
    {
        float step = chaseSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step); // Enemy moves towards player from current position at given chase speed.

        // Check if player is out of detection range
        if (Vector3.Distance(transform.position, player.transform.position) > detectionRange)
        {
            currentState = AIState.Idle; // Originally AIState.Patrol
        }
    }


    // MISCELLANEOUS //
    void OnTriggerEnter(Collider other)
    {

        goreSFX.Play();
        playerDmgUI.SetActive(true);

        // plays the damage sound effect and displays player's damage
    }

    IEnumerator StaringContest() // If player looks at enemy while enemy is stalking them and player continues to stare, enemy disappears.
    {
        contest = true;
        float stareLength = Random.Range(2.5f, 5.1f);
        yield return new WaitForSeconds(stareLength);

        blinkOverlay.SetActive(true);
        while (blinkOverlay.GetComponent<Image>().color.a < 1)
        {
            yield return new WaitForSeconds(0.01f);
            Color currentAlpha = blinkOverlay.GetComponent<Image>().color;
            currentAlpha.a += 0.25f;
            blinkOverlay.GetComponent<Image>().color = currentAlpha;
        }

        yield return new WaitForSeconds(0.1f);
        stalking = false;
        currentState = AIState.Idle; // Enemy leaves the player alone and goes back to patrolling. Orig AIState.Patrol.
        visScript.visibility = 0;
        gameObject.transform.position = idlePos;

        while (blinkOverlay.GetComponent<Image>().color.a > 0)
        {
            yield return new WaitForSeconds(0.01f);
            Color currentAlpha = blinkOverlay.GetComponent<Image>().color;
            currentAlpha.a -= 0.25f;
            blinkOverlay.GetComponent<Image>().color = currentAlpha;
        }
        blinkOverlay.SetActive(false);
        contest = false;
    }

    bool CheckIfVisible() // Check if enemy is visible anywhere on screen.
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        if (viewPos.x < 1 && viewPos.x > 0 && viewPos.y < 1 && viewPos.y > 0 && viewPos.z > 0) { return true; }
        else { return false; }
    }

    void CheckIfStaring() // Checks if the enemy is currently being DIRECTLY seen by the player.
    {
        LayerMask mask = ~LayerMask.GetMask("UI");
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 50, mask))
        {
            if (hit.collider.gameObject == gameObject) { staring = true; }
            else { staring = false; }
        }
        else { staring = false; }
    }

    void FollowPlayer() // Enemy rotates to look at player.
    {
        Quaternion rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, rotation.eulerAngles.y, transform.eulerAngles.z);
    }

    void GetTerrainHeight() // Makes enemy placed just above the terrain.
    {
        float yPos;
        RaycastHit hit;
        if (Physics.Raycast(transform.localPosition, transform.TransformDirection(Vector3.down), out hit))
        {
            Debug.Log($"DOWN | Terrain Pos: {hit.transform.position.y} Distance: {hit.distance} Original Enemy Pos: {transform.position.y}");
            if (hit.collider.gameObject.name == "Terrain") { yPos = hit.distance; }
            else { yPos = transform.position.y; }
        }
        else { yPos = transform.position.y; }
        transform.position = new Vector3(transform.position.x, transform.position.y + yPos - 1, transform.position.z);
    }
}
