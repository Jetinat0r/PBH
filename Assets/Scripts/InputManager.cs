using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    [SerializeField]
    public PlayerInput playerInputComponent;

    public InputActionAsset inputActions;
    public InputActionMap globalInputMap;
    //public InputActionMap uiInputMap;
    //public InputActionMap survivorInputMap;
    //public InputActionMap conductorInputMap;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        inputActions = playerInputComponent.actions;
        globalInputMap = inputActions.FindActionMap("Global");
        //uiInputMap = inputActions.FindActionMap("UI");
        //survivorInputMap = inputActions.FindActionMap("Survivor");
        //conductorInputMap = inputActions.FindActionMap("Conductor");
    }
}
