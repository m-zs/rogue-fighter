using UnityEngine;

public class ManaComponent : MonoBehaviour
{
    [SerializeField] private float baseMaxMana = 100;
    [SerializeField] private float maxMana = 100;
    [SerializeField] private float mana = 100;

    public float Mana => mana;
    public float MaxMana => maxMana;
    public float BaseMaxMana => baseMaxMana;
    
    public void TakeDamage(float damage)
    {
        mana = Mathf.Clamp(mana - damage, 0, maxMana);
    }

    public void SetMaxMana(float newMaxMana, bool refill = false)
    {
        maxMana = newMaxMana;
        if (refill) mana = maxMana;
    }

    private void Awake()
    {
        SetMaxMana(baseMaxMana, true);
    }
}
