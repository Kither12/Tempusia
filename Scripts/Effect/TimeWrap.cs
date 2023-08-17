using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeWrap : MonoBehaviour
{
    public bool isScaled;
    private SpriteRenderer spriteRenderer;
    private Image img;
    private Material mat;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        img = GetComponent<Image>();
    }
    private void Update()
    {
        if (spriteRenderer != null)
            mat = spriteRenderer.material;
        if (img != null)
            mat = img.material;
        if (isScaled)
        {
            mat.SetFloat("UnscaledTime", Time.unscaledTime);
        }
        else
        {
            mat.SetFloat("UnscaledTime", Time.time);
        }
    }
}
