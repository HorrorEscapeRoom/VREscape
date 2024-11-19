using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Safe
{
    public class SoundUtility : MonoBehaviour
    {
        [SerializeField] AudioSource[] audioSources;

        public static SoundUtility Instance { get; private set; }

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(Instance);
        }

        public enum SoundType
        {
            Moving,
            Locking,
            Unlocking,
            CorrectNumber,
            IncorrectNumber
        }

        bool TryGetValidAudioSource(int index, out AudioSource audioSource)
        {
            audioSource = null; 

            if (audioSources == null)
            {
                Debug.LogError("Audio sources are not initialized.");
                return false; 
            }

            if (index < 0 || index >= audioSources.Length)
            {
                Debug.LogError($"Sound index {index} is out of range for audio sources.");
                return false;
            }

            audioSource = audioSources[index]; 

            if (audioSource == null || audioSource.clip == null)
            {
                Debug.LogError($"Audio source at index {index} is invalid.");
                return false;
            }

            return true; 
        }

        public void PlaySound(SoundType sound) 
        {
            int soundIndex = (int)sound;
            if (TryGetValidAudioSource(soundIndex, out AudioSource audioSource) && !audioSource.isPlaying) 
                audioSource.Play();
        }
    }
}
