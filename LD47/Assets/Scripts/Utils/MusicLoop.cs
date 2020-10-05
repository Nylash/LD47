using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicLoop : MonoBehaviour
{
    public static MusicLoop instance;

    private AudioSource audioSource;
    [SerializeField] private AudioClip StartMusic = null;
    [SerializeField] private AudioClip LoopMusic = null;
    private bool loopIsPlaying = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(StartMusic);
    }

    private void Update()
    {
        if (!loopIsPlaying)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = LoopMusic;
                audioSource.loop = true;
                loopIsPlaying = true;
            }
        }
    }
}
