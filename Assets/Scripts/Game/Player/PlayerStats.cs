
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;

    #region Sigleton
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<PlayerStats>();
            return instance;
        }
    }
    #endregion

    [Header("Health Stats")]
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private float maxTotalHealth;

    [Header("Combat Stats")]
    [SerializeField] private float weaponRange = 1;
    [SerializeField] private float damage = 1;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float stunTime = 1f;

    [Header("Combat Stats")]
    [SerializeField] private float speed = 5;


    public float Health { get { return health; } }
    public float MaxHealth { get { return maxHealth; } }
    public float MaxTotalHealth { get { return maxTotalHealth; } }
    public float Speed { get { return speed; } }
    public float WeaponRange { get { return weaponRange; } }
    public float Damage { get { return damage; } }
    public float Cooldown { get { return cooldown; } }
    public float KnockbackForce { get { return knockbackForce; } }
    public float StunTime { get { return stunTime; } }


    public void Heal(float health)
    {
        this.health += health;
        ClampHealth();
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        ClampHealth();

        if (health <= 0f)
        {
            Die();
        }
    }

    public void AddHealth()
    {
        if (maxHealth < maxTotalHealth)
        {
            maxHealth += 1;
            health = maxHealth;

            if (onHealthChangedCallback != null)
                onHealthChangedCallback.Invoke();
        }
    }

    void ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}
