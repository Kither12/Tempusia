using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RewindData 
{
    public List<GameObject> Button;
    public Vector2 Velocity;
    public Vector3 position;
    public Vector3 scale;
    public Quaternion rotation;
    public Sprite sprite;
    public float elevatorCurrentTime;
    public bool elevatorIsMoveForward;
    public float elevatorSpeed;
    public bool flipX;
    public Transform parent;
    public void reset()
    {
        Button = null;
        Velocity = Vector2.zero;
        position = Vector3.zero;
        scale = Vector3.zero;
        rotation = Quaternion.identity;
        sprite = null;
        elevatorCurrentTime = 0;
        elevatorIsMoveForward = false;
        elevatorSpeed = 0;
        flipX = false;
        parent = null;
    }
}
