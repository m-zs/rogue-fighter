using System;
using UnityEngine;

public enum CreatureAction
{
    Move,
    Attack
}

public class Creature : MonoBehaviour
{
    [SerializeField] private int size = 10;
    [SerializeField] private int strength = 10;
    [SerializeField] private int dexterity = 10;
    [SerializeField] private int intelligence = 10;
    [SerializeField] private int endurance = 10;
    [SerializeField] private int luck = 10;

    [SerializeField] private float movementSpeed = 0;
    [SerializeField] private float maxPhysicalDefence = 80;
    [SerializeField] private float physicalDefence = 0;
    [SerializeField] private float maxElementalDefence = 80;
    [SerializeField] private float elementalDefence = 0;

    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float health = 100;
    [SerializeField] private float maxMana = 100;
    [SerializeField] private float mana = 100;
    
    [SerializeField] private bool isDead = false;
    
    public float MovementSpeed => movementSpeed;
    public float Health => health;
    public float Mana => mana;
    public float PhysicalDefence { get => physicalDefence; set => physicalDefence = Math.Min(value, maxPhysicalDefence); }
    public float ElementalDefence { get => elementalDefence; set => elementalDefence = Math.Min(value, elementalDefence); }
    
    private float CalcHealth()
    {
        return (strength * 10) + (size * 2);
    }

    private float CalcMana()
    {
        return intelligence * 10;
    }

    private float CalcSpeed()
    {
        return dexterity;
    }

    private float CalcPhysicalDefence()
    {
        return Math.Min(endurance, maxPhysicalDefence);
    }

    private float CalcElementalDefence()
    {
        return Math.Min(endurance, maxElementalDefence);
    }

    private void UpdateResourceStats(bool refillCurrent = false)
    {
        maxHealth = CalcHealth();
        maxMana = CalcMana();

        if (refillCurrent)
        {
            health = maxHealth;
            mana = maxMana;
        }
    }

    private void UpdateStats()
    {
        movementSpeed = CalcSpeed();
        physicalDefence = CalcPhysicalDefence();
        elementalDefence = CalcElementalDefence();
    }

    protected virtual void Awake()
    {
        UpdateResourceStats(true);
        UpdateStats();
    }

    protected virtual void Update()
    {
        if (isDead) return;
        UpdateResourceStats();
        UpdateStats();
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return; 
        health -= damage;
        if (health <= 0) isDead = true;
    }

    public void TakeManaDamage(float damage)
    {
        if (isDead) return;
        mana -= damage;
    }

    public bool CanPerformAction(CreatureAction action)
    {
        if (isDead) return false;

        return action switch
        {
            CreatureAction.Move => movementSpeed > 0,
            CreatureAction.Attack => true,
            _ => true
        };
    }
}
