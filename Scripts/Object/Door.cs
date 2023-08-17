using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject Button;
    public GameObject Light;
    public Animator anim;

    [HideInInspector]public List<SpriteRenderer> buttonLightColor;
    [HideInInspector]public List<SpriteRenderer> doorLightColor;
    public float animationCurrentTime;
    public Color defaultColor;
    private void Start()
    {
        foreach (Transform child in Button.transform)
        {
            buttonLightColor.Add(child.GetChild(0).gameObject.GetComponent<SpriteRenderer>());
            child.GetComponent<Button>().defaultColor = defaultColor;
            child.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = defaultColor;
            child.GetComponent<Button>().door = this;
        }
        foreach (Transform child in Light.transform)
        {
            doorLightColor.Add(child.gameObject.GetComponent<SpriteRenderer>());
            child.gameObject.GetComponent<SpriteRenderer>().color = defaultColor;
        }
    }
    // Update is called once per frame
    
    public void Check()
    {
        LightCheck();
        if (LightAllGreen())
        {
            anim.SetBool("isActive", true);
        }
        else
        {
            anim.SetBool("isActive", false);
        }
    }
    public bool LightAllGreen()
    {
        foreach(SpriteRenderer item in buttonLightColor)
        {
            if(item.color == defaultColor)
            {
                return false;
            }
        }
        return true;
    }
    private void LightCheck()
    {
        for(int i = 0; i< buttonLightColor.Count; i++)
        {
            if(buttonLightColor[i].color == Color.green)
            {
                doorLightColor[i].color = Color.green;
            }
            else
            {
                doorLightColor[i].color = defaultColor;
            }
        }
    }
}
