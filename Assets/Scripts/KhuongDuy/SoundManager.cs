using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource
        music,
        buttonClick,
        referee,
        jump,
        ballHit,
        ballKick,
        crossBarHit,
        goal,
        lockCharacter,
        matchWon,
        matchLost;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(this.gameObject);
        }
    }
}
