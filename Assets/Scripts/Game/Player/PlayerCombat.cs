using System.Diagnostics;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator animator;
    public Transform attackPoint;
    public float weaponRange = 1;
    public float damage = 1;
    public LayerMask enemyLayer;


    [SerializeField] private float cooldown = 0.5f;
    private float timer;

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
    }

    public void Attack()
    {
        if (timer <= 0)
        {
            animator.SetBool("isAttack", true);
            Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, weaponRange, enemyLayer);

            if (hits.Length > 0)
            {
                hits[0].GetComponent<Monster>().TakeDamage(damage);
                // hits[0].GetComponent<Player>().Knockback(transform, knockbackForce, stunTime);
            }

            timer = cooldown;
        }
    }

    public void FinishAttack()
    {
        animator.SetBool("isAttack", false);
    }
}
