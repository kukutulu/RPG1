using Unity.VisualScripting;
using UnityEngine;

public class AIChase : MonoBehaviour
{
    public GameObject player;
    public float speed = 1f;
    public float attackRange = 1.5f;
    public float chaseRange = 6f;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private Transform curPlayer;
    private bool isChasing = false;

    // If your sprite's default artwork faces right when scale.x is positive,
    // set this to true. If the sprite faces right when scale.x is negative,
    // set this to false.
    public bool spriteFacesRightByDefault = true;
    private bool facingRight = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null)
            animator = GetComponent<Animator>();
        if (animator != null)
            animator.SetBool("isNearPlayer", false);
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        // initialize facing based on localScale.x and the sprite's default direction
        facingRight = (transform.localScale.x >= 0f) == spriteFacesRightByDefault;

    }

    public void ChasePlayer()
    {
        if (isChasing)
        {
            float distance = Vector2.Distance(transform.position, curPlayer.transform.position);
            if (distance <= attackRange)
            {
                animator.SetBool("isNormalAttack", true);
            }
            else
            {
                animator.SetBool("isNormalAttack", false);
            }

            animator.SetBool("isNearPlayer", true);
            float horizontal = curPlayer.position.x - transform.position.x;
            Flip(-horizontal);

            Vector2 dir = (curPlayer.position - transform.position).normalized;
            rb.linearVelocity = dir * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (curPlayer == null)
            {
                curPlayer = collision.transform;
            }
            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isChasing = false;
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isNearPlayer", false);
            animator.SetBool("isNormalAttack", false);
        }
    }

    private void Flip(float horizontal)
    {
        if (Mathf.Approximately(horizontal, 0f))
            return;

        bool targetFacingRight = horizontal > 0f;
        if (targetFacingRight == facingRight)
            return;

        facingRight = targetFacingRight;

        Vector3 s = transform.localScale;
        float sign = (targetFacingRight == spriteFacesRightByDefault) ? 1f : -1f;
        s.x = sign * Mathf.Abs(s.x);
        transform.localScale = s;
    }
}