using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerControl : MonoBehaviour
{
    public float JumpForce = 10f;
    public LayerMask groundlayer;
    public Transform groundcheck;
    public AnimationClip Player_Die;
    private int Score = 0;
    public TextMeshProUGUI ScoreText;
    public GameObject Restart_Button;


    private Animator animator;
    private Rigidbody2D rb;
    private void Start()
    {   
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Restart_Button.SetActive(false);

        // CRITICAL: Unfreeze the game whenever a new scene starts!
        Time.timeScale = 1f;
    }

    public void Update()
    {
        bool isGrounded = Physics2D.OverlapCircle(groundcheck.position, 0.2f, groundlayer);
        //Debug.Log("Value of speed:" + GroundLooper.globalspeed);
        animator.SetBool("IsGrounded", isGrounded);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //animator.SetBool("IsGrounded", false);
            rb.linearVelocity = Vector2.up * JumpForce;
        }
        else
        {
            //animator.SetBool("IsGrounded", true);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            animator.SetBool("IsDead", true);
            GroundLooper.globalspeed = 0f;
            Time.timeScale = 0f;    
            
            if(Restart_Button != null)
            {
                Restart_Button.SetActive(true);
            }

            //Invoke(nameof(Restart_Game), Player_Die.length);
        }
    }

    public void Restart_Game()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GroundLooper.globalspeed = 5f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("coin"))
        {
            Destroy(collision.gameObject);
            Debug.Log("Score Increases");
            Score++;
            ScoreText.text = Score.ToString();
        }
    }
}
