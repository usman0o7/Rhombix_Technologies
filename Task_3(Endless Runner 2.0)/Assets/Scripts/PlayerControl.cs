using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float JumpForce = 10f;
    public LayerMask groundlayer;
    public Transform groundcheck;

    [Header("Health UI Elements")]
    public Image[] heartImages;
    private int currentHealth = 3;
    private int maxHealth = 3;

    [Header("Fireball Weapon Settings")]
    public GameObject fireballPrefab;
    public Transform firePoint;
    public Image[] ammoImages;
    public float ammoRegenInterval = 8f;

    private int currentAmmo = 2;
    private int maxAmmo = 2;
    private float ammoTimer = 0f;

    [Header("Game State UI & Animation")]
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

        // Initialize health pool and heart UI display state
        currentHealth = maxHealth;
        UpdateHealthUI();

        // Initialize weapon inventory ammo display state
        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        Time.timeScale = 1f;
    }

    public void Update()
    {
        // <--- JUMPING MECHANIM --->
        bool isGrounded = Physics2D.OverlapCircle(groundcheck.position, 0.2f, groundlayer);
        animator.SetBool("IsGrounded", isGrounded);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = Vector2.up * JumpForce;
        }
        

        // <--- Left Mouse Button Shoot --->
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            ShootFireball();
        }

        // <--- AMMO RELOAD COOLDOWN --->
        if (currentAmmo < maxAmmo)
        {
            ammoTimer += Time.deltaTime;
            if (ammoTimer >= ammoRegenInterval)
            {
                currentAmmo++;
                UpdateAmmoUI();
                ammoTimer = 0f;
            }
        }
        else
        {
            ammoTimer = 0f;
        }
    }

    void ShootFireball()
    {
        currentAmmo--;
        UpdateAmmoUI();
        Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
    }

    void UpdateAmmoUI()
    {
        for (int i = 0; i < ammoImages.Length; i++)
        {
            if (i < currentAmmo)
            {
                ammoImages[i].enabled = true;
            }
            else
            {
                ammoImages[i].enabled = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (i < currentHealth)
            {
                heartImages[i].enabled = true;
            }
            else
            {
                heartImages[i].enabled = false;
            }
        }
    }

    void Die()
    {
        animator.SetBool("IsDead", true);
        GroundLooper.globalspeed = 0f;
        Time.timeScale = 0f;

        if (Restart_Button != null)
        {
            Restart_Button.SetActive(true);
        }
    }

    public void Restart_Game()
    {
        GroundLooper.globalspeed = 5f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("coin"))
        {
            Destroy(collision.gameObject);
            Score++;
            ScoreText.text = Score.ToString();
        }
    }
}