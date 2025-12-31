using System.Diagnostics;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Animator animator;
    public Transform attackPoint;
    public LayerMask enemyLayer;
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
            Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, PlayerStats.Instance.WeaponRange, enemyLayer);

            if (hits.Length > 0)
            {
                foreach (var hit in hits)
                {
                    if (hit is CapsuleCollider2D)
                    {
                        hit.GetComponent<Monster>().TakeDamage(PlayerStats.Instance.Damage);
                        hit.GetComponent<Monster>().Knockback(transform, PlayerStats.Instance.KnockbackForce, PlayerStats.Instance.StunTime);
                    }
                }
            }
            timer = PlayerStats.Instance.Cooldown;
        }
    }

    public void FinishAttack()
    {
        animator.SetBool("isAttack", false);
    }
}
