﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager instance;


    [System.Serializable]
    public class Sound
    {
        public SoundName name;
        public List<AudioClip> clips = new List<AudioClip>();
        [Range(0, 1)] public float volume = 1;
        [Range(-3, 3)] public List<float> pitchs = new List<float>();
    }

    [ArrayElementTitle("name")]
    public List<Sound> sounds = new List<Sound>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void PlaySoundLoop(SoundName soundName, AudioSource source)
    {
        int index = GetIndex(soundName);
        if (index == sounds.Count)
        {
            Debug.LogWarning("There is no sound with this name " + soundName + " on SoundsManger, please verify your typing.");
            return;
        }
        source.pitch = sounds[index].pitchs[Random.Range(0, sounds[index].pitchs.Count)];
        source.volume = sounds[index].volume;
        source.clip = sounds[index].clips[Random.Range(0, sounds[index].clips.Count)];
        source.loop = true;
    }

    public void PlaySoundOneShot(SoundName soundName, AudioSource source)
    {
        int index = GetIndex(soundName);
        if (index == sounds.Count)
        {
            Debug.LogWarning("There is no sound with this name " + soundName + " on SoundsManger, please verify your typing.");
            return;
        }
        source.pitch = sounds[index].pitchs[Random.Range(0, sounds[index].pitchs.Count)];
        source.PlayOneShot(sounds[index].clips[Random.Range(0, sounds[index].clips.Count)], sounds[index].volume);
    }

    int GetIndex(SoundName name)
    {
        foreach (Sound item in sounds)
        {
            if (name == item.name)
                return sounds.IndexOf(item);
        }
        return sounds.Count;
    }

    public enum SoundName
    {
        TO_DEFINE, waaaaf, wooshDeplacement, ouafDeMort, rewindTime, ghostPop, door, pickUpBones, victory
    }
}
