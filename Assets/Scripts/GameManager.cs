using Riptide;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<ushort, Player> playerList = new Dictionary<ushort, Player>();
    [SerializeField]
    public List<Color> playerColors = new List<Color>();
    [SerializeField]
    public List<Transform> playerSpawnPositions = new List<Transform>();

    [SerializeField]
    public Player localPlayerPrefab;
    [SerializeField]
    public Player remotePlayerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        ClientManager.instance.client.ClientDisconnected += OnClientDisconnect;
        ClientManager.instance.client.Disconnected += OnDisconnect;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnNewPlayer(ushort _newClientId, string _playerName, int _playerColor)
    {
        //This if could technically be removed for a ternary on the prefab, but we may want to do something different later
        if (ClientManager.instance.client.Id == _newClientId)
        {
            //If we are joining
            //Spawn localPlayer prefab
            Transform _spawnPos = playerSpawnPositions[_newClientId % playerSpawnPositions.Count];
            Player _newPlayer = Instantiate(instance.localPlayerPrefab, _spawnPos.position, Quaternion.identity);
            _newPlayer.SetSpawnInfo(_newClientId, _playerName, _playerColor);
            _newPlayer.SpawnPlayer(_spawnPos.position, Quaternion.identity);

            playerList.Add(_newClientId, _newPlayer);
        }
        else
        {
            //If someone else is joining us
            //Spawn remotePlayer prefab
            Transform _spawnPos = playerSpawnPositions[_newClientId % playerSpawnPositions.Count];
            Player _newPlayer = Instantiate(instance.remotePlayerPrefab, _spawnPos.position, Quaternion.identity);
            _newPlayer.SetSpawnInfo(_newClientId, _playerName, _playerColor);
            _newPlayer.SpawnPlayer(_spawnPos.position, Quaternion.identity);

            playerList.Add(_newClientId, _newPlayer);
        }
    }

    #region Messages
    #region Joining & Disconnecting
    [MessageHandler((ushort)ServerToClientId.playerSpawnInfo)]
    public static void AcceptNewClient(Message _message)
    {
        ushort _newClientId = _message.GetUShort();
        string _playerName = _message.GetString();
        int _playerColor = _message.GetInt();

        //Protect against duplicate players and/or shenanigans
        if (playerList.ContainsKey(_newClientId))
        {
            Debug.LogWarning($"Accepting new Client {_newClientId}: {_playerName}|{_playerColor} that already exists as {playerList[_newClientId].playerUserName}|{playerList[_newClientId].colorId}!");
            return;
        }

        //Create new player
        instance.SpawnNewPlayer(_newClientId, _playerName, _playerColor);
    }

    //When we disconnect
    public void OnDisconnect(object _sender, DisconnectedEventArgs _e)
    {
        foreach (Player _player in playerList.Values)
        {
            _player.DisconnectPlayer();
        }
        playerList.Clear();

        /*
        if (playerList.TryGetValue(ClientManager.instance.client.Id, out Player _disconnectedPlayer))
        {
            Debug.Log($"Disconnected player {ClientManager.instance.client.Id}; Goodbye, {_disconnectedPlayer.playerUserName}!");
            _disconnectedPlayer.DisconnectPlayer();
            playerList.Remove(ClientManager.instance.client.Id);
        }
        else
        {
            Debug.LogWarning($"Player {ClientManager.instance.client.Id} could not be removed, does not exist!");
        }
        */
    }

    //When someone else disconnects
    public void OnClientDisconnect(object _sender, ClientDisconnectedEventArgs _e)
    {
        if(playerList.TryGetValue(_e.Id, out Player _disconnectedPlayer))
        {
            Debug.Log($"Disconnected player {_e.Id}; Goodbye, {_disconnectedPlayer.playerUserName}!");
            _disconnectedPlayer.DisconnectPlayer();
            playerList.Remove(_e.Id);
        }
        else
        {
            Debug.LogWarning($"Player {_e.Id} could not be removed, does not exist!");
        }
    }
    #endregion

    [MessageHandler((ushort)ServerToClientId.playerPosRot)]
    public static void UpdatePlayerPosRot(Message _message)
    {
        ushort _fromClientId = _message.GetUShort();
        Vector3 _pos = _message.GetVector3();
        Quaternion _rot = _message.GetQuaternion();

        if(_fromClientId == ClientManager.instance.client.Id)
        {
            Debug.LogWarning($"Received own PosRot!");
            return;
        }

        if (playerList.TryGetValue(_fromClientId, out Player _player))
        {
            _player.transform.SetPositionAndRotation( _pos, _rot);
        }
        else
        {
            Debug.LogWarning($"Received PosRot for player {_fromClientId} that does not exist!");
        }
    }
    #endregion
}
