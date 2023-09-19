using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip levelUpSound;
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Hit()
    {
        audioSource.PlayOneShot(hitSound);
    }

    public void LevelUp()
    {
        audioSource.PlayOneShot(levelUpSound);
    }
}
