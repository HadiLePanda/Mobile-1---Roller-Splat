using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;

    public static AudioManager singleton;

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
    }

    // SFX
    public void PlaySound2DOneShot(AudioClip clip, float volume = 1.0f, float pitchVariation = 0f)
    {
        if (clip == null)
            return;

        // randomize sound values and play it
        sfxSource.pitch = Random.Range(1 - pitchVariation, 1 + pitchVariation);
        sfxSource.PlayOneShot(clip, volume);

        // reset default values
        sfxSource.pitch = 1f;
    }

    // MUSIC
    public void PlayMusic() => musicSource.Play();
    public void StopMusic() => musicSource.Stop();
    public void SetMusicVolume(float volume) => musicSource.volume = volume;
    public void SetMusicPitch(float pitch) => musicSource.pitch = pitch;
}
