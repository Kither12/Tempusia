using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool isEndGame;
    public PlayerData playerData;
    public SerializeDictionary<string, BoxData> boxData;
    public SerializeDictionary<string, ElevatorData> elevatorData;
    public SerializeDictionary<string, PortalData> portalData;
    public SerializeDictionary<string, KeyData> keyData;
    public SerializeDictionary<string, bool> endData;
    public GameData()
    {
        playerData = new PlayerData();
        boxData = new SerializeDictionary<string, BoxData>();
        elevatorData = new SerializeDictionary<string, ElevatorData>();
        portalData = new SerializeDictionary<string, PortalData>();
        keyData = new SerializeDictionary<string, KeyData>();
        endData = new SerializeDictionary<string, bool>();
    }
}
