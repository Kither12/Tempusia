using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public bool isUI;
    public Transform follow;
    public Vector3 offSet;
    private void Update()
    {
        transform.position = follow.position + offSet;
    }
}
