using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class BasicEnemyAI : MonoBehaviour
{
    public enum AIState { Idle, Patrol, Stalk, Chase }
    public AIState currentState = AIState.Idle;

    private GameObject player;
    private PlayerHealth playerHealth;
    private DevTools devTools;

    public AudioSource goreSFX;
    public AudioSource stalkCue;
    public GameObject playerDmgUI;
    private AudioSource chaseMusic;

    private GameObject blinkOverlay;
    [HideInInspector] public bool chase = false;
    private bool staring = false;
    private bool stalking = false;
    private bool contest = false;
    private bool disappearing = false;
    private bool damageRange = false;
    private Vector3 idlePos;

    private Visibility visScript;
    public float visibility = 0;
    private float maxVis;

    public Transform[] waypoints;
    public float detectionRange = 30f;

    public float patrolSpeed = 2f;
    public float minChaseSpeed = 10f;
    private float chaseSpeed;
    public float maxChaseSpeed = 15f;

    private Transform currentWaypoint;
    private int waypointIndex = 0;
    private Animator animator;
   

    private void Start()
    {
        player = GameObject.Find("PlayerController");
        playerHealth = player.GetComponent<PlayerHealth>();
        devTools = player.GetComponent<DevTools>();
        visScript = GameObject.Find("VisibilityUI").GetComponent<Visibility>();
        chaseMusic = GameObject.Find("MusicPlayer").GetComponent<AudioSource>();
        maxVis = visScript.maxVisibility;

        //currentWaypoint = waypoints[waypointIndex];
        animator = GetComponent<Animator>();
        

        blinkOverlay = GameObject.Find("BlinkOverlay");
        idlePos = gameObject.transform.position;
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
                if (!stalking && !devTools.godMode) { Stalk(); }
                break;
            case AIState.Chase:
                if (!chase && !devTools.godMode) { StartCoroutine(Chase()); }
                break;
        }
        CheckIfStaring();
        if (currentState == AIState.Stalk && staring && !contest) { StartCoroutine(StaringContest()); }

        if (Vector3.Distance(transform.position, player.transform.position) <= 5 && !chase) { currentState = AIState.Chase; visScript.visibility = maxVis; }
    }

    void SetState()
    {
        if (visibility >= maxVis) { currentState = AIState.Chase; }
        else if (visibility >= visScript.visCaution && !stalking && !chase) { currentState = AIState.Stalk; }
        else if (visibility < visScript.visCaution && !stalking && !chase) { currentState = AIState.Idle; } // Originally AIState.Patrol
    }


    // ENEMY STATES //
    void Idle()
    {
        gameObject.transform.position = idlePos;
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
        stalkCue.Play();
        SpawnEnemy(80f);
        StartCoroutine(IgnoreEnemy());
    }

    private IEnumerator Chase()
    {
        chase = true;
        if (chaseMusic.isPlaying) { StartCoroutine(FadeMusic(chaseMusic, 0.1f)); }
        animator.SetBool("IsChasing", true);
        chaseMusic.Play();

        if (Vector3.Distance(transform.position, player.transform.position) > 5) { SpawnEnemy(detectionRange / 2); }
        yield return new WaitForSeconds(5f); // Headstart! Do SFX here to indicate chase start?
        chaseSpeed = minChaseSpeed;
        float start = Time.time;

        while (Vector3.Distance(transform.position, player.transform.position) < detectionRange || TimeSince(start) < 15.0f)
        {
            // Debug.Log("Distance: " + Vector3.Distance(transform.position, player.transform.position));
            if (chaseSpeed < maxChaseSpeed) { chaseSpeed += 0.01f; }
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, chaseSpeed * Time.deltaTime); // Enemy moves towards player from current position at given chase speed.
            yield return new WaitForSeconds(Time.deltaTime);
        }

        animator.SetBool("IsChasing", false);

        // When player gets out of range...
        Debug.Log("Chase ended.");
        currentState = AIState.Idle; // Originally AIState.Patrol
        visScript.visibility = 0;
        visScript.UpdateOverlay();

        chase = false;
        StartCoroutine(FadeMusic(chaseMusic, 0.5f));
    }


    // MISCELLANEOUS //
    void OnTriggerEnter(Collider other)
    {
        chaseSpeed = minChaseSpeed; // Enemy slows down when attacking to give player window of opportunity to run.
        if (!devTools.godMode)
        {
            damageRange = true;
            StartCoroutine(DamagePlayer());
        }
        // plays the damage sound effect and displays player's damage
    }

    void OnTriggerExit(Collider other) { damageRange = false; }

    private IEnumerator DamagePlayer()
    {
        float attackCooldown = 3.0f;
        while (damageRange)
        {
            goreSFX.Play();
            playerHealth.TakeDamage(20);
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    private IEnumerator StaringContest() // If player looks at enemy while enemy is stalking them and player continues to stare, enemy disappears.
    {
        contest = true;

        float stareLength = Random.Range(2.5f, 5.1f);
        float start = Time.time;

        while (staring)
        {
            if (TimeSince(start) >= stareLength && !disappearing) { StartCoroutine(Disappear()); }
            yield return new WaitForSeconds(0.1f);
        }
        if (TimeSince(start) < stareLength) { currentState = AIState.Chase; visScript.visibility = maxVis; Debug.Log("Player looked away."); } // If player didn't stare for long enough, chase player.
        stalking = false;
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
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 100, mask))
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
        Vector3 newPos = transform.position;
        Renderer renderer = GetComponent<Renderer>();
        newPos.y = Terrain.activeTerrain.SampleHeight(transform.position) + (renderer.bounds.size.y / 2) - 1; // Accounts for the enemy's height AND terrain height to place enemy vertically.
        transform.position = newPos;
    }

    void SpawnEnemy(float range)
    {
        Vector3 position = new Vector3(player.transform.position.x + Random.Range(-range / 2, (range / 2) + 1), player.transform.position.y, player.transform.position.z + Random.Range(-range / 2, (range / 2) + 1));
        transform.position = position;
        GetTerrainHeight();
        while (Vector3.Distance(transform.position, player.transform.position) < (range / 2) || CheckIfVisible()) // While enemy moves to a point that's visible or too close to the player, go to a new point until not seen / far enough away.
        {
            Vector3 newPos = new Vector3(player.transform.position.x + Random.Range(-range, range + 1), player.transform.position.y, player.transform.position.z + Random.Range(-range, range + 1));
            transform.position = newPos;
            GetTerrainHeight();
        }
    }

    private IEnumerator IgnoreEnemy()
    {
        float timer = 10f;
        yield return new WaitForSeconds(timer);
        if (stalking && !staring)
        {
            if (CheckIfVisible() && !disappearing) { StartCoroutine(Disappear()); }
            else { currentState = AIState.Idle; }
            stalking = false;
        }
        //while (timer > 0 && !staring) { timer -= Time.deltaTime; yield return new WaitForSeconds(Time.deltaTime); }
        //if (timer <= 0 && !staring) { currentState = AIState.Chase; stalking = false; } // Enemy chases player if not stared at in time.
    }

    private IEnumerator Disappear()
    {
        disappearing = true;
        blinkOverlay.SetActive(true);
        while (blinkOverlay.GetComponent<Image>().color.a < 1)
        {
            yield return new WaitForSeconds(0.01f);
            Color currentAlpha = blinkOverlay.GetComponent<Image>().color;
            currentAlpha.a += 0.25f;
            blinkOverlay.GetComponent<Image>().color = currentAlpha;
        }

        yield return new WaitForSeconds(0.1f);
        currentState = AIState.Idle; // Enemy leaves the player alone and goes back to patrolling. Orig AIState.Patrol.
        visScript.visibility = 0;
        visScript.UpdateOverlay();

        while (blinkOverlay.GetComponent<Image>().color.a > 0)
        {
            yield return new WaitForSeconds(0.01f);
            Color currentAlpha = blinkOverlay.GetComponent<Image>().color;
            currentAlpha.a -= 0.25f;
            blinkOverlay.GetComponent<Image>().color = currentAlpha;
        }
        blinkOverlay.SetActive(false);
        disappearing = false;
    }

    private IEnumerator FadeMusic(AudioSource music, float rate)
    {
        while (music.volume > 0)
        {
            music.volume -= 0.1f;
            yield return new WaitForSeconds(rate);
        }
        music.Stop();
        music.volume = 1f;
    }

    float TimeSince(float time)
    {
        float difference = Time.time - time;
        return difference;
    }
}
