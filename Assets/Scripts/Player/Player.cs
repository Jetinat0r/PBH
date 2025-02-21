using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> playerList = new Dictionary<ushort, Player>();

    [SerializeField]
    protected GameObject playerPivot;

    [SerializeField]
    private GameObject handHolder;
    private Tween pushAnimationTween = null;
    private Coroutine pushAnimationCoroutine = null;
    private Coroutine pushForceBoxCoroutine = null;
    //All offsets are Y's
    [SerializeField]
    private float handFullBackOffset = -0.25f;
    private float handDefaultOffset;
    [SerializeField]
    private float handFullForwardOffset = 0.5f;

    [SerializeField]
    private float pushChargeTime = 0.15f; //How long to pull the arms back for the push
    [SerializeField]
    private float pushActiveTime = 0.4f; //How long the actual "push" lasts
    [SerializeField]
    private float pushBoxActiveStart = 0.1f; //When during pushActiveTime to enable the push box
    [SerializeField]
    private float pushBoxActiveEnd = 0.4f; //When during the pushActiveTime to disable the push box
    [SerializeField]
    private float pushReturnTime = 0.2f; //How long from max extension to return to rest so next push can start
    [SerializeField]
    private GameObject pushForceBox;

    [SerializeField]
    private TMP_Text nameplate;
    [SerializeField]
    private SpriteRenderer baseGraphics;
    [SerializeField]
    private SpriteRenderer[] handGraphics = new SpriteRenderer[2];

    public ushort playerId;
    public string playerUserName = "Player";

    private void Start()
    {
        handDefaultOffset = handHolder.transform.localPosition.y;
    }

    public void SetSpawnInfo(ushort _id, string _username)
    {
        playerId = _id;
        playerUserName = _username;
        //Above line should actually be handled by server
        baseGraphics.color = ClientManager.instance.playerColors[_id];

        foreach (SpriteRenderer sr in handGraphics)
        {
            sr.color = ClientManager.instance.playerColors[_id];
        }

        nameplate.text = playerUserName;
    }

    public void ChargePush(bool _autoStart = false)
    {
        this.SafeStopCoroutine(pushAnimationCoroutine);

        this.SafeStopCoroutine(pushForceBoxCoroutine);
        pushForceBox.SetActive(false);

        pushAnimationTween?.Kill();
        pushAnimationTween = handHolder.transform.DOLocalMoveY(handFullBackOffset, pushChargeTime).SetEase(Ease.OutQuad);

        //Unsure whether or not to allow charging of pushes. This var allows that decision to be made easily
        //  Remote players should probably never have this set
        if (_autoStart)
        {
            pushAnimationCoroutine = this.StartCallAfterSeconds(ExecutePush, pushChargeTime);
        }
    }

    public void ExecutePush()
    {
        this.SafeStopCoroutine(pushAnimationCoroutine);

        pushAnimationTween?.Kill();
        pushAnimationTween = handHolder.transform.DOLocalMoveZ(handFullForwardOffset, pushActiveTime).SetEase(Ease.OutCubic);

        pushAnimationCoroutine = this.StartCallAfterSeconds(EndPush, pushActiveTime);
        pushForceBoxCoroutine = this.StartCallAfterSeconds(() =>
        {
            pushForceBox.SetActive(true);
            pushForceBoxCoroutine = this.StartCallAfterSeconds(() => { pushForceBox.SetActive(false); }, pushBoxActiveEnd - pushBoxActiveStart);
        }, pushBoxActiveStart);
    }

    public void EndPush()
    {
        this.SafeStopCoroutine(pushAnimationCoroutine);

        pushAnimationTween?.Kill();
        pushAnimationTween = handHolder.transform.DOLocalMoveZ(handDefaultOffset, pushReturnTime).SetEase(Ease.OutQuad);
    }
}
