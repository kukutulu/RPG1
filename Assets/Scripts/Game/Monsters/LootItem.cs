using UnityEngine;

[System.Serializable]
public class LootItem
{
    public GameObject prefab;
    [Range(0, 100)] public float dropChance;
}
