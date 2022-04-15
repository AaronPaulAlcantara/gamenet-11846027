using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    static AudioSource audioRef;
    public AudioClip Ez;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        audioRef = GetComponent<AudioSource>();

    }

    public void playMusic()
    {
        audioRef.PlayOneShot(Ez);
    }
}
