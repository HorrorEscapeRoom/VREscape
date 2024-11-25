using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Clips")]
    [SerializeField] AudioClip doorOpenClip;
    [SerializeField] AudioClip doorCloseClip;
    [SerializeField] AudioClip doorLockClip;
    [SerializeField] AudioClip doorUnlockClip;
    [SerializeField] AudioClip dialCorrectClip;
    [SerializeField] AudioClip dialIncorrectClip;
    [SerializeField] AudioClip dialMoveClip;

    [Header("Audio Source")]
    [SerializeField] AudioSource audioSource;

    void Awake()
    {
        if (audioSource == null)
        {
            Debug.LogError("[SoundManager] AudioSource is missing. Please attach an AudioSource component.");
            enabled = false;
        }
    }

    public void PlayDoorOpenSound() => PlaySound(doorOpenClip);

    public void PlayDoorCloseSound() => PlaySound(doorCloseClip);

    public void PlayDoorLockSound() => PlaySound(doorLockClip);

    public void PlayDoorUnlockSound() => PlaySound(doorUnlockClip);

    public void PlayDialCorrectSound() => PlaySound(dialCorrectClip);

    public void PlayDialIncorrectSound() => PlaySound(dialIncorrectClip);

    public void PlayDialMoveSound() => PlaySound(dialMoveClip);

    void PlaySound(AudioClip clip)
    {
        if (clip != null) audioSource.PlayOneShot(clip);
        else Debug.LogWarning("[SoundManager] AudioClip is missing for this action.");
    }
}