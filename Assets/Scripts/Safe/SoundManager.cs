using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour, ISoundManager
{
    [SerializeField] private AudioSource[] audioSources;

    public enum SoundType
    {
        Moving,
        Locking,
        Unlocking,
        CorrectNumber,
        IncorrectNumber
    }

    public void PlaySound(SoundType sound)
    {
        if (audioSources != null && audioSources.Length > (int)sound && !audioSources[(int)sound].isPlaying)
        {
            audioSources[(int)sound].Play();
        }
    }
}
