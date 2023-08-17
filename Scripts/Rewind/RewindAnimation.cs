using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindAnimation : MonoBehaviour
{
    private RewindRecorder rewindRecorder;
    public List<Animator> environment;
    void Awake()
    {
        rewindRecorder = GameObject.FindGameObjectWithTag("Player").GetComponent<RewindRecorder>();
        foreach(Transform child in transform)
        {
            if (child.GetComponent<Animator>() != null)
            {
                environment.Add(child.GetComponent<Animator>());
            }
        }
    }
    private void Start()
    {
        rewindRecorder.startRewind += () =>
         {
             foreach (Animator anim in environment)
             {

                 anim.SetFloat("Speed", -1);
             }
         };

        rewindRecorder.endRewind += () =>
        {
            foreach (Animator anim in environment)
            {
                anim.SetFloat("Speed", 1);
            }
        };
    }
}
