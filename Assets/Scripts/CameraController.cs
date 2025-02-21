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
    private Transform mainMenuPos;
    [SerializeField]
    private Transform settingsMenuPos;
    [SerializeField]
    private Transform arenaSurvivorViewPos;
    [SerializeField]
    private Transform arenaConductorViewPos;

    private Tween sceneChangeTween;

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
    public void MoveCamToPos(Transform _targetPos, float _time)
    {
        sceneChangeTween?.Kill();
        sceneChangeTween = mainCameraCenter.transform.DOMove(_targetPos.position, _time).SetEase(Ease.InOutQuad);
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
        MoveCamToPos(mainMenuPos, _time);
    }

    public void MoveToSettingsMenu(float _time)
    {
        MoveCamToPos(settingsMenuPos, _time);
    }

    public void MoveToArenaSurvivorView(float _time)
    {
        MoveCamToPos(arenaSurvivorViewPos, _time);
    }

    public void MoveToArenaConductorView(float _time)
    {
        MoveCamToPos(arenaConductorViewPos, _time);
    }
}
