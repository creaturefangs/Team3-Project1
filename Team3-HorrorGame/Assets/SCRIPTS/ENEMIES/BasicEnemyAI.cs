using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicEnemyAI : MonoBehaviour
{

    public GameObject playerPrefab;
    public AudioSource goreSFX;
    public GameObject playerDmgUI;

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
    }

    private void Patrol()
    {
        float step = patrolSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, step);

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

    private void Chase()
    {
        float step = chaseSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.position, step);

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

    }
}
