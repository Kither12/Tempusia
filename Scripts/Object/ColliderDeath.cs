using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDeath : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.isTrigger == false)
        {
            collision.GetComponent<PlayerController>().Dead();
        }
    }
}
