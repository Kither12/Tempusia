using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = mainCamera.aspect * cameraHeight;

        float imgHeight = spriteRenderer.sprite.bounds.size.y;
        float imgWidth = spriteRenderer.sprite.bounds.size.x;

        gameObject.transform.localScale = new Vector3(cameraWidth / imgWidth, cameraHeight / imgHeight, 0);
    }
}