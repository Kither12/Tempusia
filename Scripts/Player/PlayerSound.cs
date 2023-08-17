using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Grass,
    Wooden,
    Metal
}
public class PlayerSound : MonoBehaviour
{
    [HideInInspector]
    public AudioClip currentWalkingSound;
    [HideInInspector]
    public AudioClip currentJumpingSound;
    public AudioClip[] jumpClip;
    public AudioClip[] walkClip;
}
