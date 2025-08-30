using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
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

[Serializable]
public struct Stats
{
    public int Strength;
    public int Dexterity;
    public int Intelligence;
    public int Endurance;
    public int Luck;
    public int Size;

    public int MovementSpeed;

    public int MaxPhysicalDefence;
    public int PhysicalDefence;

    public int MaxElementalDefence;
    public int ElementalDefence;
}

public class StatsComponent : MonoBehaviour
{
    [SerializeField] private List<StatProcessorMapping> statProcessors = new List<StatProcessorMapping>();
    [SerializeField] private Stats baseStats;
    private Stats currentStats = new();
    private Stats statsCache = new();

    [Serializable]
    public class StatProcessorMapping
    {
        public Stat stat;
        [SerializeReference] public StatProcessor processor;
    }

    private void BuildStatsCache()
    {
        foreach (var statProcessor in statProcessors.Where(statProcessor => statProcessor.processor != null))
        {
            switch (statProcessor.stat)
            {
                case Stat.Strength:
                    statsCache.Strength = Mathf.CeilToInt(statProcessor.processor.Compute(currentStats));
                    break;
                case Stat.Dexterity:
                    statsCache.Dexterity = Mathf.CeilToInt(statProcessor.processor.Compute(currentStats));
                    break;
                case Stat.Intelligence:
                    statsCache.Intelligence = Mathf.CeilToInt(statProcessor.processor.Compute(currentStats));   
                    break;
                case Stat.Endurance:
                    statsCache.Endurance = Mathf.CeilToInt(statProcessor.processor.Compute(currentStats));
                    break;
                case Stat.Luck:
                    statsCache.Luck = Mathf.CeilToInt(statProcessor.processor.Compute(currentStats));
                    break;
                case Stat.Size:
                    statsCache.Size = Mathf.CeilToInt(statProcessor.processor.Compute(currentStats));
                    break;
                case Stat.MovementSpeed:
                    statsCache.MovementSpeed = Mathf.CeilToInt(statProcessor.processor.Compute(currentStats));
                    break;
                case Stat.MaxPhysicalDefence:
                    statsCache.MaxPhysicalDefence = Mathf.CeilToInt(statProcessor.processor.Compute(currentStats));
                    break;
                case Stat.PhysicalDefence:
                    statsCache.PhysicalDefence = Mathf.CeilToInt(statProcessor.processor.Compute(currentStats));
                    break;
                case Stat.MaxElementalDefence:
                    statsCache.MaxElementalDefence = Mathf.CeilToInt(statProcessor.processor.Compute(currentStats));
                    break;
                case Stat.ElementalDefence:
                    statsCache.ElementalDefence = Mathf.CeilToInt(statProcessor.processor.Compute(currentStats));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            };
        }
    }

    public int GetStat(Stat stat)
    {
        return stat switch
        {
            Stat.Strength => statsCache.Strength,
            Stat.Dexterity => statsCache.Dexterity,
            Stat.Intelligence => statsCache.Intelligence,
            Stat.Endurance => statsCache.Endurance,
            Stat.Luck => statsCache.Luck,
            Stat.Size => statsCache.Size,
            Stat.MovementSpeed => statsCache.MovementSpeed,
            Stat.MaxPhysicalDefence => statsCache.MaxPhysicalDefence,
            Stat.PhysicalDefence => statsCache.PhysicalDefence,
            Stat.MaxElementalDefence => statsCache.MaxElementalDefence,
            Stat.ElementalDefence => statsCache.ElementalDefence,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public Stats CurrentStats()
    {
        return statsCache;
    }
    
    private void Awake()
    {
        currentStats = baseStats;
        BuildStatsCache();
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
        BuildStatsCache();
    }
}
