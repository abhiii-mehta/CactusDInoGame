using UnityEngine;

public class AudioSequencePlayer : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Gameplay Music Tracks (Order: 1 to 6)")]
    public AudioClip[] speedClips = new AudioClip[6]; 

    [Header("References")]
    public PlayerController playerController;

    private int currentTrackIndex = 0;
    private double nextStartTime;
    private bool isPlayingGameplaySequence = false;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        }
        
        // Ensure nothing plays on boot (Main Menu state)
        audioSource.Stop();
    }

    public void StartGameplaySequence()
    {
        if (speedClips.Length == 0 || speedClips[0] == null) return;

        isPlayingGameplaySequence = true;
        audioSource.loop = false; 
        currentTrackIndex = 0;

        audioSource.clip = speedClips[0];
        nextStartTime = AudioSettings.dspTime;
        audioSource.PlayScheduled(nextStartTime);
        nextStartTime += speedClips[0].length;

        if (playerController != null)
        {
            playerController.SetSpeedTier(0);
        }
    }

    public void StopMusic()
    {
        isPlayingGameplaySequence = false;
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void PauseMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public void ResumeMusic()
    {
        if (audioSource != null)
        {
            audioSource.UnPause();
        }
    }

    void Update()
    {
        if (!isPlayingGameplaySequence) return;

        // Check if it's time to queue up the next track in sequence
        if (currentTrackIndex < speedClips.Length - 1)
        {
            if (AudioSettings.dspTime >= nextStartTime - 0.1) // 100ms look-ahead buffer
            {
                currentTrackIndex++;
                AudioClip nextClip = speedClips[currentTrackIndex];

                if (nextClip != null)
                {
                    audioSource.SetScheduledStartTime(nextStartTime);
                    audioSource.clip = nextClip;
                    audioSource.PlayScheduled(nextStartTime);

                    nextStartTime += nextClip.length;

                    // Bump player speed tier to match the new track
                    if (playerController != null)
                    {
                        playerController.SetSpeedTier(currentTrackIndex);
                    }

                    // If this is the final 6th track (index 5), loop it continuously
                    if (currentTrackIndex == speedClips.Length - 1)
                    {
                        audioSource.loop = true;
                    }
                }
            }
        }
    }
}