using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject[] coin;
    public float Interval = 5f;
    public AnimationClip coin_Rotate;
    private void Start()
    {
        InvokeRepeating("spawn", 1f, Interval);
    }

    void spawn()
    {
        Instantiate(coin[Random.Range(0, coin.Length)], transform.position, Quaternion.identity);
    }
}
