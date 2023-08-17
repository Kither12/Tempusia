using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonDestroyOnLoad : MonoBehaviour
{
    private void Awake()
    {
        GameObject tempGameObject = GameObject.Find(gameObject.name);
        if (tempGameObject != gameObject)
        {
            if (tempGameObject.GetComponent<Canvas>() != null) tempGameObject.GetComponent<Canvas>().worldCamera = Camera.main;
            Destroy(gameObject);
        }
        else { 
            DontDestroyOnLoad(gameObject);
        }
    }
}
