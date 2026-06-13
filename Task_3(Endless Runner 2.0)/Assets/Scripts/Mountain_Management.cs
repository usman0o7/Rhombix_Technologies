using UnityEngine;

public class Mountain_Management : MonoBehaviour
{
    public float speed = 0.5f;
    public float width = 10.5f;

    private void Update()
    {
        //Debug.Log("Value of speed: in moveleft" + GroundLooper.globalspeed);
        transform.Translate(Vector2.left * speed * Time.deltaTime, Space.World);
        if (transform.position.x < -20f)
        {
            Destroy(gameObject);
        }

        transform.Translate(Vector2.left * speed * Time.deltaTime);
        if (transform.position.x <= -width)
        {
            transform.position += new Vector3(width * 2, 0, 0);
        }

    }
}
