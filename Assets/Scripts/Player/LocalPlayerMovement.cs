using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalPlayerMovement : MonoBehaviour
{
    [SerializeField]
    private LocalPlayer player;

    private InputActionMap survivorInputMap;
    private InputAction playerMovementAction;
    private InputAction playerMousePosAction;
    private InputAction playerLookAction;
    private InputAction playerDashAction;

    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private float dashSpeedModifier = 2f;
    [SerializeField]
    private float dashDuration = 0.3f;
    [SerializeField]
    private float dashCooldown = 0.5f;
    private bool isDashing = false;
    private bool canDash = true;

    private void Awake()
    {
        survivorInputMap = InputManager.instance.globalInputMap;

        playerMovementAction = survivorInputMap.FindAction("Movement");
        playerMousePosAction = survivorInputMap.FindAction("MousePos");
        playerLookAction = survivorInputMap.FindAction("Look");
        playerDashAction = survivorInputMap.FindAction("Dash");

        //playerMovementAction.performed += OnPlayerMove;
        //Debug.Log(InputManager.instance.playerInputComponent.currentControlScheme);
        //Debug.Log(InputManager.instance.playerInputComponent.currentActionMap);
        playerDashAction.started += OnDash;
    }

    

    private void Update()
    {
        if (!survivorInputMap.enabled)
        {
            return;
        }

        if (InputManager.instance.playerInputComponent.currentControlScheme == "Keyboard&Mouse")
        {
            Vector2 _mousePos = playerMousePosAction.ReadValue<Vector2>();
            Vector2 _lookDir = Camera.main.ScreenToWorldPoint(_mousePos) - transform.position;

            if(_lookDir != Vector2.zero)
            {
                player.playerPivot.transform.up = _lookDir.normalized;
            }
        }
        else
        {
            Vector2 _lookDir = playerLookAction.ReadValue<Vector2>();
            if(_lookDir != Vector2.zero)
            {
                player.playerPivot.transform.up = _lookDir.normalized;
            }
        }
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (canDash)
        {
            canDash = false;
            isDashing = true;

            this.StartCallAfterSeconds(() =>
            {
                isDashing = false;
            }, dashDuration);
            this.StartCallAfterSeconds(() =>
            {
                canDash = true;
            }, dashCooldown);
        }
    }

    private void OnDestroy()
    {
        playerDashAction.started -= OnDash;
    }

    private void FixedUpdate()
    {
        if (!survivorInputMap.enabled)
        {
            return;
        }

        Vector2 _inputMoveDir = playerMovementAction.ReadValue<Vector2>();
        //Don't normalize if trying to "walk"
        if (_inputMoveDir.magnitude > 1f)
        {
            _inputMoveDir.Normalize();
        }

        player.rb.velocity = _inputMoveDir * moveSpeed * (isDashing ? dashSpeedModifier : 1f) * Time.fixedDeltaTime;
    }

    
}
