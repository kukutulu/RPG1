using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    Rigidbody2D rb;
    public Animator animator;
    [SerializeField] HealthBar healthBar;
    [SerializeField] private float health, maxHealth = 5f;
    [SerializeField] private float exp = 50f;
    private bool isKnockback = false;
    private bool isDead = false;
    private AIChase AIChase;

    public delegate void MonsterDead(float exp);
    public static event MonsterDead OnMonsterDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar = GetComponentInChildren<HealthBar>();
        AIChase = GetComponent<AIChase>();

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
        if (AIChase != null && !isKnockback)
        {
            AIChase.ChasePlayer();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return;

        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        StartCoroutine(PlayHurtAnimation());
        if (health <= 0)
        {
            OnMonsterDead(exp);
            Die();
        }
    }

    void Die()
    {
        isDead = true;
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

    public void Knockback(Transform enemy, float force, float stunTime)
    {
        isKnockback = true;
        Vector2 dir = (transform.position - enemy.position).normalized;
        rb.linearVelocity = dir * force;
        StartCoroutine(KnockbackCounter(stunTime));
    }

    IEnumerator KnockbackCounter(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        rb.linearVelocity = Vector2.zero;
        isKnockback = false;
    }

    IEnumerator PlayHurtAnimation()
    {
    animator.SetBool("isHurt", true);
    yield return new WaitForSeconds(0.5f);
    animator.SetBool("isHurt", false);
    }
}