using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Source")]
    public AudioSource sfxSource;

    [Header("SFX Pools (Randomized 4-Options)")]
    public AudioClip[] dinoSpawnClips = new AudioClip[4];
    public AudioClip[] attackClips = new AudioClip[4];

    [Header("Single SFX")]
    public AudioClip heartLostClip;
    public AudioClip gameOverClip;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void PlayDinoSpawnSound()
    {
        PlayRandomClip(dinoSpawnClips);
    }

    public void PlayAttackSound()
    {
        PlayRandomClip(attackClips);
    }

    public void PlayHeartLostSound()
    {
        PlaySingleClip(heartLostClip);
    }

    public void PlayGameOverSound()
    {
        PlaySingleClip(gameOverClip);
    }

    private void PlayRandomClip(AudioClip[] clips)
    {
        if (clips.Length == 0 || sfxSource == null) return;
        int randomIndex = Random.Range(0, clips.Length);
        if (clips[randomIndex] != null)
        {
            sfxSource.PlayOneShot(clips[randomIndex]);
        }
    }

    private void PlaySingleClip(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}