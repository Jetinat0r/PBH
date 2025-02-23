using Riptide;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayer : Player
{
    [SerializeField]
    public LocalPlayerMovement movement;
    [SerializeField]
    public LocalPlayerAttack attack;

    private void FixedUpdate()
    {
        SendPlayerPosRot();
    }

    #region Messages
    public void SendPlayerPosRot()
    {
        Message message = Message.Create(MessageSendMode.Unreliable, ClientToServerId.playerPosRot);
        message.AddVector3(transform.position);
        message.AddQuaternion(playerPivot.transform.rotation);

        ClientManager.instance.client.Send(message);
    }

    public void SendChargePush()
    {
        ChargePush();
    }

    public void SendExecutePush()
    {
        ExecutePush();
    }
    #endregion
}
