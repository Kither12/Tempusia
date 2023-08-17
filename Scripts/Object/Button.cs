using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    [HideInInspector]public bool check;
    protected RewindRecorder rewindRecorder;
    public float maxTime;
    private PlayerInput input;
    protected SpriteRenderer lightColor;
    [HideInInspector]public bool canInteract;
    protected AudioSource audioSource;
    [HideInInspector]public Color defaultColor;
    [HideInInspector] public Door door;
    // Start is called before the first frame update
    void Awake()
    {
        rewindRecorder = GameObject.FindGameObjectWithTag("Player").GetComponent<RewindRecorder>();
        lightColor = transform.GetChild(0).GetComponent<SpriteRenderer>();
        input = KeyboardManager.input;
        audioSource = gameObject.GetComponent<AudioSource>();
    }
    public virtual void Start()
    {
        input.normal.Interact.performed += (_) =>
        {
            if(canInteract && !check)
            {
                Trigger(true);
            }
        };
    }
    private void OnDisable()
    {
        input.Disable();
    }
    private void OnEnable()
    {
        input.Enable();
    }
    public virtual void Trigger(bool save)
    {
        lightColor.color = Color.green;
        audioSource.Play();
        check = true;
        door.Check();
        if (rewindRecorder.recordedData.Count > 0 && save)
        {
            RewindData data = rewindRecorder.recordedData.Pop();
            if (data.Button == null)
            {
                data.Button = new List<GameObject>();
            }
            data.Button.Add(gameObject);
            rewindRecorder.recordedData.Push(data);
        }
        StartCoroutine(delay());
    }
    private IEnumerator delay()
    {
        yield return new WaitForSeconds(maxTime);
        lightColor.color = defaultColor;
        check = false;
        door.Check();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canInteract = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canInteract = false;
        }
    }
}
