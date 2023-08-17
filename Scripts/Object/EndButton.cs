using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EndButton : Button, IData
{
    public CinemachineVirtualCamera cineCamera;
    private float previewTime = 1.5f;
    public GameObject player;
    public string ID;

    public override void Start()
    {
        base.Start();
        if (DataManager.instance.hasData)
        {
            LoadData(DataManager.instance.gameData);
        }
    }
    public override void Trigger(bool save)
    {
        if (!check)
        {
            lightColor.color = Color.green;
            audioSource.Play();
            check = true;
            door.Check();
            StartCoroutine(previewDoor());
        }
    }
    private IEnumerator previewDoor()
    {
        cineCamera.Follow = door.transform;
        cineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = 0f;
        cineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0f;
        KeyboardManager.input.Disable();
        yield return new WaitForSecondsRealtime(previewTime);
        KeyboardManager.input.Enable();
        cineCamera.Follow = player.transform;
        cineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneWidth = 0.2f;
        cineCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_DeadZoneHeight = 0.1f;
    }
    public void SaveData(GameData data)
    {
        if (!string.IsNullOrEmpty(ID))
        {
            if (data.endData.ContainsKey(ID))
            {
                data.endData.Remove(ID);
            }
            data.endData.Add(ID, lightColor.color == Color.green);
        }
    }
    public void LoadData(GameData data)
    {
        if (!string.IsNullOrEmpty(ID))
        {
            bool isGreen;
            data.endData.TryGetValue(ID, out isGreen);
            if (isGreen)
            {
                lightColor.color = Color.green;
                check = true;
                door.Check();
            }
        }
    }
}
