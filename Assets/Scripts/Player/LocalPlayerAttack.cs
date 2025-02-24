using Riptide;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalPlayerAttack : MonoBehaviour
{
    [SerializeField]
    private LocalPlayer player;

    private InputActionMap survivorInputMap;
    private InputAction playerPushAction;

    private bool canPush = true;
    private bool isPushing = false;

    void Awake()
    {
        survivorInputMap = InputManager.instance.globalInputMap;

        playerPushAction = survivorInputMap.FindAction("Push");

        //playerMovementAction.performed += OnPlayerMove;
        //Debug.Log(InputManager.instance.playerInputComponent.currentControlScheme);
        //Debug.Log(InputManager.instance.playerInputComponent.currentActionMap);
        playerPushAction.started += ChargePush;
        playerPushAction.canceled += ExecutePush;
    }

    private void OnDestroy()
    {
        playerPushAction.started -= ChargePush;
        playerPushAction.canceled -= ExecutePush;
    }

    private void ChargePush(InputAction.CallbackContext context)
    {
        if (!canPush)
        {
            return;
        }

        canPush = false;
        isPushing = true;

        player.ChargePush();
        Message _chargePushMessage = Message.Create(MessageSendMode.Reliable, ClientToServerId.pushStart);
        ClientManager.instance.client.Send(_chargePushMessage);
    }

    private void ExecutePush(InputAction.CallbackContext context)
    {
        if (!isPushing)
        {
            return;
        }

        isPushing = false;

        player.ExecutePush();
        Message _pushExecuteMessage = Message.Create(MessageSendMode.Reliable, ClientToServerId.pushExecute);
        ClientManager.instance.client.Send(_pushExecuteMessage);
        this.StartCallAfterSeconds(EndPush, player.pushActiveTime);
    }

    private void EndPush()
    {
        player.EndPush();
        Message _pushEndMessage = Message.Create(MessageSendMode.Reliable, ClientToServerId.playerPushReturn);
        ClientManager.instance.client.Send(_pushEndMessage);

        this.StartCallAfterSeconds(() => { canPush = true; }, player.pushReturnTime);
    }
}
