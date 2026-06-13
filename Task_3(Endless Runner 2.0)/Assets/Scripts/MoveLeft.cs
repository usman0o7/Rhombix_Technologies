using UnityEngine;

public class MoveLeft : MonoBehaviour
{

    private void Update()
    {

        transform.Translate(Vector2.left * GroundLooper.globalspeed * Time.deltaTime);
        if(transform.position.x < -20f)
        {
            Destroy(gameObject);
        }
    }
}
