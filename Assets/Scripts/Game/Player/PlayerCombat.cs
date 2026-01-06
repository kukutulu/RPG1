using System.Diagnostics;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    
    public Transform attackPoint;
    public LayerMask enemyLayer;
    AudioManager audioManager;
    private float timer;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

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
                        audioManager.PlaySFX(audioManager.playerHit);
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
