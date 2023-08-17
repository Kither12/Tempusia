using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorGroup : MonoBehaviour
{
    public Elevator[] elevators;
    private bool areDisable;

    private void Start()
    {
        StartCoroutine(firstUpdate());
    }
    private IEnumerator firstUpdate()
    {
        yield return null;
        DisableAllElevator();
    }
    private void DisableAllElevator()
    {
        foreach(Elevator elevator in elevators)
        {
            elevator.enabled = false;
        }
        areDisable = true;
    }
    private void EnableAllElevator()
    {
        foreach (Elevator elevator in elevators)
        {
            elevator.enabled = true;
        }
        areDisable = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MainCamera1") && areDisable)
        {
            EnableAllElevator();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("MainCamera1") && areDisable)
        {
            EnableAllElevator();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("MainCamera1") && !areDisable)
        {
            DisableAllElevator();
        }
    }
    
}
