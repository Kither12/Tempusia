using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startpos, fixedY;
    private GameObject cam;

    public float parallexEffect;
    
    private void Awake()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera1");
    }
    void Start()
    {
        gameObject.transform.parent = cam.transform;
        startpos = transform.position.x;
        fixedY = transform.position.y;
    }
    void LateUpdate()
    {
        float dist = (cam.transform.position.x * parallexEffect);
        transform.position = new Vector3(startpos + dist, fixedY, transform.position.z);
    }
}
