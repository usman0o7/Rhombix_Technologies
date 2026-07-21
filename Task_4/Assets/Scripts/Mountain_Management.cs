using UnityEngine;

public class Mountain_Management : MonoBehaviour
{
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