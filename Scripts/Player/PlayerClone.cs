using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerClone : MonoBehaviour
{
    private RewindRecorder rewindRecorder;

    private PlayerCloneManager cloneManager;
    private SpriteRenderer spriteRenderer;

    private Stack<RewindData> forwardData;
    private Stack<RewindData> backwardData;

    private Material mat;
    private float appearTime;
    private void Awake()
    {
        cloneManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCloneManager>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rewindRecorder = GameObject.FindGameObjectWithTag("Player").GetComponent<RewindRecorder>();
    }
    private void Start()
    {
        mat = spriteRenderer.material;
        mat.SetFloat("Fade", 1);
    }
    public void rewind(Stack<RewindData> data)
    {
        RewindData[] arr = new RewindData[data.Count];
        data.CopyTo(arr, 0);
        Array.Reverse(arr);
        forwardData = new Stack<RewindData>(arr);
        backwardData = new Stack<RewindData>();
        appearTime = forwardData.Count * Time.fixedDeltaTime;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
    }
    private void Update()
    {
        if(Time.timeScale == 0)
        {
            return;
        }
        if (!rewindRecorder.isRecorded)
        {
            if (forwardData.Count == 0)
            {
                deletedClone();
                backwardData.Clear();
                return;
            }
            RewindData newData = forwardData.Pop();
            gameObject.transform.position = newData.position;
            gameObject.transform.localScale = newData.scale;
            spriteRenderer.flipX = newData.flipX ;
            spriteRenderer.sprite = newData.sprite;
            if (newData.Button != null)
            {
                foreach(GameObject button in newData.Button)
                {
                    button.GetComponent<Button>().Trigger(false);
                }
            }
            backwardData.Push(newData);
        }
        else
        {
            if (backwardData.Count > 0)
            {
                RewindData newData = backwardData.Pop();
                forwardData.Push(newData);
                gameObject.transform.position = newData.position;
                gameObject.transform.localScale = newData.scale;
                spriteRenderer.flipX = newData.flipX;
                if (newData.Button != null)
                {
                    foreach (GameObject button in newData.Button)
                    {
                        button.GetComponent<Button>().Trigger(false);
                    }
                }
                spriteRenderer.sprite = newData.sprite;
            }
            else
            {
                deletedClone();
                forwardData.Clear();
            }
        }
    }
    IEnumerator disolve()
    {
        transform.parent = null;
        while (mat.GetFloat("Fade") != 0) {
            mat.SetFloat("Fade", Mathf.Max(mat.GetFloat("Fade") - Time.deltaTime, 0));
            yield return null;
        }
        Destroy(gameObject);
    }
    public void deletedClone()
    {
        if (spriteRenderer.sprite != null && mat.GetFloat("Fade") == 1)
        {
            StartCoroutine(disolve());
        }
    }
}
