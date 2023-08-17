using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Elevator : MonoBehaviourID, IData
{
    public float delayTime;
    public float speed;
    public bool canBeRewinded;
    public Material rewindMat;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public AudioSource audioSource;

    private Vector3 currentPos;
    private Vector3 movePos;
    private bool isMoveForward;
    private float currentTime;
    private RewindRecorder rewindRecorder;
    private Stack<RewindData> backwardData;
    private Stack<RewindData> forwardData;
    private GameObject player;
    private Material defaultMat;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentPos = transform.position;
        movePos = transform.GetChild(0).position;
        rewindRecorder = player.GetComponent<RewindRecorder>();
    }
    private void Start()
    {
        if (transform.parent != null)
        {
            transform.parent = null;
        }
        currentTime = delayTime;
        backwardData = new Stack<RewindData>();
        forwardData = new Stack<RewindData>();
        if (DataManager.instance.hasData)
        {
            LoadData(DataManager.instance.gameData);
        }
        defaultMat = spriteRenderer.material;
    }
    void Update()
    {
        if (Time.timeScale == 0)
        {
            return;
        }
        if (canBeRewinded)
        {
            if (!rewindRecorder.isRecorded)
            {
                if (forwardData.Count == 0)
                {
                    Move(speed);
                    backwardData.Clear();
                    spriteRenderer.material = defaultMat;
                    return;
                }
                RewindData newData = forwardData.Pop();
                gameObject.transform.position = newData.position;
                gameObject.transform.localScale = newData.scale;
                currentTime = newData.elevatorCurrentTime;
                isMoveForward = newData.elevatorIsMoveForward;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = 0;
                backwardData.Push(newData);
                spriteRenderer.material = rewindMat;
            }
            else
            {
                spriteRenderer.material = rewindMat;
                if (backwardData.Count > 0)
                {
                    RewindData newData = backwardData.Pop();
                    forwardData.Push(newData);
                    gameObject.transform.position = newData.position;
                    gameObject.transform.localScale = newData.scale;
                    currentTime = newData.elevatorCurrentTime;
                    isMoveForward = newData.elevatorIsMoveForward;
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = 0;
                }
                else
                {
                    Move(speed);
                    RewindData newData = new RewindData();
                    newData.position = gameObject.transform.position;
                    newData.scale = gameObject.transform.localScale;
                    newData.elevatorCurrentTime = currentTime;
                    newData.elevatorIsMoveForward = isMoveForward;
                    forwardData.Push(newData);
                }
            }
        }
        else {
            Move(speed);
        }
    }
    private void Move(float speed)
    {
        if (!isMoveForward && currentTime <= 0)
        {
            if (transform.position != movePos)
            {
                transform.position = Vector3.MoveTowards(transform.position, movePos, speed*Time.deltaTime);
            }
            else
            {
                isMoveForward = true;
                currentTime = delayTime;
            }
        }
        else if (isMoveForward && currentTime <= 0)
        {
            if (transform.position != currentPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentPos, speed * Time.deltaTime);
            }
            else
            {
                isMoveForward = false;
                currentTime = delayTime;
            }
            
        }
        else if (currentTime > 0)
        {
            GetComponent<AudioSource>().Stop();
            currentTime -= Time.deltaTime;
        }
    }

    public void LoadData(GameData data)
    {
        if (!string.IsNullOrEmpty(ID))
        {
            ElevatorData elevatorData;
            data.elevatorData.TryGetValue(ID, out elevatorData);
            transform.position = elevatorData.position;
            isMoveForward = elevatorData.isMoveForward;
            backwardData.Clear();
            forwardData.Clear();
        }
    }

    public void SaveData(GameData data)
    {
        if (!string.IsNullOrEmpty(ID))
        {
            if (data.elevatorData.ContainsKey(ID))
            {
                data.elevatorData.Remove(ID);
            }
            ElevatorData elevatorData = new ElevatorData();
            elevatorData.position = transform.position;
            elevatorData.isMoveForward = isMoveForward;
            data.elevatorData.Add(ID, elevatorData);
        }
    }
    private void OnDisable()
    {
        SaveData(DataManager.instance.gameData);
    }
}
