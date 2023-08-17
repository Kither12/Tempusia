using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    public GameObject[] buttons;
    public EventSystem eventSystem;
    public AudioClip pointerAudio;
    public Canvas canvas;
    private bool currentSelectedButton;
    private bool ignoreNextExit;
    private PlayerInput input;
    private void Start()
    {
        input = new PlayerInput();
        input.Enable();
        input.normal.UpArrow.performed += (_) =>
        {
            if (GetComponent<Canvas>().enabled)
            {
                if(!currentSelectedButton)
                    eventSystem.SetSelectedGameObject(buttons[0]);
                currentSelectedButton = true;
                eventSystem.currentSelectedGameObject.GetComponent<AudioSource>().PlayOneShot(pointerAudio);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                ignoreNextExit = true;
            }
        };
        input.normal.DownArrow.performed += (_) =>
        {
            if (GetComponent<Canvas>().enabled)
            {
                if (!currentSelectedButton)
                    eventSystem.SetSelectedGameObject(buttons[buttons.Length - 1]);
                currentSelectedButton = true;
                eventSystem.currentSelectedGameObject.GetComponent<AudioSource>().PlayOneShot(pointerAudio);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                ignoreNextExit = true;
            }
        };
    }
    public void PointerEnter(GameObject gameObject)
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(pointerAudio);
        eventSystem.SetSelectedGameObject(gameObject);
        currentSelectedButton = true;
    }
    public void PointerExit()
    {
        if (!ignoreNextExit)
        {
            eventSystem.SetSelectedGameObject(null);
            currentSelectedButton = false;
        }
        else
        {
            ignoreNextExit = false;
        }
    }
    private void Update()
    {
        if(canvas.enabled == false)
        {
            currentSelectedButton = false;
        }
        else if(eventSystem.currentSelectedGameObject == null)
        {
            currentSelectedButton = false;
        }
    }
}
