using UnityEngine;

public class GroundLooper : MonoBehaviour
{
    public static float globalspeed = 5f;
    public float width = 10.5f;

    private void Update()
    {
        //globalspeed = globalspeed + 0.01f * Time.deltaTime;
        transform.Translate(Vector2.left * globalspeed * Time.deltaTime);
        if (transform.position.x <= -width)
        {
            transform.position += new Vector3(width * 2, 0, 0);
        }
    }
}
