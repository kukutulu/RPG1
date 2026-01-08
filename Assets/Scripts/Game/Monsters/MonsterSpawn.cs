using UnityEngine;
using System.Collections;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private float mushroomSpawnInterval = 5f;
    [SerializeField] private int maxEnemy = 10;
    private int currEnemy = 10;

    private void Start()
    {
        StartCoroutine(SpawnEnemy(mushroomPrefab, mushroomSpawnInterval));
    }

    private IEnumerator SpawnEnemy(GameObject monster, float interval)
    {
        yield return new WaitForSeconds(interval);
        Instantiate(monster, new Vector3(Random.Range(-10f, 10f), Random.Range(-5f, 5f), 0), Quaternion.identity);
        StartCoroutine(SpawnEnemy(monster, interval));
    }

    /// <summary>
    /// Manually spawn a monster at a specific position (or random if position is null).
    /// Called from PeerManager when viewer sends a spawn command.
    /// </summary>
    /// <param name="x">X position (if null, random between -10 and 10)</param>
    /// <param name="y">Y position (if null, random between -5 and 5)</param>
    public void SpawnMonsterManually(float? x = null, float? y = null)
    {
        if (mushroomPrefab == null)
        {
            Debug.LogWarning("[MonsterSpawn] Cannot spawn: mushroomPrefab is null");
            return;
        }

        float spawnX = x ?? Random.Range(-10f, 10f);
        float spawnY = y ?? Random.Range(-5f, 5f);
        Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);

        Instantiate(mushroomPrefab, spawnPos, Quaternion.identity);
        Debug.Log($"[MonsterSpawn] Manually spawned monster at ({spawnX:F2}, {spawnY:F2})");
    }
}
