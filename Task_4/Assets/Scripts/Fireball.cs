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
}