using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !DataManager.instance.gameData.isEndGame)
        {
            DataManager.instance.gameData.isEndGame = true;
            SceneManagered.instance.CreditScene();
        }
    }
}
