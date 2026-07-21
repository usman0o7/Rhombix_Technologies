using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float JumpForce = 12f;
    public float fallMultiplier = 2.5f;
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
    private bool wasGroundedLastFrame = true;

    private bool isDead = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (Restart_Button != null)
            Restart_Button.SetActive(false);

        isDead = false;
        currentHealth = maxHealth;
        UpdateHealthUI();

        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        Time.timeScale = 1f;
    }

    public void Update()
    {
        if (isDead)
            return;

        bool isGrounded = groundcheck != null &&
            Physics2D.OverlapCircle(groundcheck.position, 0.2f, groundlayer);
        animator.SetBool("IsGrounded", isGrounded);

        // Play Land Sound
        if (isGrounded && !wasGroundedLastFrame && AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.landSound);
        }
        wasGroundedLastFrame = isGrounded;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = Vector2.up * JumpForce;

            // Play Jump Sound
            if (AudioManager.instance != null)
                AudioManager.instance.PlaySFX(AudioManager.instance.jumpSound);
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            ShootFireball();
        }

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
        if (fireballPrefab == null || firePoint == null)
            return;

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
        if (isDead)
            return;

        if (collision.gameObject.CompareTag("Damage"))
        {
            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth -= damage;
        UpdateHealthUI();

        // Play Damage Sound
        if (AudioManager.instance != null)
            AudioManager.instance.PlaySFX(AudioManager.instance.damageSound);

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
        if (isDead)
            return;

        isDead = true;


        animator.SetBool(deadBoolName, true);
        GroundLooper.globalspeed = 0f;

        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlaySFX(AudioManager.instance.dieSound);
            AudioManager.instance.StopMusic();
        }

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
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayMusic();
        }

        GroundLooper.globalspeed = 5f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead)
            return;

        if (collision.gameObject.CompareTag("coin"))
        {
            Destroy(collision.gameObject);
            Score++;

            if (ScoreText != null)
                ScoreText.text = Score.ToString();

            // Play Coin Sound via AudioManager
            if (AudioManager.instance != null)
                AudioManager.instance.PlaySFX(AudioManager.instance.coinSound);
        }
    }
}