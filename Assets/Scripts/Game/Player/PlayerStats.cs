
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;

    #region Sigleton
    private static PlayerStats instance;
    [SerializeField] private SpriteRenderer spriteRendererLevelUp;
    [SerializeField] private string levelUpChildName = "LevelUp"; // Name of the child GameObject with the level up sprite
    [SerializeField] private float levelUpDisplayDuration = 0.8f; // How long to show the level up sprite (0 = don't auto-hide)

    [System.Obsolete]
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

    private void Start()
    {
        GetLevelUpSprite();
        HeartItem.OnHeartItemCollected += Heal;
        AtkPotion.OnAtkPotionCollected += BoostAtk;
        SpeedPotion.OnSpeedPotionCollected += BoostSpeed;
    }

    private void GetLevelUpSprite()
    {
        if (spriteRendererLevelUp == null)
        {
            Transform levelUpChild = transform.Find(levelUpChildName);
            if (levelUpChild != null)
            {
                spriteRendererLevelUp = levelUpChild.GetComponent<SpriteRenderer>();
            }

            if (spriteRendererLevelUp == null)
            {
                spriteRendererLevelUp = GetComponentInChildren<SpriteRenderer>();
            }
        }

        if (spriteRendererLevelUp != null)
        {
            spriteRendererLevelUp.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("PlayerStats: Level Up SpriteRenderer not found! Make sure to assign it in the inspector or name the child GameObject '" + levelUpChildName + "'");
        }
    }


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

    void BoostAtk(float amount)
    {
        damage += amount;
    }

    void BoostSpeed(float percent)
    {
        speed += 0.1f;
    }

    public void LevelUp()
    {
        if (spriteRendererLevelUp != null)
        {
            spriteRendererLevelUp.gameObject.SetActive(true);

            // Auto-deactivate after duration if specified
            if (levelUpDisplayDuration > 0f)
            {
                StartCoroutine(HideLevelUpSpriteAfterDelay(levelUpDisplayDuration));
            }
        }
        else
        {
            Debug.LogWarning("PlayerStats: Cannot activate level up sprite - SpriteRenderer is null!");
        }
    }

    private IEnumerator HideLevelUpSpriteAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (spriteRendererLevelUp != null)
        {
            spriteRendererLevelUp.gameObject.SetActive(false);
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}
