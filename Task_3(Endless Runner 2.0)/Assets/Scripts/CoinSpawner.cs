// CoinSpawner.cs
using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour
{
    public GameObject[] coin;
    public float Interval = 5f;

    [Tooltip("Optional: adds +/- random seconds to the interval so spawns feel less predictable. Leave at 0 to keep the exact Interval.")]
    public float randomVariation = 0f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (GroundLooper.globalspeed > 0 && coin.Length > 0) // Stop spawning if game over
            {
                Instantiate(coin[Random.Range(0, coin.Length)], transform.position, Quaternion.identity);
            }

            float wait = Interval + Random.Range(-randomVariation, randomVariation);
            yield return new WaitForSeconds(Mathf.Max(0.1f, wait));
        }
    }
}