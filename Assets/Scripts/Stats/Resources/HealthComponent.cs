using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public event System.Action OnDeathEvent;

    [SerializeField] private float baseMaxHealth = 100;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float health = 100;
    [SerializeField] private bool isDead = false;
    
    public float Health => health;
    public float MaxHealth => maxHealth;
    public float BaseMaxHealth => baseMaxHealth;
    public bool IsDead => isDead;
    
    public void TakeDamage(float damage)
    {
        if (isDead) return; 
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        if (health <= 0) OnDeath();
    }

    public void SetMaxHealth(float newMaxHealth, bool refill = false)
    {
        this.maxHealth = newMaxHealth;
        if (refill) health = maxHealth;
    }

    private void OnDeath()
    {
        isDead = true;
        OnDeathEvent?.Invoke();
    }

    private void Awake()
    {
        SetMaxHealth(baseMaxHealth, true);
    }
}
