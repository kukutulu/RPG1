using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator animator;
    public PlayerCombat playerCombat;
    public SwordHitbox sword;

    private float x;
    private float y;
    private bool isRun = false;
    private bool isKnockback = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sword = GetComponentInChildren<SwordHitbox>();
    }

    private void FixedUpdate()
    {
        if (!isKnockback)
        {
            HandleMovement();
            HandleAnimation();
        }
    }

    private void Update()
    {
        if (Input.GetButtonUp("Slash") && !isKnockback)
        {
            playerCombat.Attack();
        }
    }

    private void HandleMovement()
    {
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * PlayerStats.Instance.Speed, Input.GetAxis("Vertical") * PlayerStats.Instance.Speed);

        sword.UpdatePosition(new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"))
        );
    }

    private void HandleAnimation()
    {
        if (rb.linearVelocityX != 0 || rb.linearVelocityY != 0)
        {
            isRun = true;
        }
        else
        {
            isRun = false;
        }

        if (isRun)
        {
            animator.SetFloat("X", x);
            animator.SetFloat("Y", y);
        }

        animator.SetBool("isRun", isRun);
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
}
