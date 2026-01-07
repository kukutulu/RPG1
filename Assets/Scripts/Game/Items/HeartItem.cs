using System;
using UnityEngine;

public class HeartItem: MonoBehaviour, ICollectible
{
    public static event Action OnHeartItemCollected;
    public void Collect()
    {
        Destroy(gameObject);
        OnHeartItemCollected?.Invoke();
    }
}
