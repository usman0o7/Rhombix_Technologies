using UnityEngine;
using System.Collections;

public class CactusSpawner : MonoBehaviour
{
    public GameObject[] cactus;
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
            if (GroundLooper.globalspeed > 0 && cactus.Length > 0)
            {
                Instantiate(cactus[Random.Range(0, cactus.Length)], transform.position, Quaternion.identity);
            }

            float wait = Interval + Random.Range(-randomVariation, randomVariation);
            yield return new WaitForSeconds(Mathf.Max(0.1f, wait));
        }
    }
}