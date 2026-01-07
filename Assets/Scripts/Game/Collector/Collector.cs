using UnityEngine;

public class Collector : MonoBehaviour
{
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ICollectible>(out var collectible))
        {
            audioManager.PlaySFX(audioManager.pickItem);
            collectible.Collect();
        }
    }
}
