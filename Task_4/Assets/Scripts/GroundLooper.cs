using UnityEngine;

public class GroundLooper : MonoBehaviour
{
    public static float globalspeed = 5f;
    public float width = 10.5f;

    [Header("speed the game up the longer you survive after selecing true")]
    public bool increaseDifficultyOverTime = false;
    public float maxSpeed = 15f;
    public float speedIncreasePerSecond = 0.05f;

    private float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if (increaseDifficultyOverTime && globalspeed > 0f)
        {
            float elapsed = Time.time - startTime;
            globalspeed = Mathf.Min(5f + elapsed * speedIncreasePerSecond, maxSpeed);
        }

        transform.Translate(Vector2.left * globalspeed * Time.deltaTime, Space.World);
        if (transform.position.x <= -width)
        {
            transform.position += new Vector3(width * 2f, 0f, 0f);
        }
    }
}