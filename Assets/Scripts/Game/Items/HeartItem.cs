using System;
using UnityEngine;

public class HeartItem : MonoBehaviour, ICollectible
{
    public static event Action<float> OnHeartItemCollected;
    [SerializeField] private float healAmount = 1f;

    public void Collect()
    {
        Destroy(gameObject);
        OnHeartItemCollected?.Invoke(healAmount);
    }
}
