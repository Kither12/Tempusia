using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOutsideRender : MonoBehaviour
{
    private void OnBecameVisible()
    {
        GetComponent<Animator>().enabled = true;
    }
    private void OnBecameInvisible()
    {
        GetComponent<Animator>().enabled = false;
    }
}