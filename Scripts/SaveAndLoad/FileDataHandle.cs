using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";

    private string dataFileName = "";

    private readonly string key = "NaoTomori-GawrGura-KaoriMiyazono-KITHER";
    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public void Save(GameData gameData)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

        string dataToStore = JsonUtility.ToJson(gameData);

        dataToStore = EncryptDecrypt(dataToStore);
        if (File.Exists(fullPath))
        {
            File.SetAttributes(fullPath, File.GetAttributes(fullPath) & ~FileAttributes.ReadOnly);
        }

        using (FileStream stream = new FileStream(fullPath, FileMode.Create))
        {
            using (StreamWriter streamWriter = new StreamWriter(stream))
            {
                streamWriter.Write(dataToStore);
            }
        }
        File.SetAttributes(fullPath, File.GetAttributes(fullPath) | FileAttributes.ReadOnly);
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            string dataToLoaded = "";
            using (FileStream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    dataToLoaded = streamReader.ReadToEnd();
                }
            }

            dataToLoaded = EncryptDecrypt(dataToLoaded);

            loadedData = JsonUtility.FromJson<GameData>(dataToLoaded);
        }

        return loadedData;
    }
    private string EncryptDecrypt(string data)
    {
        string modifiledData = "";
        for(int i = 0; i < data.Length; ++i)
        {
            modifiledData += (char)(data[i] ^ key[i % key.Length]);
        }
        return modifiledData;
    }
}
