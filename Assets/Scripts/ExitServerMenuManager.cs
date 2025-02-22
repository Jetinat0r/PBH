using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitServerMenuManager : MonoBehaviour
{
    public static ExitServerMenuManager instance;

    [SerializeField]
    public GameObject exitServerMenuPanel;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void OpenMenu()
    {
        exitServerMenuPanel.SetActive(true);
    }

    public void CloseMenu()
    {
        exitServerMenuPanel.SetActive(false);
    }

    public void LeaveServer()
    {
        //TODO: Replace with request disconnect message
        ClientManager.instance.client.Disconnect();
        CloseMenu();

        MainMenuManager.instance.ReturnToMainMenu();
    }
}
