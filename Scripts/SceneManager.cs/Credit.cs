using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : MonoBehaviour
{
    private PlayerInput input;
    public float delayTime;
    private bool canReturn;
    private void Start()
    {
        input = new PlayerInput();
        input.Enable();
        StartCoroutine(WaitToReturn());
        input.normal.AnyKeys.performed += (_) =>
        {
            if (canReturn)
            {
                SceneManagered.instance.ReturnToMenu(false);
                canReturn = false;
            }
        };
    }
    IEnumerator WaitToReturn()
    {
        yield return new WaitForSeconds(delayTime);
        canReturn = true;
    }
}
