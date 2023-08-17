using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CheckPoint : MonoBehaviour
{
    public int checkPointIndex;
    [HideInInspector] public GameData gameData;
    private PlayerInput input;
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
    }
    private void Awake()
    {
        input = new PlayerInput();
        if (!DataManager.instance.hasData)
        {
            string path = Path.Combine(Application.persistentDataPath, "checkPoint_" + checkPointIndex + ".kit");
            if (File.Exists(path))
            {
                File.SetAttributes(path, FileAttributes.Normal);
                File.Delete(path);
            }
        }
        LoadData();
    }
    private void Start()
    {
        if(gameData != null)
        {
            haveDataEffect();
        }
    }
    private void LoadData()
    {
        FileDataHandler dataHandler = new FileDataHandler(Application.persistentDataPath, "checkPoint_" + checkPointIndex + ".kit");
        gameData = dataHandler.Load();
    }
    private void SaveData()
    {
        if(gameData == null)
        {
            FileDataHandler dataHandler = new FileDataHandler(Application.persistentDataPath, "checkPoint_" + checkPointIndex + ".kit");
            DataManager.instance.SaveGame(dataHandler);
            CheckPointManager.instance.AddCheckPoint(this);
            haveDataEffect();
            gameData = dataHandler.Load();
        }
    }
    private void haveDataEffect()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
        GetComponent<BoxCollider2D>().isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SaveData();
        }
    }
}
