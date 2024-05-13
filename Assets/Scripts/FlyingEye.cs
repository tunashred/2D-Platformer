using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEye : MonoBehaviour
{
    public float flightSpeed = 2f;
    public float waypointReachedDistance = 0.1f;
    public DetectionZone biteDetectionZone;
    public List<Transform> waypoints;

    private Animator animator;
    private Rigidbody2D rb;
    private Damageable damageable;
    private Transform nextWaypoint;

    public bool hasTarget = false;
    private int waypointNum = 0;

    public bool CanMove
    {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }

    public bool HasTarget
    {
        get { return hasTarget; }
        set
        {
            hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    private void UpdateDirection()
    {
        Vector3 locScale = transform.localScale;
        if (transform.localScale.x > 0)
        {
            if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
        else
        {
            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(-1 * locScale.x, locScale.y, locScale.z);
            }
        }
    }
    
    private void Flight()
    {
        // when you normalize a vector, it's just a direction without any speed
        Vector2 directionToWaypoint = (nextWaypoint.position - transform.position).normalized;

        // this is the magnitude between the two points
        float distance = Vector2.Distance(nextWaypoint.position, transform.position);

        rb.velocity = directionToWaypoint * flightSpeed;
        UpdateDirection();
        
        if (distance <= waypointReachedDistance)
        {
            waypointNum++;
            if (waypointNum >= waypoints.Count)
            {
                waypointNum = 0;
            }

            nextWaypoint = waypoints[waypointNum];
        }
    }

    private void Start()
    {
        nextWaypoint = waypoints[waypointNum];
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate()
    {
        if (damageable.IsAlive)
        {
            if (CanMove)
            {
                Flight();
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            rb.gravityScale = 2f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HasTarget = biteDetectionZone.detectedColliders.Count > 0;
    }
}