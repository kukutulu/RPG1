using System;
using UnityEngine;

public class SpeedPotion: MonoBehaviour, ICollectible
{
    public static event Action <float> OnSpeedPotionCollected;
    [SerializeField] private float speedBoostPercent = 20f;
    public void Collect()
    {
        Destroy(gameObject);
        OnSpeedPotionCollected?.Invoke(speedBoostPercent);
    }
}
