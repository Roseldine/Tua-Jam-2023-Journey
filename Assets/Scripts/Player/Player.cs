using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StardropTools;
using StardropTools.ScriptableValue;

public class Player : AnimatedHealthyObject, IManager
{
    [NaughtyAttributes.Expandable] [SerializeField] PlayerSettingsSO settings;
    [SerializeField] PlayerState            state;
    [SerializeField] CharacterController    controller;
    [SerializeField] BoxColliderDetector    groundChecker;
    [SerializeField] BoxColliderDetector    collisionChecker;
    [SerializeField] BoxColliderDetector    collectableChecker;
    [SerializeField] Transform              graphic; 
    [SerializeField] bool                   canMove;

    [Header("Scriptable Values")]
    [SerializeField] float speedMultiplier = 1;
    [SerializeField] float boostGauge = 0;

    [Header("Scriptable Values")]
    [SerializeField] ScriptableFloat distanceValue;
    [SerializeField] ScriptableFloat boostValue;


    float horizontal, verticalSpeed, forwardSpeed;
    Vector3 moveDirection, lookDirection;

    public bool GetCanUpdate() => true;

    public void InitializeManager()
    {
        GameManager.OnMainMenu.AddListener(ResetPlayer);
        GameManager.OnPlayStart.AddListener(StartRunning);
        GameManager.OnPlayEnd.AddListener(()    => canMove = false);

        collisionChecker.OnColliderEnter.AddListener(Collision);
        groundChecker.OnContactStart.AddListener(GroundTouched);
        collectableChecker.OnColliderEnter.AddListener(CollectableFound);

        ResetPlayer();
    }

    public void LateInitializeManager() { }

    public void UpdatePlayer()
    {
        if (canMove == false)
            return;

        HandleInput();
        HandleMovement();
        collisionChecker.SearchForColliders();
        collectableChecker.SearchForColliders();
    }

    void ResetPlayer()
    {
        Revive();
        ChangeState(PlayerState.Idle);

        verticalSpeed = 0;
        selfTransform.position = Vector3.zero;
        forwardSpeed = settings.speedForwards * speedMultiplier;
    }

    void StartRunning()
    {
        canMove = true;
        ChangeState(PlayerState.Running);
    }

    

    // Movement
    void HandleMovement()
    {
        moveDirection.x = horizontal * settings.speedHorizontal;
        moveDirection.y = verticalSpeed;
        moveDirection.z = forwardSpeed;

        controller.Move(moveDirection * Time.deltaTime);
        distanceValue.SetFloat(UtilsMath.RoundDecimals(PosZ, settings.distanceDecimals));
    }

    void HandleInput()
    {
        groundChecker.SearchForColliders();
        horizontal  = Input.GetAxisRaw("Horizontal");

        // Tilt
        if (Mathf.Abs(horizontal) > 0)
            lookDirection = (Vector3.forward * settings.speedForwards) + (Vector3.right * horizontal * settings.lookMultiplier);
        else
            lookDirection = Vector3.forward;

        UtilsVector.SmoothLookAt(graphic, lookDirection, settings.lookSpeed);

        // Movement
        if (groundChecker.HasContact == true)
        {
            if (Input.GetButtonDown("Jump"))
            {
                ChangeState(PlayerState.JumpStart);
                verticalSpeed = Mathf.Sqrt(2 * settings.jumpHeight * -settings.gravity);
            }

            else
            {
                if (state == PlayerState.Running)
                    verticalSpeed = 0.0f;
            }
        }

        else
            verticalSpeed += settings.gravity * Time.deltaTime;
    }

    void GroundTouched()
    {
        ChangeState(PlayerState.Running);
    }



    // Collision
    void Collision(Collider collidedObject) => ChangeState(PlayerState.Collided);

    void HandleCollision()
    {
        PlayCollision();
        forwardSpeed = -settings.collisionKnockbackSpeed;
        verticalSpeed = Mathf.Sqrt(2 * settings.collisionKnockbackHeight * -settings.gravity);
        GameManager.OnPlayerCollision?.Invoke();

        ApplyDamage(1);
        if (healthComponent.IsDead)
            GameManager.OnPlayEndRequest?.Invoke();
    }



    // Collectables
    void CollectableFound(Collider collectableCollider)
    {
        Collectable collectable = collectableCollider.GetComponent<Collectable>();
        collectable.Collect();
        IncreaseSpeed(collectable.Value);

    }

    void IncreaseSpeed(float amount)
    {
        speedMultiplier = Mathf.Clamp(speedMultiplier + amount, settings.minSpeedMultiplier, settings.maxSpeedMultiplier);
        forwardSpeed = settings.speedForwards * speedMultiplier;

        boostGauge = Mathf.Clamp(boostGauge + amount, 0, settings.maxBoost);
        boostValue.AddAmount(amount);
    }


    void ChangeState(PlayerState targetState)
    {
        if (state == targetState)
            return;

        state = targetState;

        switch (targetState)
        {
            case PlayerState.Idle:
                PlayRandomIdle();
                break;

            case PlayerState.Running:
                forwardSpeed = settings.speedForwards * speedMultiplier;
                PlayRun();
                break;

            case PlayerState.JumpStart:
                PlayJumpStart();
                break;

            case PlayerState.JumpEnd:
                PlayJumpEnd();
                break;

            case PlayerState.Collided:
                HandleCollision();
                break;
        }
    }


    // Animation

    void PlayRandomIdle()   => CrossFadeAnimation(Random.Range(0, 3));
    void PlayRun()          => CrossFadeAnimation(3);
    void PlayCollision()    => CrossFadeAnimation(4);
    void PlayJumpStart()    => CrossFadeAnimation(5);
    void PlayJumpEnd()      => CrossFadeAnimation(6);
}
