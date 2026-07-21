// GroundLooper.cs
using UnityEngine;

public class GroundLooper : MonoBehaviour
{
    public static float globalspeed = 5f;
    public float width = 10.5f;

    [Header("Optional: speed the game up the longer you survive")]
    [Tooltip("Off by default, so the game plays exactly like before unless you turn this on.")]
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
        // Only ramps up while the game is actually running (globalspeed is 0 after death,
        // so this correctly stops increasing speed once the player dies).
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