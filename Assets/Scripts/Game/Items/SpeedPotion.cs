using System;
using UnityEngine;

public class SpeedPotion: MonoBehaviour, ICollectible
{
    public static event Action OnSpeedPotionCollected;
    public void Collect()
    {
        Destroy(gameObject);
        OnSpeedPotionCollected?.Invoke();
    }
}
