using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePad : MonoBehaviourID, IData
{
    public Portal portal;
    public SpriteRenderer Light;
    public LayerMask[] objectLayer;
    public float maxTime;
    private float currentTime;
    private bool check;
    // Start is called before the first frame update
    void Awake()
    {
        portal = transform.parent.parent.GetComponent<Portal>();
        Light = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Light.color = Color.red;
    }
    private void Start()
    {
        if (DataManager.instance.hasData)
        {
            LoadData(DataManager.instance.gameData);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (portal.roundLight.color != Color.green)
        {
            if (currentTime > 0 && !check)
            {
                currentTime -= Time.deltaTime;
            }
            else if(currentTime <=0)
            {
                Light.color = Color.red;
            }
        }
        else
        {
            Light.color = Color.green;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            Light.color = Color.green;
            currentTime = maxTime;
            check = true;
        }
        else
        {
            foreach(LayerMask layer in objectLayer)
            {
                if (IsInLayerMask(collision.gameObject, layer))
                {
                    Light.color = Color.green;
                    currentTime = maxTime;
                    check = true;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        check = false;
    }
    public bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }

    public void LoadData(GameData data)
    {
        if (!string.IsNullOrEmpty(ID))
        {
            PortalData isGreen;
            data.portalData.TryGetValue(ID, out isGreen);
            Light.color = isGreen.portalColor;
            if(isGreen.portalColor == Color.green)
            {
                currentTime = 1e9f;
            }
        }
    }

    public void SaveData(GameData data)
    {
        if (!string.IsNullOrEmpty(ID))
        {
            if (data.portalData.ContainsKey(ID))
            {
                data.portalData.Remove(ID);
            }
            PortalData portal = new PortalData();
            portal.portalColor = Light.color;
            data.portalData.Add(ID, portal);
        }
    }
}
