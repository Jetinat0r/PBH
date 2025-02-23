using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public GameObject playerPivot;

    [SerializeField]
    public Rigidbody2D rb;
    [SerializeField]
    public Collider2D physicsCollider;

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
    public int colorId = 0;

    private void Start()
    {
        handDefaultOffset = handHolder.transform.localPosition.y;
    }

    public void SetSpawnInfo(ushort _id, string _username, int _colorId)
    {
        playerId = _id;
        playerUserName = _username;
        colorId = _colorId;

        nameplate.color = GameManager.instance.playerColors[colorId];
        baseGraphics.color = GameManager.instance.playerColors[colorId];

        foreach (SpriteRenderer sr in handGraphics)
        {
            sr.color = GameManager.instance.playerColors[colorId];
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

    public void SpawnPlayer(Vector3 _spawnPos, Quaternion _spawnRot)
    {
        //TODO: Animation / particle FX for spawning
        transform.SetPositionAndRotation(_spawnPos, _spawnRot);
    }

    public void KillPlayer()
    {
        //TODO: Animation / particle FX for exploding
        //TODO: Hide graphics, disable input, and remove from arena; move to GBJ
    }

    public void DisconnectPlayer()
    {
        KillPlayer();

        //TODO: Delay until after kill animation
        Destroy(gameObject);
    }
}
