using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;
using Riptide.Utils;

public class ClientManager : MonoBehaviour
{
    Client client;
    [SerializeField]
    private string serverAddress = "127.0.0.1";
    [SerializeField]
    private ushort port = 7777;
    private string desiredPlayerName = "Player";
    private string assignedPlayerName = "Player";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    public static void InitializeRiptide()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        client = new Client();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        client.Update();
    }
}
