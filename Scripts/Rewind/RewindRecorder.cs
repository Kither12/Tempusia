using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class RewindRecorder : MonoBehaviour
{
    [HideInInspector] public bool isRecorded;
    [HideInInspector] public Stack<RewindData> recordedData;
    [HideInInspector] public float startTime;
    [HideInInspector] public Vector3 cloneScale;
    public AudioSource audioSource;
    public AudioClip rewindAudioClip;
    public float maxTime;
    private SpriteRenderer spriteRenderer;

    public delegate void rewindActions();
    public rewindActions startRewind, endRewind;

    private PlayerInput input;
    public Transform[] allParent;

    public Renderer2DData renderer;
    private int assignSpeed;
    private Blit blit;
    private void OnEnable()
    {
        input.Enable();
    }
    private void OnDisable()
    {
        input.Disable();
        startRewind += StartRewind;
        endRewind += EndRewind;
        input.normal.Rewind.performed += RewindPerform;
    }
    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        input = KeyboardManager.input;
    }
    private void Start()
    {
        isRecorded = false;
        recordedData = new Stack<RewindData>();
        blit = renderer.rendererFeatures[renderer.rendererFeatures.Count - 1] as Blit;
        blit.settings.blitMaterial.SetFloat("_IsInProgress", 0);
        blit.settings.blitMaterial.SetFloat("_Progress", 0);
        startRewind += StartRewind;
        endRewind += EndRewind;
        input.normal.Rewind.performed += RewindPerform;
    }
    private void RewindPerform(InputAction.CallbackContext e)
    {
        if (Time.timeScale == 1)
        {
            if (isRecorded == false && blit.settings.blitMaterial.GetFloat("_Progress") == 0)
            {
                audioSource.PlayOneShot(rewindAudioClip);
                startRewind();
            }
            else if (isRecorded == true && blit.settings.blitMaterial.GetFloat("_Progress") == 1)
            {
                endRewind();
            }
        }
    }
    private void StartRewind()
    {
        StartCoroutine("record", record());
        isRecorded = true;
        assignSpeed = 1;
        blit.settings.blitMaterial.SetFloat("_Magnification", Mathf.Abs(blit.settings.blitMaterial.GetFloat("_Magnification")));
        blit.settings.blitMaterial.SetFloat("_IsInProgress", 1);
    }
    private void EndRewind()
    {
        StopCoroutine("record");
        isRecorded = false;
        blit.settings.blitMaterial.SetFloat("_Magnification", -Mathf.Abs(blit.settings.blitMaterial.GetFloat("_Magnification")));
        assignSpeed = -1;
    }
    private void Update()
    {
        if (transform.parent == null)
        {
            cloneScale = transform.localScale;
        }
        else
        {
            cloneScale = new Vector3(transform.parent.localScale.x * transform.localScale.x, transform.parent.localScale.y * transform.localScale.y, 0f);
        }
        if(blit.settings.blitMaterial.GetFloat("_IsInProgress") == 1)
        {
            blit.settings.blitMaterial.SetFloat("_Progress", Mathf.Clamp01(blit.settings.blitMaterial.GetFloat("_Progress") + assignSpeed * Time.deltaTime * 2.5f));
            if(blit.settings.blitMaterial.GetFloat("_Progress") == 0)
            {
                blit.settings.blitMaterial.SetFloat("_IsInProgress", 0);
            }
        }
        if (isRecorded && Time.timeScale != 0)
        {
            RewindData newData = new RewindData();
            newData.position = transform.position;
            newData.scale = cloneScale;
            newData.sprite = spriteRenderer.sprite;
            newData.rotation = transform.rotation;
            newData.flipX = spriteRenderer.flipX;
            recordedData.Push(newData);
        }
    }
    IEnumerator record()
    {
        yield return new WaitForSeconds(maxTime);
        endRewind();
    }
}
