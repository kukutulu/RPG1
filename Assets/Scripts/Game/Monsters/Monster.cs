using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Rigidbody2D rb;
    public Animator animator;
    [SerializeField] HealthBar healthBar;
    [SerializeField] float health, maxHealth = 5f;
    [SerializeField] float deathDelay = 3f;

    private bool isDead = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<HealthBar>();

        // Force isDead to false immediately
        if (animator != null)
        {
            animator.SetBool("isDead", false);
        }
    }

    private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Monster Died");

        // Destroy after animation plays
        animator.SetBool("isDead", true);

        // Disable components to prevent interaction
        if (rb != null) rb.simulated = false;
        if (healthBar != null) healthBar.gameObject.SetActive(false);
    }

    public void AfterDeadAnimation()
    {
        Destroy(gameObject);
    }
}