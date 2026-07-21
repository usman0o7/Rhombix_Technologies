using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float JumpForce = 12f;
    public float fallMultiplier = 2.5f; // extra gravity once falling, so landing is quick
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
    public string deadBoolName = "IsDead";
    private int Score = 0;
    public TextMeshProUGUI ScoreText;
    public GameObject Restart_Button;

    private Animator animator;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (Restart_Button != null)
            Restart_Button.SetActive(false);

        currentHealth = maxHealth;
        UpdateHealthUI();

        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        Time.timeScale = 1f;
    }

    public void Update()
    {
        // Ground Check
        bool isGrounded = Physics2D.OverlapCircle(groundcheck.position, 0.2f, groundlayer);
        animator.SetBool("IsGrounded", isGrounded);

        // Jump Input - only works while grounded, so there's no double/air jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = Vector2.up * JumpForce;
        }

        // Once the player starts falling, add extra gravity so they come back down
        // to the ground quickly instead of floating. This is the only gravity
        // change we apply - jump height always stays the same (no short jump).
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        // Combat Input
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            ShootFireball();
        }

        // Passive Ammo Regen
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
            ammoImages[i].enabled = (i < currentAmmo);
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
            heartImages[i].enabled = (i < currentHealth);
        }
    }

    void Die()
    {
        animator.SetBool(deadBoolName, true);
        GroundLooper.globalspeed = 0f;

        // Delay freezing time slightly so the player death animation actually triggers
        StartCoroutine(FreezeTimeDelayed(0.5f));

        if (Restart_Button != null)
        {
            Restart_Button.SetActive(true);
        }
    }

    private IEnumerator FreezeTimeDelayed(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 0f;
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