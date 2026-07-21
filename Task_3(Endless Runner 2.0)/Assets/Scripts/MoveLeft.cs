// MoveLeft.cs
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [Tooltip("How far past the left edge this object travels before it's destroyed (keeps memory clean).")]
    public float destroyBound = 20f;

    private void Update()
    {
        transform.Translate(Vector2.left * GroundLooper.globalspeed * Time.deltaTime);
        if (transform.position.x < -destroyBound)
        {
            Destroy(gameObject);
        }
    }
}