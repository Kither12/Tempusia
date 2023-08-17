using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    public string fileName;


    [HideInInspector]public GameData gameData;
    private List<IData> dataObjects;
    [HideInInspector] public bool hasData;
    [HideInInspector] public GameData originalData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }
    private void Start()
    {
        FileDataHandler dataHandler = new FileDataHandler(Application.persistentDataPath, "Original.kit");
        originalData = dataHandler.Load();
    }
    public void loadCurrentGame()
    {
        FileDataHandler dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        LoadGame(dataHandler);
    }
    public void NewGame()
    {
        hasData = false;
        gameData = new GameData();
        GameManager.instance.isGameOver = false;
    }
    public void LoadDataToWorld(GameData gameData)
    {
        dataObjects = FindAllDataObjects();
        foreach (IData dataObject in dataObjects)
        {
            dataObject.LoadData(gameData);
        }
    }
    public void LoadGame(FileDataHandler dataHandler)
    {
        gameData = dataHandler.Load();
        hasData = true;
        GameManager.instance.isGameOver = false;
        if (gameData == null)
        {
            hasData = false;
            return;
        }
    }
    public void SaveGame(FileDataHandler dataHandler)
    {
        dataObjects = FindAllDataObjects();
        foreach (IData dataObject in dataObjects)
        {
            dataObject.SaveData(gameData);
        }
        if(SceneManagered.instance.isInGame)
        {
            dataHandler.Save(gameData);
        }
    }
    private void OnApplicationQuit()
    {
        FileDataHandler dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        SaveGame(dataHandler);
        PlayerPrefs.Save();
    }

    public List<IData> FindAllDataObjects()
    {
        IEnumerable<IData> dataObjects = FindObjectsOfType<MonoBehaviour>().OfType<IData>();
        return new List<IData>(dataObjects);
    }
    public void getStartData()
    {
        if (!File.Exists(Path.Combine(Application.persistentDataPath, "Original.kit")))
        {
            FileDataHandler dataHandler = new FileDataHandler(Application.persistentDataPath, "Original.kit");
            SaveGame(dataHandler);
            originalData = dataHandler.Load();
        }
    }
}
