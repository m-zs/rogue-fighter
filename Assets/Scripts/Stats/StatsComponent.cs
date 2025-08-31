using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
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

public class StatModifier
{
    public Stat stat;
    public float value;
}

public class StatsComponent : MonoBehaviour
{
    [SerializedDictionary("Stat", "StatProcessorMapping")] public SerializedDictionary<Stat, StatProcessorMapping> statProcessors = new();
    [SerializeField] private Stats baseStats;

    private List<StatModifier> modifiers = new();

    private Stats currentStats = new();
    private Stats statsCache = new();

    [Serializable]
    public class StatProcessorMapping
    {
        public Stat stat;
        [SerializeReference] public StatProcessor processor;
    }

    public void AddModifiers(List<StatModifier> newModifiers)
    {
        modifiers.AddRange(newModifiers);
        BuildStatsCache();
    }

    public void RemoveModifiers(List<StatModifier> appliedModifiers)
    {
        foreach (var statModifier in modifiers.ToList())
        {
            modifiers.Remove(statModifier);
        }
        BuildStatsCache();
    }

    private void BuildStatsCache()
    {
        foreach (var statProcessorMapping in statProcessors)
        {
            var statProcessor = statProcessorMapping.Value;
            switch (statProcessor.stat)
            {
                case Stat.Strength:
                    statsCache.Strength = CalculateStat(statProcessor.processor, Stat.Strength);
                    break;
                case Stat.Dexterity:
                    statsCache.Dexterity = CalculateStat(statProcessor.processor, Stat.Dexterity);
                    break;
                case Stat.Intelligence:
                    statsCache.Intelligence = CalculateStat(statProcessor.processor, Stat.Intelligence);
                    break;
                case Stat.Endurance:
                    statsCache.Endurance = CalculateStat(statProcessor.processor, Stat.Endurance);
                    break;
                case Stat.Luck:
                    statsCache.Luck = CalculateStat(statProcessor.processor, Stat.Luck);
                    break;
                case Stat.Size:
                    statsCache.Size = CalculateStat(statProcessor.processor, Stat.Size);
                    break;
                case Stat.MovementSpeed:
                    statsCache.MovementSpeed = CalculateStat(statProcessor.processor, Stat.MovementSpeed);
                    break;
                case Stat.MaxPhysicalDefence:
                    statsCache.MaxPhysicalDefence = CalculateStat(statProcessor.processor, Stat.MaxPhysicalDefence);
                    break;
                case Stat.PhysicalDefence:
                    statsCache.PhysicalDefence = CalculateStat(statProcessor.processor, Stat.PhysicalDefence);
                    break;
                case Stat.MaxElementalDefence:
                    statsCache.MaxElementalDefence = CalculateStat(statProcessor.processor, Stat.MaxElementalDefence);
                    break;
                case Stat.ElementalDefence:
                    statsCache.ElementalDefence = CalculateStat(statProcessor.processor, Stat.ElementalDefence);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            ;
        }
    }

    private int CalculateStat(StatProcessor processor, Stat stat)
    {
        var value = processor.Compute(currentStats);
        foreach (var statModifier in modifiers)
        {
            if (statModifier.stat == stat)
            {
                value += statModifier.value;
            }
        }

        return Mathf.CeilToInt(value);
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
                currentStats.Strength += amount;
                break;
            case Stat.Dexterity:
                currentStats.Dexterity += amount;
                break;
            case Stat.Intelligence:
                currentStats.Intelligence += amount;
                break;
            case Stat.Endurance:
                currentStats.Endurance += amount;
                break;
            case Stat.Luck:
                currentStats.Luck += amount;
                break;
            case Stat.Size:
                currentStats.Size += amount;
                break;
            case Stat.MovementSpeed:
                currentStats.MovementSpeed += amount;
                break;
            case Stat.MaxPhysicalDefence:
                currentStats.MaxPhysicalDefence += amount;
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
