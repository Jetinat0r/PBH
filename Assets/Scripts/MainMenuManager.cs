using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using Riptide;
using System;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform mainMenuPanel;
    [SerializeField]
    private TMP_InputField playerNameInputField;
    [SerializeField]
    private TMP_InputField serverAddressInputField;
    [SerializeField]
    private TMP_InputField serverPortInputField;
    [SerializeField]
    private TMP_Text clientErrorText;
    private Color errorColor = new Color(1f, 0f, 0f, 1f);
    private Color invisibleErrorColor = new Color(1f, 0f, 0f, 0f);
    private Tween errorTextFadeTween;
    [SerializeField]
    private float errorFadeoutTime = 5f;
    [SerializeField]
    private Image loadingWheel;
    [SerializeField]
    private float loadingWheelRotationTime = 2.5f;
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button settingsButton;

    // Start is called before the first frame update
    void Start()
    {
        loadingWheel.transform.DORotate(new Vector3(0f, 0f, -360f), loadingWheelRotationTime, RotateMode.LocalAxisAdd).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1);
        ClientManager.instance.client.Connected += DidConnect;
        ClientManager.instance.client.ConnectionFailed += OnConnectionFailed;
    }

    //Called to bring the main menu back into view
    public void ReturnToMainMenu()
    {
        mainMenuPanel.gameObject.SetActive(true);
    }

    //Called to remove the main menu from view
    public void LeaveMainMenu()
    {
        SetUiConnectingState(false);
        mainMenuPanel.gameObject.SetActive(false);
    }

    //What each UI element should do when attempting to connect to a server
    //  Mainly used for throbber feedback and disabling input elements during a connection attempt
    public void SetUiConnectingState(bool _connecting)
    {
        loadingWheel.gameObject.SetActive(_connecting);
        playerNameInputField.interactable = !_connecting;
        serverAddressInputField.interactable = !_connecting;
        serverPortInputField.interactable = !_connecting;
        playButton.interactable = !_connecting;
        settingsButton.interactable = !_connecting;
    }

    public void FlashError(string _errorText)
    {
        SetUiConnectingState(false);

        errorTextFadeTween?.Kill();
        clientErrorText.text = _errorText;
        clientErrorText.color = errorColor;
        errorTextFadeTween = clientErrorText.DOColor(invisibleErrorColor, errorFadeoutTime).SetEase(Ease.InCubic);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            FlashError(serverAddressInputField.text);
        }
    }

    #region ButtonEvents
    //Attempts to connect to the server
    //  Also sends preferred name and color
    public void TryConnectToServer()
    {
        //Validate preferred name and color input
        string _preferredPlayerName = playerNameInputField.text.Trim();
        if (_preferredPlayerName.Length <= 0 || _preferredPlayerName.Length >= ClientManager.PLAYER_NAME_MAX_LENGTH)
        {
            FlashError("Name must be 1-10 characters long!");
            return;
        }

        Message _message = Message.Create();
        //Add preferred name
        _message.AddString(_preferredPlayerName);
        //Add preferred color index
        _message.AddInt(0);

        if (ClientManager.instance.client.Connect(serverAddressInputField.text + ":" + serverPortInputField.text, message: _message))
        {
            SetUiConnectingState(true);
        }
        else
        {
            FlashError("Could not resolve server address, check address and port!");
        }
    }
    #endregion

    #region ServerEvents
    private void OnConnectionFailed(object _sender, ConnectionFailedEventArgs _e)
    {
        switch (_e.Reason)
        {
            case (RejectReason.ServerFull):
                FlashError("Connection rejected: Server is full!");
                break;
            case (RejectReason.AlreadyConnected):
                FlashError("Connection rejected: Already connected to server!");
                Debug.LogError("Connection rejected: Already connected to server!");
                break;
            case (RejectReason.NoConnection):
                FlashError("Connection rejected: Could not connect to server!");
                break;
            case (RejectReason.Custom):
                if(_e.Message == null)
                {
                    FlashError("Connection rejected: Unknown custom rejection from Server!");
                }
                else
                {
                    FlashError("Connection rejected: " + _e.Message.GetString());
                }
                break;
        }
    }

    private void DidConnect(object _sender, EventArgs _e)
    {
        LeaveMainMenu();
        //SendPlayerInfo();
    }
    #endregion
}
