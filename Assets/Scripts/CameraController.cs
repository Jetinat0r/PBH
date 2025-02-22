using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [SerializeField]
    private Transform mainCameraCenter;
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Transform mainMenuPos;
    [SerializeField]
    private Transform settingsMenuPos;
    [SerializeField]
    private Transform arenaSurvivorViewPos;
    [SerializeField]
    private Transform arenaConductorViewPos;
    [SerializeField]
    private float defaultZoom = 12f;
    [SerializeField]
    private float conductorZoom = 16f;

    private Tween sceneChangeTween;
    private Tween cameraZoomTween;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //_time is in seconds
    public void TweenCamPosZoom(Transform _targetPos, float _targetZoom, float _time)
    {
        sceneChangeTween?.Kill();
        cameraZoomTween?.Kill();

        sceneChangeTween = mainCameraCenter.transform.DOMove(_targetPos.position, _time).SetEase(Ease.InOutQuad);
        cameraZoomTween = mainCamera.DOOrthoSize(_targetZoom, _time).SetEase(Ease.InOutQuad);
    }

    //Magnitude: how far / hard the camera shakes in game units
    //Frequency: how often the camera shakes per seconds
    //Duration: how long the shaking lasts
    //Magnitude Easing: how the magnitude of the shake should change over duration
    //Frequency Easing: how the frequency of the shake should change over duration
    public void ApplyShake(float _magnitude, float _frequency, float _duration, Ease _magnitudeEasing = Ease.OutQuad, Ease _frequencyEasing = Ease.OutQuad)
    {
        Debug.LogWarning($"Camera Shake is not yet implemented!");
    }

    //Times are not baked into the class because the from context will influence the transition time
    //  e.g. Time for Main Menu -> Survivor View != Time for Conductor View -> Survivor View

    public void MoveToMainMenu(float _time)
    {
        TweenCamPosZoom(mainMenuPos, defaultZoom, _time);
    }

    public void MoveToSettingsMenu(float _time)
    {
        TweenCamPosZoom(settingsMenuPos, defaultZoom, _time);
    }

    public void MoveToArenaSurvivorView(float _time)
    {
        TweenCamPosZoom(arenaSurvivorViewPos, defaultZoom, _time);
    }

    public void MoveToArenaConductorView(float _time)
    {
        TweenCamPosZoom(arenaConductorViewPos, conductorZoom, _time);
    }
}
