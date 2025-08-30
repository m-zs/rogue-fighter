using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Operation
{
    Add,
    Subtract,
    Multiply,
    Divide,
    Power,
    Clamp
}

[System.Serializable]
public abstract class OperationPayload
{
    public Operation operation;

    public abstract float GetValue(Stats stats);
}

[System.Serializable]
public class OperationStatPayload : OperationPayload
{
    public Stat stat;
    
    public override float GetValue(Stats stats)
    {
        return stat switch
        {
            Stat.Strength => stats.Strength,
            Stat.Dexterity => stats.Dexterity,
            Stat.Intelligence => stats.Intelligence,
            Stat.Endurance => stats.Endurance,
            Stat.Luck => stats.Luck,
            Stat.Size => stats.Size,
            Stat.MovementSpeed => stats.MovementSpeed,
            Stat.MaxPhysicalDefence => stats.MaxPhysicalDefence,
            Stat.PhysicalDefence => stats.PhysicalDefence,
            Stat.MaxElementalDefence => stats.MaxElementalDefence,
            Stat.ElementalDefence => stats.ElementalDefence,
            _ => throw new ArgumentOutOfRangeException(nameof(stat), stat, null)
        };
    }
}

[System.Serializable]
public class OperationValuePayload : OperationPayload
{
    public int value;

    public override float GetValue(Stats stats)
    {
        return value;
    }
}

internal static class StatProcessorUtils
{
    public static float ApplyOperation(float a, float b, Operation operation)
    {
        return operation switch
        {
            Operation.Add => a + b,
            Operation.Subtract => a - b,
            Operation.Multiply => a * b,
            Operation.Divide => a / b,
            Operation.Power => Mathf.Pow(a, b),
            Operation.Clamp => Mathf.Clamp(a, 0, b),
            _ => throw new ArgumentOutOfRangeException(nameof(operation), operation, null)
        };
    }
}

[System.Serializable]
public class CombinedPayload : OperationPayload
{
    [SerializeReference] public List<OperationPayload> operations = new List<OperationPayload>();

    public override float GetValue(Stats stats)
    {
        const float result = 0f;
        if (operations == null || operations.Count == 0) return result;
        return operations.Aggregate(result, (current, op) => StatProcessorUtils.ApplyOperation(current, op.GetValue(stats), op.operation));
    }
}

[CreateAssetMenu(fileName = "StatProcessor", menuName = "Scriptable Objects/StatProcessor")]
public class StatProcessor : ScriptableObject
{
    [SerializeField] public float defaultValue = 0;
    [SerializeReference] public List<OperationPayload> operations = new List<OperationPayload>();
    
    public float Compute(Stats stats)
    {
        var result = defaultValue;
        if (operations == null || operations.Count == 0) return result;
        return operations.Aggregate(result, (current, operation) => StatProcessorUtils.ApplyOperation(current, operation.GetValue(stats), operation.operation));
    }
}
