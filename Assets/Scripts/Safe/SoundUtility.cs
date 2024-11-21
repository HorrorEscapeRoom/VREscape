using UnityEngine;

namespace Assets.Scripts.Safe
{
    public class SoundUtility : MonoBehaviour
    {
        [SerializeField] private AudioSource[] audioSources;

        public static SoundUtility Instance { get; private set; }

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public enum SoundType
        {
            Moving,
            Locking,
            Unlocking,
            CorrectNumber,
            IncorrectNumber
        }

        public void PlaySound(SoundType soundType)
        {
            int index = (int)soundType;
            if (audioSources != null && index >= 0 && index < audioSources.Length)
            {
                AudioSource audioSource = audioSources[index];
                if (audioSource != null && !audioSource.isPlaying)
                    audioSource.Play();
            }
        }
    }
}