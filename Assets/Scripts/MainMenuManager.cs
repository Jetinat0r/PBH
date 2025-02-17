using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        loadingWheel.transform.DORotate(new Vector3(0f, 0f, -360f), loadingWheelRotationTime, RotateMode.LocalAxisAdd).SetRelative(true).SetEase(Ease.Linear).SetLoops(-1);
    }

    //Called to bring the main menu back into view
    public void EnterMainMenu()
    {

    }

    //Called to remove the main menu from view
    public void ExitMainMenu()
    {

    }

    public void FlashError(string _errorText)
    {
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
}
