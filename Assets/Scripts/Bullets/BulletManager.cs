using Riptide;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PatternAssociation
{
    public CardType type;
    public BulletSpawner bulletSpawnerPrefab;
}

public class BulletManager : MonoBehaviour
{
    public static BulletManager instance;

    [SerializeField]
    public List<PatternAssociation> bulletPatternDictionary = new List<PatternAssociation>();

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
    }

    public void PlaceBulletPattern(Vector3Int _tilePos, int _bulletPatternId)
    {
        if(!GridTileSelector.instance.TryGetWorldPosFromTilePos(_tilePos, out Vector3 _worldPos))
        {
            Debug.LogError($"Invalid bullet pattern spawn position: {_tilePos}!");
            return;
        }
        _worldPos.z = 0f; //Probably not necessary, but I like it

        if(_bulletPatternId < 0 || _bulletPatternId >= bulletPatternDictionary.Count)
        {
            Debug.LogError($"Invalid bullet pattern: {_bulletPatternId}!");
            return;
        }

        BulletSpawner _newSpawner = Instantiate(bulletPatternDictionary[_bulletPatternId].bulletSpawnerPrefab, _worldPos, Quaternion.identity);
        _newSpawner.transform.parent = transform;

        _newSpawner.StartPattern(_bulletPatternId);
    }

    #region Messages
    [MessageHandler((ushort)ServerToClientId.spawnBulletPattern)]
    public static void ReceiveSpawnBulletPattern(Message _message)
    {
        Vector3Int _tilePos = _message.GetVector3Int();
        int _bulletPatternId = _message.GetInt();

        instance.PlaceBulletPattern(_tilePos, _bulletPatternId);
    }
    #endregion
}
