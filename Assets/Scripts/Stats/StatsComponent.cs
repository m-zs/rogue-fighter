using System;
using UnityEngine;

public enum Stat
{
    Strength,
    Dexterity,
    Intelligence,
    Endurance,
    Luck,
    Size,
    MovementSpeed,
    MaxPhysicalDefence,
    PhysicalDefence,
    MaxElementalDefence,
    ElementalDefence
}

public struct Stats
{
    public int Strength;
    public int Dexterity;
    public int Intelligence;
    public int Endurance;
    public int Luck;
    public int Size;

    public float MovementSpeed;

    public float MaxPhysicalDefence;
    public float PhysicalDefence;

    public float MaxElementalDefence;
    public float ElementalDefence;
}

public class StatsComponent : MonoBehaviour
{
    [SerializeField] private Stats baseStats;
    private Stats currentStats;

    public Stats CurrentStats()
    {
        var stats = currentStats;

        stats.Strength = baseStats.Strength;
        stats.Dexterity = baseStats.Dexterity;
        stats.Intelligence = baseStats.Intelligence;
        stats.Endurance = baseStats.Endurance;
        stats.Luck = baseStats.Luck;
        stats.Size = baseStats.Size;

        stats.MovementSpeed = baseStats.MovementSpeed + baseStats.Dexterity;
        stats.MaxPhysicalDefence = baseStats.MaxPhysicalDefence;
        stats.MaxElementalDefence = baseStats.MaxElementalDefence;

        stats.PhysicalDefence = Mathf.Min(baseStats.PhysicalDefence + baseStats.Endurance, baseStats.MaxPhysicalDefence);
        stats.ElementalDefence = Mathf.Min(baseStats.ElementalDefence + baseStats.Endurance, baseStats.MaxElementalDefence);

        return stats;
    }
    
    private void Awake()
    {
        currentStats = baseStats;
    }

    public void IncreaseStat(Stat stat, int amount)
    {
        switch (stat)
        {
            case Stat.Strength:
                currentStats.Strength+=amount;
                break;
            case Stat.Dexterity:
                currentStats.Dexterity+=amount;
                break;
            case Stat.Intelligence:
                currentStats.Intelligence+=amount;
                break;
            case Stat.Endurance:
                currentStats.Endurance+=amount;
                break;
            case Stat.Luck:
                currentStats.Luck+=amount;
                break;
            case Stat.Size:
                currentStats.Size+=amount;
                break;
            case Stat.MovementSpeed:
                currentStats.MovementSpeed+=amount;
                break;
            case Stat.MaxPhysicalDefence:
                currentStats.MaxPhysicalDefence+=amount;
                break;
            case Stat.PhysicalDefence:
                currentStats.PhysicalDefence = Mathf.Min(currentStats.PhysicalDefence + amount, currentStats.MaxPhysicalDefence);
                break;
            case Stat.MaxElementalDefence:
                currentStats.MaxElementalDefence += amount;
                break;
            case Stat.ElementalDefence:
                currentStats.ElementalDefence = Mathf.Min(currentStats.ElementalDefence + amount, currentStats.MaxElementalDefence);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(stat), stat, null);
        }
    }
}
