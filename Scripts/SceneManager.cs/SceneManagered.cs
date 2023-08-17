using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class SceneManagered : MonoBehaviour
{
    public static SceneManagered instance;
    public Image cover;
    public UnityEngine.UI.Button[] buttons;
    [HideInInspector] public bool isInGame;
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
        if (!File.Exists(Path.Combine(Application.persistentDataPath, DataManager.instance.fileName)))
        {
            buttons[1].interactable = false;
        }
    }
    public void NewGame()
    {
        GameManager.instance.MenuCanvas.GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(newGame());
    }
    private IEnumerator newGame()
    {
        while (cover.color.a < 1)
        {
            cover.color = new Color(0, 0, 0, Mathf.Min(cover.color.a + Time.unscaledDeltaTime, 1));
            yield return null;
        }
        DataManager.instance.NewGame();
        AsyncOperation op = SceneManager.LoadSceneAsync(1);
        GameManager.instance.MenuCanvas.enabled = false;
        KeyboardManager.input.Enable();
        Time.timeScale = 1;
        GameManager.instance.InGameMenu.GetComponent<CanvasGroup>().interactable = true;
        isInGame = true;
        while (!op.isDone)
        {
            yield return null;
        }
        while (cover.color.a > 0)
        {
            cover.color = new Color(0, 0, 0, Mathf.Max(cover.color.a - Time.unscaledDeltaTime, 0));
            yield return null;
        }
    }
    public void LoadGame()
    {
        GameManager.instance.MenuCanvas.GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(loadGame());
    }
    private IEnumerator loadGame()
    {
        while (cover.color.a < 1)
        {
            cover.color = new Color(0, 0, 0, Mathf.Min(cover.color.a + Time.unscaledDeltaTime, 1));
            yield return null;
        }
        DataManager.instance.loadCurrentGame();
        AsyncOperation op = SceneManager.LoadSceneAsync(1);
        GameManager.instance.MenuCanvas.enabled = false;
        KeyboardManager.input.Enable();
        Time.timeScale = 1;
        GameManager.instance.InGameMenu.GetComponent<CanvasGroup>().interactable = true;
        isInGame = true;
        while (!op.isDone)
        {
            yield return null;
        }
        while (cover.color.a > 0)
        {
            cover.color = new Color(0, 0, 0, Mathf.Max(cover.color.a - Time.unscaledDeltaTime, 0));
            yield return null;
        }
    }
    public void CreditScene()
    {
        FileDataHandler dataHandler = new FileDataHandler(Application.persistentDataPath, DataManager.instance.fileName);
        DataManager.instance.SaveGame(dataHandler);
        StartCoroutine(FadeToCreditScene());
    }
    IEnumerator FadeToCreditScene()
    {
        isInGame = false;
        while (cover.color.a < 1)
        {
            cover.color = new Color(0, 0, 0, Mathf.Min(cover.color.a + Time.deltaTime / 2.5f, 1));
            yield return null;
        }
        AsyncOperation op = SceneManager.LoadSceneAsync(2);
        while (!op.isDone)
        {
            yield return null;
        }
        while (cover.color.a > 0)
        {
            cover.color = new Color(0, 0, 0, Mathf.Max(cover.color.a - Time.deltaTime / 2.5f, 0));
            yield return null;
        }
    }
    public void ReturnToMenu(bool save)
    {
        if (save)
        {
            FileDataHandler dataHandler = new FileDataHandler(Application.persistentDataPath, DataManager.instance.fileName);
            DataManager.instance.SaveGame(dataHandler);
        }
        GameManager.instance.InGameMenu.GetComponent<CanvasGroup>().interactable = false;
        StartCoroutine(returnToMenu());
    }
    private IEnumerator returnToMenu()
    {
        while (cover.color.a < 1)
        {
            cover.color = new Color(0, 0, 0, Mathf.Min(cover.color.a + Time.unscaledDeltaTime, 1));
            yield return null;
        }
        GameManager.instance.cameraMat.SetFloat("_IsInProgress", 0);
        GameManager.instance.cameraMat.SetFloat("_Progress", 0);
        AsyncOperation op = SceneManager.LoadSceneAsync(0);
        while (!op.isDone)
        {
            yield return null;
        }
        GameManager.instance.InGameMenu.GetComponent<CanvasGroup>().interactable = false;
        GameManager.instance.MenuCanvas.enabled = true;
        GameManager.instance.BackToGame();
        isInGame = false;
        while (cover.color.a > 0)
        {
            cover.color = new Color(0, 0, 0, Mathf.Max(cover.color.a - Time.unscaledDeltaTime, 0));
            yield return null;
        }
        GameManager.instance.MenuCanvas.GetComponent<CanvasGroup>().interactable = true;
    }
}
