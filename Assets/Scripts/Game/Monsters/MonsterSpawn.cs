using UnityEngine;
using System.Collections;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private float mushroomSpawnInterval = 5f;

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
}
