using UnityEngine;

public class ManaComponent : MonoBehaviour
{
    [SerializeField] private float maxMana = 100;
    [SerializeField] private float mana = 100;
    
    public float Mana => mana;
    public float MaxMana => maxMana;
    
    public void TakeDamage(float damage)
    {
        mana = Mathf.Clamp(mana - damage, 0, maxMana);
    }
}
