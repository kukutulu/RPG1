using System;
using UnityEngine;

public class AtkPotion: MonoBehaviour, ICollectible
{
    public static event Action OnAtkPotionCollected;
    public void Collect()
    {
        Destroy(gameObject);
        OnAtkPotionCollected?.Invoke();
    }
}
