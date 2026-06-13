using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 12f;

    private void Update()
    {
        // Move the fireball to the right
        transform.Translate(Vector2.right * speed * Time.deltaTime, Space.World);

        // Remove the fireball asset automatically
        if (transform.position.x > 20f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            Destroy(collision.gameObject); // Erase the cactus
            Destroy(gameObject);          // Erase this fireball
        }
    }
}