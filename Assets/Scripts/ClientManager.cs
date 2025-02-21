using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Riptide;
using Riptide.Utils;

public enum ClientToServerId : ushort
{
    requestDisconnect = 0,
    joinInfo = 1,
    playerPosRot = 2,
    pushStart = 3,
    pushExecute = 4,
}

public enum ServerToClientId : ushort
{
    disconnectWithReason = 0,
    playerSpawnInfo = 1,
    playerPosRot = 2,
    playerPushStart = 3,
    playerPushExecute = 4,
}

public class ClientManager : MonoBehaviour
{
    //Maximum length in chars a player's name can be
    //  The server can return a name longer due to adding identifiers (e.g. Player1, Player2)
    //  This length ensures that with the server's additions the name is not too long for the display field
    public const int PLAYER_NAME_MAX_LENGTH = 10;

    public static ClientManager instance = null;

    public Client client;
    [SerializeField]
    public List<Color> playerColors = new List<Color>();
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

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        client = new Client();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        client.Update();
    }

    #region ServerEvents
    
    #endregion
}
