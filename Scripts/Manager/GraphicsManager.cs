using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicsManager : MonoBehaviour
{
    public TextMeshProUGUI FullScreenText, VSyncText, ResolutionText;
    public List<ResItem> res = new List<ResItem>();
    private int currentresIndex = 0;
    private bool screenFullscreen;
    private void Start()
    {
        if (PlayerPrefs.HasKey("V-Sync"))
        {
            screenFullscreen = (PlayerPrefs.GetInt("fullScreen") == 1);
            QualitySettings.vSyncCount = PlayerPrefs.GetInt("V-Sync");
            currentresIndex = PlayerPrefs.GetInt("resIndex");
            Screen.SetResolution(res[currentresIndex].width, res[currentresIndex].height, screenFullscreen ? true : false);
        }
        else
        {
            ResetDefault();
            screenFullscreen = true;
        }
        UpdateUI();
    }
    private void UpdateUI()
    {
        if (QualitySettings.vSyncCount == 0)
        {
            VSyncText.text = "OFF";
        }
        else
        {
            VSyncText.text = "ON";
        }
        if (Screen.fullScreen)
        {
            FullScreenText.text = "ON";
        }
        else
        {
            FullScreenText.text = "OFF";
        }
        ResolutionText.text = res[currentresIndex].width + " x " + res[currentresIndex].height;
    }
    private void Update()
    {
        UpdateUI();
    }
    public void ChangeScreenMode()
    {
        Screen.fullScreen = !Screen.fullScreen;
        screenFullscreen = !screenFullscreen;
        PlayerPrefs.SetInt("fullScreen", screenFullscreen ? 1 : 0);
    }
    public void ChangeVSync()
    {
        QualitySettings.vSyncCount = (QualitySettings.vSyncCount == 0) ? 1 : 0;
        PlayerPrefs.SetInt("V-Sync", QualitySettings.vSyncCount);
    }
    public void ResRight()
    {
        currentresIndex = (currentresIndex + 1 + res.Count) % res.Count;
        Screen.SetResolution(res[currentresIndex].width, res[currentresIndex].height, Screen.fullScreen);
        PlayerPrefs.SetInt("resIndex", currentresIndex);
    }
    public void ResLeft()
    {
        currentresIndex = (currentresIndex - 1 + res.Count) % res.Count;
        Screen.SetResolution(res[currentresIndex].width, res[currentresIndex].height, Screen.fullScreen);
        PlayerPrefs.SetInt("resIndex", currentresIndex);
    }
    public void ResetDefault()
    {
        QualitySettings.vSyncCount = 0;
        Screen.SetResolution(res[0].width, res[0].height, true);
        currentresIndex = 0;
        PlayerPrefs.SetInt("resIndex", 0);
        PlayerPrefs.SetInt("V-Sync", 0);
        PlayerPrefs.SetInt("fullScreen", 1);
        screenFullscreen = true;
    }
}
[System.Serializable]
public class ResItem
{
    public int width, height;
}
