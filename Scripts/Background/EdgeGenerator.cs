using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeGenator : MonoBehaviour
{
    private Camera mainCamera;
    public float boundWidth;
    public bool triggerLeft, triggerRight, triggerUp, triggerDown;
    public bool Right, Left, Up, Down;
    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    private void Start()
    {
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = mainCamera.aspect * cameraHeight;

        if (Left)
        {
            BoxCollider2D boxLeft = gameObject.AddComponent<BoxCollider2D>();
            boxLeft.size = new Vector2(boundWidth, cameraHeight);
            boxLeft.offset = new Vector2(-(cameraWidth + boundWidth) / 2, 0);
            boxLeft.isTrigger = triggerLeft;
        }
        if (Right)
        {
            BoxCollider2D boxRight = gameObject.AddComponent<BoxCollider2D>();
            boxRight.size = new Vector2(boundWidth, cameraHeight);
            boxRight.offset = new Vector2((cameraWidth + boundWidth) / 2, 0);
            boxRight.isTrigger = triggerRight;
        }
        if (Up)
        {
            BoxCollider2D boxUp = gameObject.AddComponent<BoxCollider2D>();
            boxUp.size = new Vector2(cameraWidth, boundWidth);
            boxUp.offset = new Vector2(0, (cameraHeight + boundWidth) / 2);
            boxUp.isTrigger = triggerUp;
        }
        if (Down)
        {
            BoxCollider2D boxDown = gameObject.AddComponent<BoxCollider2D>();
            boxDown.size = new Vector2(cameraWidth, boundWidth);
            boxDown.offset = new Vector2(0, -(cameraHeight + boundWidth) / 2);
            boxDown.isTrigger = triggerDown;
        }
    }
}
