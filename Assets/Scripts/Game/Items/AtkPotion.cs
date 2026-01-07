using System;
using UnityEngine;

public class AtkPotion : MonoBehaviour, ICollectible
{
    public static event Action<float> OnAtkPotionCollected;
    [SerializeField] private float atkAmount = 1f;

    public void Collect()
    {
        Destroy(gameObject);
        OnAtkPotionCollected?.Invoke(atkAmount);
    }
}
