using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject Parent;
    public SpriteRenderer parentRoundLight;
    public GameObject Child;
    public GameObject Light;
    public SpriteRenderer roundLight;
    public GameObject pressurePad;
    public List<SpriteRenderer> pressurePadLight;
    public List<SpriteRenderer> lightColor;
    public GameObject effectPrefabs;
    public bool isParent;

    void Awake()
    {
        roundLight.color = Color.red;
        if (transform.parent != null)
        {
            if (transform.parent.CompareTag("Portal")){
                isParent = false;
            }
            else
            {
                transform.parent = null;
                isParent = true;
            }
        }
        if (isParent)
        {
            foreach (Transform child in Light.transform)
            {
                lightColor.Add(child.GetComponent<SpriteRenderer>());
            }
            foreach (Transform child in pressurePad.transform)
            {
                pressurePadLight.Add(child.GetChild(0).GetComponent<SpriteRenderer>());
            }
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Portal"))
                {
                    Child = child.gameObject;
                }
            }
        }
        else
        {
            Parent = transform.parent.gameObject;
            parentRoundLight = Parent.GetComponent<Portal>().roundLight;
        }
    }
    // Update is called once per frame
    private void Update()
    {
        if (isParent)
        {
            if (roundLight.color != Color.green)
            {
                LightCheck();
                if (LightAllGreen())
                {
                    roundLight.color = Color.green;
                }
            }
        }else if(parentRoundLight.color == Color.green)
        {
            roundLight.color = Color.green;
        }
    }
    public bool LightAllGreen()
    {
        foreach (SpriteRenderer item in lightColor)
        {
            if (item.color == Color.red)
            {
                return false;
            }
        }
        return true;
    }
    private void LightCheck()
    {
        for (int i = 0; i < pressurePadLight.Count; i++)
        {
            if (pressurePadLight[i].color == Color.green)
            {
                lightColor[i].color = Color.green;
            }
            else
            {
                lightColor[i].color = Color.red;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isParent && roundLight.color == Color.green && collision.CompareTag("Player"))
        {
            StartCoroutine(createEffect(collision.gameObject));
            collision.transform.position = Child.transform.position + new Vector3(0f, 0.5f, 0f);
        }
    }
    IEnumerator createEffect(GameObject player)
    {
        GameObject prefabs = Instantiate(effectPrefabs, player.transform.position, Quaternion.identity);
        prefabs.GetComponent<SpriteRenderer>().sprite = player.GetComponent<SpriteRenderer>().sprite;
        prefabs.transform.localScale = player.transform.localScale;
        Material mat = prefabs.GetComponent<SpriteRenderer>().material;
        while (mat.GetFloat("Fade") != 0)
        {
            mat.SetFloat("Fade", Mathf.Max(mat.GetFloat("Fade") - Time.deltaTime, 0));
            yield return null;
        }
        prefabs.GetComponent<SpriteRenderer>().sprite = null;
        Destroy(prefabs);
    }
}
