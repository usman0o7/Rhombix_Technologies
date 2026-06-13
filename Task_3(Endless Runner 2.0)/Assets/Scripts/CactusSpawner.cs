using UnityEngine;

public class CactusSpawner : MonoBehaviour
{
    public GameObject[] cactus;
    public float Interval = 5f;

    private void Start()
    {
        InvokeRepeating("spawn", 1f, Interval);
    }

    void spawn()
    {
        Instantiate(cactus[Random.Range(0, cactus.Length)], transform.position, Quaternion.identity);
    }

}
