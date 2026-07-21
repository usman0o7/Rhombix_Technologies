using UnityEngine;

public class Mountain_Management : MonoBehaviour
{
    // NOTE: this used to have its own fixed "speed" value, which meant the mountains
    // never stopped moving when the player died and never sped up with the rest of
    // the game. It's now tied to GroundLooper.globalspeed instead, scaled down so it
    // still scrolls slower than the ground (a simple parallax effect).
    // Default 0.1 x globalspeed(5) = 0.5, matching the old fixed speed exactly.
    [Tooltip("How fast this layer scrolls compared to the ground speed. Lower = further away / slower.")]
    public float parallaxFactor = 0.1f;
    public float width = 10.5f;

    private void Update()
    {
        float speed = GroundLooper.globalspeed * parallaxFactor;

        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);

        if (transform.position.x <= -width)
        {
            transform.position += new Vector3(width * 2f, 0f, 0f);
        }
    }
}