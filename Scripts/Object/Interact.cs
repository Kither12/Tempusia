using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    private PlayerInput input;
    private bool isCollapse;
    public Canvas interactCanvas;
    public GameObject blur;
    
    private void Start()
    {
        input = KeyboardManager.input;
        input.Enable();
        input.normal.Interact.performed += EnableCanvas;
        input.normal.Exit.performed += DisableCanvas;
    }
    private void OnDisable()
    {
        input.normal.Interact.performed -= EnableCanvas;
        input.normal.Exit.performed -= DisableCanvas;
    }
    public void EnableCanvas(UnityEngine.InputSystem.InputAction.CallbackContext e)
    {
        if (SceneManagered.instance.isInGame)
        {
            if (!GameManager.instance.isInGameMenu)
            {
                if (interactCanvas.enabled)
                {
                    blur.SetActive(false);
                    interactCanvas.enabled = false;
                    Time.timeScale = 1;
                    GameManager.instance.isShowingInteractCanvas = false;
                }
                else
                {
                    if (isCollapse)
                    {
                        blur.SetActive(true);
                        interactCanvas.enabled = true;
                        Time.timeScale = 0;
                        GameManager.instance.isShowingInteractCanvas = true;
                    }
                }
            }
        }
    }
    public void DisableCanvas(UnityEngine.InputSystem.InputAction.CallbackContext e)
    {
        if (SceneManagered.instance.isInGame)
        {
            if (interactCanvas.enabled)
            {
                blur.SetActive(false);
                interactCanvas.enabled = false;
                Time.timeScale = 1;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollapse = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isCollapse = false;
        }
    }
}
