// Fireball.cs
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 12f;

    [Tooltip("How far right the fireball can travel before it's destroyed.")]
    public float destroyBound = 20f;

    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime, Space.World);

        if (transform.position.x > destroyBound)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }

    // NOTE: for this to fire at all, the Fireball prefab needs a Collider2D with
    // "Is Trigger" turned on, plus a Rigidbody2D (Body Type can be Kinematic).
    // Unity only sends trigger events if at least one of the two colliders
    // involved has a Rigidbody2D attached.
}