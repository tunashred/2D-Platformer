using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private int _maxHealth = 100;

    [SerializeField] private float _timeSinceHit = 0;
    [SerializeField] public float invincibilityTime = 0.25f;

    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    [SerializeField] private bool _isAlive = true;

    [SerializeField] private bool _isInvincible = false;

    public bool IsAlive
    {
        get { return _isAlive; }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set" + value);
        }
    }

    [SerializeField] private int health = 100;

    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            if (health < 0)
            {
                IsAlive = false;
            }
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if (_isInvincible)
        {
            if (_timeSinceHit > invincibilityTime)
            {
                _isInvincible = false;
                _timeSinceHit = 0;
            }

            _timeSinceHit += Time.deltaTime;
        }

        Hit(10);
    }

    public void Hit(int damage)
    {
        if (IsAlive && !_isInvincible)
        {
            Health -= damage;
            _isInvincible = true;
        }
    }
}