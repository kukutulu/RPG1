using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--------Audio Sources--------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("--------Audio Clips--------")]
    public AudioClip background;
    public AudioClip playerHit;
    public AudioClip powerUp;
    public AudioClip pickItem;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
