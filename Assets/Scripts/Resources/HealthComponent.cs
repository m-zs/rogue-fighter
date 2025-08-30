using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float health = 100;
    [SerializeField] private bool isDead = false;
    
    public float Health => health;
    public float MaxHealth => maxHealth;
    public bool IsDead => isDead;
    
    public void TakeDamage(float damage)
    {
        if (isDead) return; 
        health = Mathf.Clamp(health - damage, 0, maxHealth);
        if (health <= 0) isDead = true;
    }
}
