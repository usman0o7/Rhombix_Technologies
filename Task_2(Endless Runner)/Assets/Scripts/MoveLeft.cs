using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    //public float speed = 5f;

    private void Update()
    {
        //Debug.Log("Value of speed: in moveleft" + GroundLooper.globalspeed);
        transform.Translate(Vector2.left * GroundLooper.globalspeed * Time.deltaTime);
        if(transform.position.x < -20f)
        {
            Destroy(gameObject);
        }
    }
}
