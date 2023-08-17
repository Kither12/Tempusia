using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectURewind : MonoBehaviour
{
    public Material mat;
    private Material defaultMat;
    private GameObject player;
    private RewindRecorder rewindRecorder;
    public Stack<RewindData> backwardData;
    public Stack<RewindData> forwardData;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        backwardData = new Stack<RewindData>();
        forwardData = new Stack<RewindData>();
        rewindRecorder = player.GetComponent<RewindRecorder>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        defaultMat = GetComponent<SpriteRenderer>().material;
    }
    private void Start()
    {
        rewindRecorder.startRewind += () =>
        {
            GetComponent<SpriteRenderer>().material = mat;
        };
    }
    private void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        if (!rewindRecorder.isRecorded)
        {
            if (forwardData.Count == 0)
            {
                backwardData.Clear();
                GetComponent<SpriteRenderer>().material = defaultMat;
                return;
            }
            RewindData newData = forwardData.Pop();
            gameObject.transform.parent = newData.parent;
            gameObject.transform.position = newData.position;
            gameObject.transform.rotation = newData.rotation;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0;
            backwardData.Push(newData);
        }
        else
        {
            if (backwardData.Count > 0)
            {
                RewindData newData = backwardData.Pop();
                forwardData.Push(newData);
                gameObject.transform.parent = newData.parent;
                gameObject.transform.position = newData.position;
                gameObject.transform.rotation = newData.rotation;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = 0;
            }
            else
            {
                RewindData newData = new RewindData();
                newData.parent = gameObject.transform.parent;
                newData.position = gameObject.transform.position;
                newData.rotation = gameObject.transform.rotation;
                forwardData.Push(newData);
            }
        }
    }
}
