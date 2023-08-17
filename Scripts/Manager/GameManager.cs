using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public LayerMask layerGround;
    public Image cover;
    public Canvas OptionCanvas, MenuCanvas, AudioOptionCanvas, KeyboardCanvas, InGameMenu, VideoCanvas;
    public float FadeDelay;
    public GameObject Blur;
    private Canvas currentOptionCanvas;
    private Canvas currentCanvas;
    private Canvas nextCanvas;
    public bool isShowingInteractCanvas; 
    public bool isInGameMenu;
    public bool isGameOver;

    private float previousTime;
    private PlayerInput gameInput;

    public static GameManager instance;
    public Material cameraMat;

    public EventSystem eventSystem;
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
        gameInput = new PlayerInput();
        gameInput.Enable();
        gameInput.normal.Exit.performed += (_) =>
        {
            if (!SceneManagered.instance.isInGame)
            {
                if(OptionCanvas.enabled)
                {
                    BackOption();
                }
                else if(currentOptionCanvas != null)
                {
                    BackChildOption();
                }
            }
            else
            {
                if(!isInGameMenu && !InGameMenu.enabled)
                {
                    if (isShowingInteractCanvas)
                    {
                        isShowingInteractCanvas = false;
                    }
                    else
                    {
                        InGameOption();
                    }
                }
                else if(isInGameMenu && InGameMenu.enabled)
                {
                    BackToGame();
                }
                else if (OptionCanvas.enabled)
                {
                    BackOption();
                }
                else if (currentOptionCanvas != null)
                {
                    BackChildOption();
                }
            }
        };
    }
    private void Update()
    {
        if(Time.timeScale == 0)
        {
            VolumeManager.instance.muteMusic();
            VolumeManager.instance.muteSfx();
        }
        else
        {
            VolumeManager.instance.unmuteMusic();
            VolumeManager.instance.unmuteSfx();
        }
        if(Mathf.Abs(Mouse.current.delta.x.ReadValue()) > 0.25)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    IEnumerator startOver()
    {
        isGameOver = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<SpriteRenderer>().enabled = false;
        while(cover.color.a < 1)
        {
            cover.color = new Color(cover.color.r, cover.color.g, cover.color.b, Mathf.Min(cover.color.a + Time.deltaTime, 1));
            yield return null;
        }
        player.GetComponent<SpriteRenderer>().enabled = true;
        player.GetComponent<AudioSource>().mute = false;
        if (CheckPointManager.instance.checkPointCount() == 0)
        {
            DataManager.instance.LoadDataToWorld(DataManager.instance.originalData);
        }
        else
        {
            CheckPointManager.instance.ActiveSelectionCheckPoint();
            CheckPointManager.instance.selectCurrentCheckPoint();
        }
        isGameOver = false;
        player.GetComponent<SpriteRenderer>().enabled = true;
        while (cover.color.a > 0)
        {
            cover.color = new Color(cover.color.r, cover.color.g, cover.color.b, Mathf.Max(cover.color.a - Time.deltaTime, 0));
            yield return null;
        }
    }
    public void GameOver()
    {
        StartCoroutine(startOver());
    }
    public void BackToGame()
    {
        currentCanvas = null;
        nextCanvas = null;
        Blur.SetActive(false);
        InGameMenu.enabled = false;
        isInGameMenu = false;
        Time.timeScale = previousTime;
        eventSystem.SetSelectedGameObject(null);
    }
    public void InGameOption()
    {
        Blur.SetActive(true);
        InGameMenu.enabled = true;
        isInGameMenu = true;
        previousTime = Time.timeScale;
        Time.timeScale = 0;
    }
    public void Option()
    {
        if (SceneManagered.instance.isInGame)
        {
            StartCoroutine(Fading(InGameMenu, OptionCanvas));
        }
        else
        {
            StartCoroutine(Fading(MenuCanvas, OptionCanvas));
        }
    }
    public void BackOption()
    {
        if (SceneManagered.instance.isInGame)
        {
            StartCoroutine(Fading(OptionCanvas, InGameMenu));
        }
        else
        {
            StartCoroutine(Fading(OptionCanvas, MenuCanvas));
        }
    }
    public void AudioOption()
    {
        StartCoroutine(Fading(OptionCanvas, AudioOptionCanvas));
        currentOptionCanvas = AudioOptionCanvas;
    }
    public void BackChildOption()
    {
        StartCoroutine(Fading(currentOptionCanvas, OptionCanvas));
        currentOptionCanvas = null;
    }
    public void KeyBoardOption()
    {
        StartCoroutine(Fading(OptionCanvas, KeyboardCanvas));
        currentOptionCanvas = KeyboardCanvas;
    }
    public void VideoOption()
    {
        StartCoroutine(Fading(OptionCanvas, VideoCanvas));
        currentOptionCanvas = VideoCanvas;
    }
    public void QuitGame()
    {
        cameraMat.SetFloat("_IsInProgress", 0);
        cameraMat.SetFloat("_Progress", 0);
        Application.Quit();
    }
    IEnumerator Fading(Canvas currentcanvas, Canvas nextcanvas)
    {
        gameInput.Disable();
        currentCanvas = currentcanvas;
        nextCanvas = nextcanvas;

        CanvasGroup currentCanvasGroup = currentCanvas.GetComponent<CanvasGroup>();
        currentCanvasGroup.interactable = false;

        CanvasGroup nextCanvasGroup = nextcanvas.GetComponent<CanvasGroup>();
        nextCanvasGroup.alpha = 0;
        nextCanvasGroup.interactable = false;
        nextCanvas.enabled = true;
        while(currentCanvasGroup.alpha > 0)
        {
            currentCanvasGroup.alpha =  Mathf.Max(currentCanvasGroup.alpha - Time.unscaledDeltaTime / FadeDelay, 0);
            yield return null;
        }
        currentCanvas.enabled = false;
        while (nextCanvasGroup.alpha < 1) {
            nextCanvasGroup.alpha = Mathf.Min(nextCanvasGroup.alpha + Time.unscaledDeltaTime * 2 / FadeDelay);
            yield return null;
        }
        nextCanvasGroup.interactable = true;
        currentCanvasGroup.alpha = 1;
        currentCanvasGroup.interactable = true;
        gameInput.Enable();
    }
}
