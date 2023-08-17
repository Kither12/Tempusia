using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResolutionUX : MonoBehaviour
{
    public PlayerInput input;
    public GraphicsManager graphicsManager;
    public EventSystem eventSystem;
    private void Start()
    {
        input = new PlayerInput();
        input.Enable();

        input.normal.LeftArrow.performed += (_) =>
        {
            if(eventSystem.currentSelectedGameObject == gameObject)
                graphicsManager.ResLeft();
        };
        input.normal.RightArrow.performed += (_) =>
        {
            if (eventSystem.currentSelectedGameObject == gameObject)
                graphicsManager.ResRight();
        };
    }

}
