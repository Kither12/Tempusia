using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviourID, IData
{
    public bool canOptimize;

    private bool areOutSide;
    private BoxCollider2D boxCollider;
    private Rigidbody2D body;
    private AudioSource audioSource;
    private LayerMask layerGround;
    private Vector3 scale;
    private float lateVelocityY;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
        scale = transform.localScale;
    }
    private void Start()
    {
        layerGround = GameManager.instance.layerGround;
        if (DataManager.instance.hasData)
        {
            LoadData(DataManager.instance.gameData);
        }
    }
    private void OnBecameVisible()
    {
        if (canOptimize) areOutSide = false;
    }
    private void OnBecameInvisible()
    {
        if (canOptimize) areOutSide = true;
    }
    private void Update()
    {
        if (!areOutSide)
        {
            CheckInElevator();
            if(body.velocity.y - lateVelocityY > 15)
            {
                audioSource.Play();
            }
            lateVelocityY = body.velocity.y;
        }
    }
    private void CheckInElevator()
    {
        RaycastHit2D[] raycastHitElevator = Physics2D.BoxCastAll(boxCollider.bounds.center, new Vector3(boxCollider.bounds.size.x / 3f, boxCollider.bounds.size.y, 0f), 0f, Vector2.down, 0.3f);
        if (raycastHitElevator.Length > 0)
        {
            transform.parent = null;
            foreach (RaycastHit2D Hit in raycastHitElevator)
            {
                if (Hit.transform.root.CompareTag("Elevator"))
                {
                    Round();
                    if (body.angularVelocity == 0f)
                    {
                        transform.parent = Hit.transform.root;
                    }
                    break;
                }
                else
                {
                    transform.localScale = scale;
                }
            }
        }
    }
    void Round()
    {
        if(Mathf.Abs(body.angularVelocity) < 8)
        {
            body.angularVelocity = 0;
        }
    }

    public void LoadData(GameData data)
    {
        if (!string.IsNullOrEmpty(ID))
        {
            BoxData boxData;
            data.boxData.TryGetValue(ID, out boxData);
            transform.position = boxData.position;
            transform.rotation = boxData.rotation;
            transform.localScale = boxData.scale;
            if (!boxData.hasParent)
            {
                transform.parent = null;
            }
            if(GetComponent<ObjectURewind>() != null)
            {
                GetComponent<ObjectURewind>().backwardData.Clear();
                GetComponent<ObjectURewind>().forwardData.Clear();
            }
        }

    }

    public void SaveData(GameData data)
    {
        if (!string.IsNullOrEmpty(ID))
        {
            if (data.boxData.ContainsKey(ID))
            {
                data.boxData.Remove(ID);
            }
            BoxData boxData = new BoxData();
            boxData.position = transform.position;
            boxData.rotation = transform.rotation;
            boxData.scale = transform.localScale;
            boxData.hasParent = (transform.parent != null);
            data.boxData.Add(ID, boxData);
        }
    }
    private void OnDisable()
    {
        SaveData(DataManager.instance.gameData);
    }
}
