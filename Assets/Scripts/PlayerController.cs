using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3f;
    public float jumpImpulse = 10f;

    private Vector2 _moveInput;
    private TouchingDirections _touchingDirections;
    private Damageable damageable;
    private StateScreens stateScreens;
    private bool isPaused = false;

    public UnityEvent OnPlayerDeath;
    public UnityEvent OnPlayerWon;

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !_touchingDirections.IsOnWall)
                {
                    if (_touchingDirections.IsGrounded)
                    {
                        return IsRunning ? runSpeed : walkSpeed;
                    }

                    return airWalkSpeed;
                }

                return 0;
            }

            return 0;
        }
    }

    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _isRunning = false;

    public bool IsMoving
    {
        get { return _isMoving; }
        set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    public bool IsRunning
    {
        get { return _isRunning; }
        set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    private Rigidbody2D rb;
    private Animator animator;

    public bool _isFacingRight = true;

    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            _isFacingRight = value;
        }
    }

    public bool CanMove
    {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }

    public bool IsAlive
    {
        get { return animator.GetBool(AnimationStrings.isAlive); }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    private void FixedUpdate()
    {
        if (!isPaused)
        {
            if (!damageable.LockVelocity)
            {
                rb.velocity = new Vector2(_moveInput.x * CurrentMoveSpeed, rb.velocity.y);
            }

            animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
        }
    }
    
    public void PausePlayerActions()
    {
        isPaused = true;
    }

    public void ResumePlayerActions()
    {
        isPaused = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            _moveInput = context.ReadValue<Vector2>();

            if (IsAlive)
            {
                IsMoving = _moveInput != Vector2.zero;
                SetFacingDirection(_moveInput);
            }
            else
            {
                IsMoving = false;
            }
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            if (context.started)
            {
                IsRunning = true;
            }
            else if (context.canceled)
            {
                IsRunning = false;
            }
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!isPaused)
        {
            if (context.started && _touchingDirections.IsGrounded && CanMove)
            {
                animator.SetTrigger(AnimationStrings.jump);
                rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!isPaused && context.started)
        {
            animator.SetTrigger(AnimationStrings.attack);
        }
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (!isPaused && context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttack);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        if (!isPaused)
        {
            rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);

            if (!IsAlive)
            {
                if (OnPlayerDeath != null)
                {
                    OnPlayerDeath.Invoke();
                }
            }
        }
    }
}